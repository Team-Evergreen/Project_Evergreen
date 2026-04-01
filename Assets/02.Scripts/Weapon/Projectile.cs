using UnityEngine;

// 투사체 기본 동작
public class Projectile : MonoBehaviour
{
    private Vector2 direction;
    private float damage;
    private float speed;

    public void Setup(float _damage, float _speed, float _lifeTime)
    {
        damage = _damage;
        speed = _speed;
        // 5초 뒤 자동 소멸
        Destroy(gameObject, _lifeTime);
    }

    private void Update()
    {
        Debug.Log("총알 이동 중...");
        // 앞으로 전진
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.CompareTag("Enemy"))
        {
            if (_collision.TryGetComponent(out EnemyController enemy))
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}