using UnityEngine;

[System.Serializable]
public class EnemySpawnRule
{
    // Enmey Data Json Id
    public int enemyId;
    public float spawnInterval;
    public int[] spawnCountsByZone;
}
