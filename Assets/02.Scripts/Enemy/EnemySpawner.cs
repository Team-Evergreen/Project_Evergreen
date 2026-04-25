using System.Collections.Generic;
using UnityEngine;
using Utils.ClassUtility;

public class EnemySpawner : MonoBehaviour
{
    private class RuntimeSpawnRule
    {
        public EnemySpawnRule rule;
        public float timer;
    }

    private StageArea currentArea;
    private Transform target;

    private readonly List<RuntimeSpawnRule> runtimeRules = new();

    private List<EnemyData> enemyDataList = new();
    private bool isSpawning;

    private void Start()
    {
        if (!TryLoadEnemyData())
            Debug.LogWarning("EnemyData 로드 실패");
    }

    private void Update()
    {
        if (!isSpawning || currentArea == null || target == null)
            return;

        for (int i = 0; i < runtimeRules.Count; i++)
        {
            RuntimeSpawnRule runtimeRule = runtimeRules[i];
            EnemySpawnRule rule = runtimeRule.rule;

            runtimeRule.timer += Time.deltaTime;

            if (runtimeRule.timer < rule.spawnInterval)
                continue;

            runtimeRule.timer = 0f;
            SpawnByRule(rule);
        }
    }

    public void StartSpawn(StageArea area, Transform target)
    {
        StopSpawn();

        if (enemyDataList == null || enemyDataList.Count == 0)
        {
            if (!TryLoadEnemyData())
            {
                Debug.LogWarning("EnemyData 로드 실패");
                return;
            }
        }

        currentArea = area;
        this.target = target;

        if (currentArea == null || currentArea.SpawnRuleGroup == null)
        {
            Debug.LogWarning("SpawnRuleGroup이 없습니다.");
            return;
        }

        foreach (EnemySpawnRule rule in currentArea.SpawnRuleGroup.spawnRules)
        {
            runtimeRules.Add(new RuntimeSpawnRule
            {
                rule = rule,
                timer = 0f
            });
        }

        isSpawning = true;
    }

    public void StopSpawn()
    {
        isSpawning = false;
        runtimeRules.Clear();

        currentArea = null;
        target = null;
    }

    private void SpawnByRule(EnemySpawnRule rule)
    {
        EnemyData enemyData = GetEnemyData(rule.enemyId);

        if (enemyData == null || enemyData.maxHP <= 0f)
        {
            Debug.LogWarning($"EnemyData 없음 또는 hp 이상함. enemyId: {rule.enemyId}");
            return;
        }

        int zoneIndex = GetRandomSpawnZoneIndex();

        if (zoneIndex < 0)
        {
            Debug.LogWarning("사용 가능한 SpawnZone이 없습니다.");
            return;
        }

        BoxCollider2D selectedZone = currentArea.SpawnZones[zoneIndex];

        int spawnCount = GetSpawnCountByZone(rule, zoneIndex);

        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 spawnPosition = GetRandomPositionInZone(selectedZone);

            EnemyController enemy = ObjectPool.Instance.GetEnemy();
            if (enemy == null)
                continue;

            enemy.Setup(target, spawnPosition, enemyData);
        }
    }

    private int GetRandomSpawnZoneIndex()
    {
        if (currentArea.SpawnZones == null || currentArea.SpawnZones.Length == 0)
            return -1;

        return Random.Range(0, currentArea.SpawnZones.Length);
    }

    private int GetSpawnCountByZone(EnemySpawnRule rule, int zoneIndex)
    {
        if (rule.spawnCountsByZone == null || rule.spawnCountsByZone.Length == 0)
            return 0;

        if (zoneIndex < 0 || zoneIndex >= rule.spawnCountsByZone.Length)
        {
            Debug.LogWarning($"SpawnCount 설정 부족. enemyId: {rule.enemyId}, zoneIndex: {zoneIndex}");
            return 0;
        }

        return rule.spawnCountsByZone[zoneIndex];
    }

    private Vector2 GetRandomPositionInZone(BoxCollider2D zone)
    {
        Vector2 size = zone.size;
        Vector2 offset = zone.offset;

        float randomX = Random.Range(-size.x * 0.5f, size.x * 0.5f);
        float randomY = Random.Range(-size.y * 0.5f, size.y * 0.5f);

        Vector3 localPoint = new Vector3(
            offset.x + randomX,
            offset.y + randomY,
            0f
        );

        return zone.transform.TransformPoint(localPoint);
    }

    private EnemyData GetEnemyData(int enemyId)
    {
        for (int i = 0; i < enemyDataList.Count; i++)
        {
            if (enemyDataList[i].id == enemyId)
                return enemyDataList[i];
        }

        return null;
    }

    private bool TryLoadEnemyData()
    {
        if (DataManager.Instance == null)
            return false;

        EnemyDataList dataList =
            DataManager.Instance.LoadJson<EnemyDataList>(DataManager.Instance.enemyDataFileName);

        if (dataList == null || dataList.Enemys == null || dataList.Enemys.Count == 0)
            return false;

        enemyDataList = dataList.Enemys;

        return enemyDataList.Count > 0;
    }
}