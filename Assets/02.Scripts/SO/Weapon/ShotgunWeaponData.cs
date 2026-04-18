using UnityEngine;

// 샷건 데이터
// 여러 발을 퍼지게 발사
[CreateAssetMenu(fileName = "ShotgunWeaponData", menuName = "Game/Weapon/Shotgun")]
public class ShotgunWeaponData : NewWeaponData
{
    [SerializeField] private float projectileSpeed = 9f;    // 투사체 속도
    [SerializeField] private int projectileCount = 3;   // 발사되는 총알 개수
    [SerializeField] private float spreadAngle = 30f;   // 총구 기준 총알이 나가는 각도

    public float ProjectileSpeed => projectileSpeed;
    public int ProjectileCount => projectileCount;
    public float SpreadAngle => spreadAngle;
}