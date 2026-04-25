using UnityEngine;

public class StageManager : MonoBehaviour
{
    // stage 클리어 시 보상 설정용 -> 추후 구현 예정
    [SerializeField] private StageData stageData;
    [SerializeField] private StageArea[] areas;
    [SerializeField] private TruckController truck;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private BarricadeHPBar barricadeHPBar;
    [SerializeField] private PlayTimeUI playTimeUI;
    [SerializeField] private StageClearUI stageClearUI;

    private int currentAreaIndex = -1;
    private StageArea currentArea;

    private void Start()
    {
        if (playTimeUI != null)
            playTimeUI.StartTimer();

        StartArea(0);
    }

    private void StartArea(int index)
    {
        if (index >= areas.Length)
        {
            StageClear();
            return;
        }

        currentAreaIndex = index;
        currentArea = areas[currentAreaIndex];

        currentArea.EnterArea();

        if (enemySpawner != null && truck != null)
            enemySpawner.StartSpawn(currentArea, truck.transform);

        if (currentArea.TruckStopZone != null)
        {
            currentArea.TruckStopZone.OnTruckReached += HandleTruckReachedStopZone;
        }

        if (truck != null && currentArea.TruckStopZone != null)
        {
            truck.MoveTo(currentArea.TruckStopZone);
        }

        if (currentArea.HasBarricade)
            currentArea.Barricade.OnDestroyed += HandleBarricadeDestroyed;

        Debug.Log($"[StageManager] Area {currentAreaIndex + 1} 시작");
    }

    private void HandleTruckReachedStopZone()
    {
        if (currentArea == null) return;

        if (!currentArea.HasBarricade)
        {
            EndCurrentArea();
            StageClear();
            return;
        }

        currentArea.Barricade.SetDamageable(true);

        if (barricadeHPBar != null)
            barricadeHPBar.Bind(currentArea.Barricade);
    }

    private void EndCurrentArea()
    {
        if (currentArea == null)
            return;

        if (currentArea.HasBarricade)
        {
            currentArea.Barricade.SetDamageable(false);
            currentArea.Barricade.OnDestroyed -= HandleBarricadeDestroyed;
        }

        if (enemySpawner != null)
            enemySpawner.StopSpawn();

        if (currentArea.TruckStopZone != null)
            currentArea.TruckStopZone.OnTruckReached -= HandleTruckReachedStopZone;

        if (barricadeHPBar != null)
            barricadeHPBar.Unbind();

        currentArea.ExitArea();

        Debug.Log($"[StageManager] Area {currentAreaIndex + 1} 종료");
    }

    private void HandleBarricadeDestroyed()
    {
        EndCurrentArea();
        StartArea(currentAreaIndex + 1);
    }

    private void StageClear()
    {
        if (enemySpawner != null) enemySpawner.StopSpawn();

        if (ObjectPool.Instance != null) ObjectPool.Instance.gameObject.SetActive(false);
        if (GameManager.Instance != null) GameManager.Instance.PlayerEnabled(false);

        if (playTimeUI != null) playTimeUI.StopTimer();

        if (stageClearUI != null)
        {
            float clearTime = playTimeUI != null ? playTimeUI.CurrentTime : 0f;
            stageClearUI.Show(clearTime);
        }
    }
}