using UnityEngine;

public class StageArea : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private StageAreaData areaData;

    [Header("References")]
    [SerializeField] private Barricade barricade;
    [SerializeField] private TruckStopZone truckStopZone;
    [SerializeField] private BoxCollider2D[] spawnZones;

    public StageAreaData AreaData => areaData;
    public Barricade Barricade => barricade;
    public TruckStopZone TruckStopZone => truckStopZone;
    public BoxCollider2D[] SpawnZones => spawnZones;

    public bool HasBarricade =>
        areaData != null && areaData.hasBarricade && barricade != null;

    public EnemySpawnRuleGroup SpawnRuleGroup =>
        areaData != null ? areaData.enemySpawnRuleGroup : null;

    public void EnterArea()
    {
        gameObject.SetActive(true);

        if (truckStopZone != null)
            truckStopZone.SetWaiting();

        if (HasBarricade)
        {
            barricade.Init(areaData.barricadeMaxHP);
        }
        else if (barricade != null)
        {
            barricade.gameObject.SetActive(false);
        }

        Debug.Log($"[Area Enter] {areaData.areaName}");
    }

    public void ExitArea()
    {
        if (barricade != null)
            barricade.gameObject.SetActive(false);

        Debug.Log($"[Area Exit] {areaData.areaName}");
    }
}