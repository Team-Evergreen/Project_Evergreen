using UnityEngine;

// 도끼 데이터
// 부채꼴 범위 공격

[CreateAssetMenu(fileName = "AxeWeaponData", menuName = "Game/Weapon/AxeWeaponData")]
public class AxeWeaponData : NewWeaponData
{
    [SerializeField] private float attackRadius = 2f;
    [SerializeField] private float attackAngle = 90f;

    public float AttackRadius => attackRadius;
    public float AttackAngle => attackAngle;
}