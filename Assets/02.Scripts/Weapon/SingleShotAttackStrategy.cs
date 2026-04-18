using UnityEngine;

// 피스톨, 라이플처럼 한 발 발사하는 공격 전략
public class SingleShotAttackStrategy : IPlayerAttackStrategy
{
    private readonly ObjectPool objectPool;
    public SingleShotAttackStrategy(ObjectPool _objectPool)
    {
        this.objectPool = _objectPool;
    }

    public void Attack(PlayerShooting _shooting, NewWeaponData _weaponData)
    {
        SingleShotWeaponData data = _weaponData as SingleShotWeaponData;
        if (data == null) return;

        Vector2 lookDir = _shooting.ShootDirection();
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        Projectile projectile = objectPool.GetProjectile();
        if (projectile == null) return;

        projectile.Setup(_shooting.enemyLayer, data.Damage, data.ProjectileSpeed, _shooting.FirePoint.position, angle);
        projectile.gameObject.SetActive(true);
    }
}
