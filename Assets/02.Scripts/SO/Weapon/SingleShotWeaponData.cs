using UnityEngine;

// 피스톨, 라이플처럼 한 번에 한 발씩 쏘는 무기 데이터.
[CreateAssetMenu(fileName = "SingleShotWeaponData", menuName = "Game/Weapon/SingleShot")]
public class SingleShotWeaponData : NewWeaponData
{
    [SerializeField] private float projectileSpeed = 10f;   // 투사체 속도

    public float ProjectileSpeed => projectileSpeed;
}