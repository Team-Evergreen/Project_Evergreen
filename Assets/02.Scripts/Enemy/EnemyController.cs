using UnityEngine;
using Utils.ClassUtility;

public class EnemyController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public EnemyData enemyData;
    private Transform target;
    private float currentHp;

    [Header("Attack Settings")]
    private float attackRange = 0.8f;
    private float attackCooldown = 1f;
    private float lastAttackTime;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        enemyData = DataManager.Instance.LoadJson<EnemyDataList>(DataManager.Instance.enemyDataFileName).Enemys[0];
    }

    // 스폰될 때 초기화하는 함수
    public void Setup(Transform _target)
    {
        if (enemyData == null)
        {
            Debug.LogError($"{gameObject.name}에 EnemyData가 할당되지 않았습니다!");
            return;
        }

        target = _target;
        currentHp = enemyData.hp;
    }

    private void Update()
    {
        if (target == null) 
            return;

        // 거리의 제곱값을 구함 (루트 연산이 없어 훨씬 빠름)
        float sqrDistance = (target.position - transform.position).sqrMagnitude;

        // 비교 대상인 사거리도 제곱해서 비교
        if (sqrDistance <= attackRange * attackRange)
        {
            OnReachedTarget();
        }
        else
        {
            Move();
        }
    }

    // 이동
    private void Move()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        transform.Translate(direction * enemyData.speed * Time.deltaTime);

        if (direction.x != 0)
        {
            spriteRenderer.flipX = direction.x < 0;
        }
    }

    // 목적지 도착
    void OnReachedTarget()
    {
        Debug.Log("적이 플레이어에게 도달하여 공격을 시작합니다!");
    }

    // 공격 받음
    public void TakeDamage(float _amount)
    {
        currentHp -= _amount;
        if (currentHp <= 0) Die();
    }

    // 사망
    private void Die()
    {
        // 경험치 아이템 드랍 로직 등을 여기에 추가
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 쿨타임이 지났는지 확인
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                // 플레이어 스크립트의 TakeDamage 호출
                // collision.GetComponent<PlayerStats>().TakeDamage(damage);

                lastAttackTime = Time.time;
                Debug.Log("플레이어에게 대미지를 입혔습니다!");
            }
        }
    }
}
