using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Stage/EnemySpawnRuleGroup")]
public class EnemySpawnRuleGroup : ScriptableObject
{
    public List<EnemySpawnRule> spawnRules = new();
}