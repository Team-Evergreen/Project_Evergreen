using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [Header("Projectile Pool")]
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private int projectilePoolSize = 30;

    [Header("Enemy Pool")]
    [SerializeField] private EnemyController enemyPrefab;
    [SerializeField] private int enemyPoolSize = 30;

    private readonly Queue<Projectile> projectilePool = new Queue<Projectile>();
    private readonly Queue<EnemyController> enemyPool = new Queue<EnemyController>();

    private Transform projectilePoolRoot;
    private Transform enemyPoolRoot;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        CreateProjectilePool();
        CreateEnemyPool();
    }

    private void CreateProjectilePool()
    {
        if (projectilePrefab == null) return;

        GameObject projectileRootObject = new GameObject("ProjectilePool");
        projectileRootObject.transform.SetParent(transform);
        projectilePoolRoot = projectileRootObject.transform;

        for (int i = 0; i < projectilePoolSize; i++)
        {
            Projectile projectile = CreateNewProjectile();
            projectile.gameObject.SetActive(false);
            projectilePool.Enqueue(projectile);
        }
    }

    private void CreateEnemyPool()
    {
        if (enemyPrefab == null) return;

        GameObject enemyRootObject = new GameObject("EnemyPool");
        enemyRootObject.transform.SetParent(transform);
        enemyPoolRoot = enemyRootObject.transform;

        for (int i = 0; i < enemyPoolSize; i++)
        {
            EnemyController enemy = CreateNewEnemy();
            enemy.gameObject.SetActive(false);
            enemyPool.Enqueue(enemy);
        }
    }

    private Projectile CreateNewProjectile()
    {
        return Instantiate(projectilePrefab, projectilePoolRoot);
    }

    private EnemyController CreateNewEnemy()
    {
        return Instantiate(enemyPrefab, enemyPoolRoot);
    }

    public Projectile GetProjectile()
    {
        Projectile projectile;

        if (projectilePool.Count > 0)
        {
            projectile = projectilePool.Dequeue();
        }
        else
        {
            projectile = CreateNewProjectile();
            projectile.gameObject.SetActive(false);
            Debug.Log("ProjectilePool 부족으로 새로 생성");
        }

        return projectile;
    }

    public void ReturnProjectile(Projectile _projectile)
    {
        if (_projectile == null) return;

        if (_projectile.gameObject.activeSelf) _projectile.gameObject.SetActive(false);
        projectilePool.Enqueue(_projectile);
    }

    public EnemyController GetEnemy()
    {
        EnemyController enemy;

        if (enemyPool.Count > 0)
        {
            enemy = enemyPool.Dequeue();
        }
        else
        {
            enemy = CreateNewEnemy();
            enemy.gameObject.SetActive(false);
            Debug.Log("EnemyPool 부족으로 새로 생성");
        }

        return enemy;
    }

    public void ReturnEnemy(EnemyController _enemy)
    {
        if (_enemy == null) return;

        if (_enemy.gameObject.activeSelf) _enemy.gameObject.SetActive(false);
        enemyPool.Enqueue(_enemy);
    }
}
