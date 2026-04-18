using UnityEngine;

public interface IPlayerAttackStrategy
{
    void Attack(PlayerShooting _shooting, NewWeaponData _weaponData);
}

public class PlayerShooting : MonoBehaviour
{
    public NewWeaponData currentWeaponData;
    public Transform FirePoint;
    public LineRenderer sniperLine;
    public LayerMask enemyLayer;

    private PlayerController playerController;
    private float timer;

    private IPlayerAttackStrategy currentAttackStrategy;

    private SingleShotAttackStrategy singleShotStrategy;
    private ShotgunAttackStrategy shotgunStrategy;
    private SniperAttackStrategy sniperStrategy;
    private AxeAttackStrategy axeStrategy;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        if (FirePoint == null) FirePoint = transform.Find("FirePoint");
        EnsureEnemyLayerMask();
    }

    private void Start()
    {
        ObjectPool objectPool = ObjectPool.Instance;

        singleShotStrategy = new SingleShotAttackStrategy(objectPool);
        shotgunStrategy = new ShotgunAttackStrategy(objectPool);
        sniperStrategy = new SniperAttackStrategy();
        axeStrategy = new AxeAttackStrategy();

        SetWeapon(currentWeaponData);
    }

    private void Update()
    {
        if (currentWeaponData == null || currentAttackStrategy == null) return;

        timer += Time.deltaTime;

        if (timer >= currentWeaponData.AttackInterval)
        {
            Fire();
            timer = 0f;
        }
    }

    public Vector2 ShootDirection()
    {
        Vector2 lookDir = playerController != null ? playerController.LookDirection : Vector2.right;

        if (lookDir == Vector2.zero) lookDir = Vector2.right;

        return lookDir.normalized;
    }

    public void Fire()
    {
        if (currentWeaponData == null || currentAttackStrategy == null) return;

        currentAttackStrategy.Attack(this, currentWeaponData);
    }

    public void SetWeapon(NewWeaponData _weaponData)
    {
        currentWeaponData = _weaponData;
        currentAttackStrategy = CreateStrategyByWeapon(_weaponData);
        timer = 0f;

        if (sniperLine != null)
        {
            sniperLine.positionCount = 0;
            sniperLine.gameObject.SetActive(false);
        }
    }

    private void EnsureEnemyLayerMask()
    {
        if (enemyLayer.value != 0) return;

        int enemyMask = LayerMask.GetMask("Enemy");
        if (enemyMask != 0)
        {
            enemyLayer = enemyMask;
        }
        else
        {
            Debug.LogWarning("Enemy layer mask is empty and no Enemy layer exists.");
        }
    }

    private IPlayerAttackStrategy CreateStrategyByWeapon(NewWeaponData _weaponData)
    {
        if (_weaponData is SingleShotWeaponData) return singleShotStrategy;
        if (_weaponData is ShotgunWeaponData) return shotgunStrategy;
        if (_weaponData is SniperWeaponData) return sniperStrategy;
        if (_weaponData is AxeWeaponData) return axeStrategy;
        return null;
    }
}
