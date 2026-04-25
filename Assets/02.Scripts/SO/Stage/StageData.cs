using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Stage/StageData")]
public class StageData : ScriptableObject
{
    public string stageName;
    public List<StageAreaData> areas = new();

    // TODO : 스테이지 클리어 시 추후 보상관련 데이터 추가
}