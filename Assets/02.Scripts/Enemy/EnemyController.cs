using UnityEngine;
using Utils.ClassUtility;

public class EnemyController : DamageableEntity
{
    private SpriteRenderer spriteRenderer;
    public EnemyData enemyData;
    private Rigidbody2D rb;
    private Transform target;
    private float currentHp;

    [Header("Attack Settings")]
    private float attackRange = 0.5f;
    private float attackCooldown = 1f;
    public float currentTime = 0f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        TryLoadEnemyData();
    }

    public void Setup(Transform _target, Vector2 _spawnPos)
    {
        if ((enemyData == null || enemyData.hp <= 0f) && !TryLoadEnemyData())
        {
            Debug.LogError($"{gameObject.name}에 EnemyData가 할당되지 않았습니다!");
            return;
        }

        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

        transform.position = _spawnPos;
        target = _target;
        currentHp = enemyData.hp;
        currentTime = 0f;

        if (rb != null) rb.linearVelocity = Vector2.zero;
        if (!gameObject.activeSelf) gameObject.SetActive(true);
    }

    private bool TryLoadEnemyData()
    {
        if (DataManager.Instance == null) return false;

        EnemyDataList dataList = DataManager.Instance.LoadJson<EnemyDataList>(DataManager.Instance.enemyDataFileName);
        if (dataList == null || dataList.Enemys == null || dataList.Enemys.Count == 0)
            return false;

        enemyData = dataList.Enemys[0];
        return enemyData != null && enemyData.hp > 0f;
    }

    private void FixedUpdate()
    {
        if (target == null)
            return;

        float sqrDistance = (target.position - transform.position).sqrMagnitude;

        if (sqrDistance <= attackRange * attackRange)
        {
            OnReachedTarget();
        }
        else
        {
            Move();
        }
    }

    private void Move()
    {
        if (rb == null || enemyData == null) return;

        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * enemyData.speed;

        if (spriteRenderer != null && direction.x != 0f)
        {
            spriteRenderer.flipX = direction.x < 0f;
        }
    }

    private void OnReachedTarget()
    {
        if (rb != null) rb.linearVelocity = Vector2.zero;
    }

    public override void TakeDamage(float _damage)
    {
        currentHp -= _damage;
        if (currentHp <= 0f) Die();
    }

    private void Die()
    {
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        target = null;
        currentTime = 0f;
        if (rb != null) rb.linearVelocity = Vector2.zero;

        if (ObjectPool.Instance != null)
            ObjectPool.Instance.ReturnEnemy(this);
    }

    private void OnTriggerStay2D(Collider2D _collision)
    {
        if (_collision.CompareTag("Player"))
        {
            currentTime += Time.deltaTime;

            if (currentTime >= attackCooldown)
            {
                currentTime = 0.0f;
                if (_collision.TryGetComponent(out DamageableEntity target))
                    target.TakeDamage(enemyData.damage);
            }
        }
    }
}
