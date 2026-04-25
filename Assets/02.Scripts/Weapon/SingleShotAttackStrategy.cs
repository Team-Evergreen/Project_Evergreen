using UnityEngine;

// 피스톨, 라이플처럼 한 발 발사하는 공격 전략
public class SingleShotAttackStrategy : IPlayerAttackStrategy
{
    private readonly ObjectPool objectPool;
    public SingleShotAttackStrategy(ObjectPool _objectPool)
    {
        this.objectPool = _objectPool;
    }

    public void Attack(PlayerWeaponController _weaponController, NewWeaponData _weaponData)
    {
        SingleShotWeaponData data = _weaponData as SingleShotWeaponData;
        if (data == null) return;

        Vector2 lookDir = _weaponController.ShootDirection();
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        Projectile projectile = objectPool.GetProjectile();
        if (projectile == null) return;

        projectile.Setup(_weaponController.enemyLayer, data.Damage, data.ProjectileSpeed, _weaponController.FirePoint.position, angle);
        projectile.gameObject.SetActive(true);
    }
}
