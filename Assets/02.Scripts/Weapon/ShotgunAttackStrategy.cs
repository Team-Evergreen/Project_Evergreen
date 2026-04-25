using Unity.VisualScripting;
using UnityEngine;

// 샷건 공격 전략
// 여러 발을 퍼지게 발사

public class ShotgunAttackStrategy : IPlayerAttackStrategy
{
    private readonly ObjectPool objectPool;
    private readonly int minCountBullet = 3;

    public ShotgunAttackStrategy(ObjectPool _objectPool)
    {
        this.objectPool = _objectPool;
    }

    public void Attack(PlayerWeaponController _weaponController, NewWeaponData _weaponData)
    {
        ShotgunWeaponData data = _weaponData as ShotgunWeaponData;
        if (data == null) return;

        int count = Mathf.Max(minCountBullet, data.ProjectileCount);

        // 전체 퍼짐 각도의 절반만큼 아래쪽부터 시작
        float startAngle = -data.SpreadAngle * 0.5f;

        // 총알 수에 맞춰 전체 범위를 균등 분배
        float angleStep = count > 1 ? data.SpreadAngle / (count - 1) : 0f;

        for (int i = 0; i < count; i++)
        {
            float offsetAngle = startAngle + angleStep * i;

            // 현재 바라보는 방향을 기준으로 offsetAngle 만큼 회전
            Vector2 shotDirection = RotateVector(_weaponController.ShootDirection(), offsetAngle);
            float angle = Mathf.Atan2(shotDirection.y, shotDirection.x) * Mathf.Rad2Deg;

            Projectile projectile = objectPool.GetProjectile();
            if (projectile == null) continue;

            projectile.Setup(_weaponController.enemyLayer, data.Damage, data.ProjectileSpeed, _weaponController.FirePoint.position, angle);
            projectile.gameObject.SetActive(true);
        }
    }

    // 기준 방향 벡터를 angle만큼 회전시킴
    private Vector2 RotateVector(Vector2 _direction, float _angle)
    {
        float rad = _angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        return new Vector2(_direction.x * cos - _direction.y * sin, _direction.x * sin + _direction.y * cos).normalized;
    }
}
