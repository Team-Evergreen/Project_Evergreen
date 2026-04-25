using UnityEngine;
using Utils.ClassUtility;

public class EnemyController : DamageableEntity
{
    [Header("References")]
    [SerializeField] private EnemyHPBar enemyHPBar;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Transform target;


    [Header("Runtime Data")]
    private EnemyData enemyData;
    private float currentHp;


    [Header("Attack Settings")]
    private float attackRange = 0.5f;
    private float attackCooldown = 1f;
    public float currentTime = 0f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (enemyHPBar == null) enemyHPBar = GetComponentInChildren<EnemyHPBar>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void Setup(Transform target, Vector2 spawnPosition, EnemyData enemyData)
    {
        this.target = target;
        this.enemyData = enemyData;

        transform.position = spawnPosition;
        currentHp = enemyData.maxHP;
        transform.localScale = Vector3.one * enemyData.scale;

        gameObject.SetActive(true);

        if (enemyHPBar != null)
            enemyHPBar.Init(enemyData.maxHP);
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
        currentHp = Mathf.Max(0f, currentHp - _damage);
        enemyHPBar.SetHP(currentHp);

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
