
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Stage/StageAreaData")]
public class StageAreaData : ScriptableObject
{
    public string areaName;
    public int barricadeMaxHP = 100;
    public bool hasBarricade = true;

    public EnemySpawnRuleGroup enemySpawnRuleGroup;
}