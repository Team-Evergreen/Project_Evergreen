using UnityEngine;

// 모든 무기의 최상위 공통 데이터
public class NewWeaponData : ScriptableObject
{
    [Header("공통 정보")]
    [SerializeField] private string weaponId;   // 내부 식별용 ID
    [SerializeField] private string displayName;    // 표시 이름
    [SerializeField] private Sprite icon;   // 아이콘
    [SerializeField] private float damage = 1f;
    [SerializeField] private float attackInterval = 1f;

    public string WeaponId => weaponId;
    public string DisplayName => displayName;
    public Sprite Icon => icon;
    public float Damage => damage;
    public float AttackInterval => attackInterval;
}
