using UnityEngine;
using Utils.EnumType;

[CreateAssetMenu(fileName = "WeaponData", menuName = "SO/WeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("Basic Info")]
    public string weaponName;
    public WeaponType weaponType;
    public EquipTaype equipTarget;

    [Header("Attack Settings")]
    public float speed;   // 공격 속도
    public float range;   // 공격 범위 (근거리면 범위, 원거리면 사거리)
    public float damage;  // 공격력
    public float fireRate;// 공격 주기
    public float lifetime;// 투사체 수명 (원거리일 때만 사용)

    public bool isMelee; // true면 근거리, false면 원거리

    public GameObject projectilePrefab; // 원거리일 때만 사용
    public RuntimeAnimatorController weaponAnim; // 무기별 전용 애니메이션
}