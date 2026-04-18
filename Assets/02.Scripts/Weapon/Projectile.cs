using UnityEngine;

// 투사체 기본 동작
public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifeTime = 3f;
    private LayerMask targetLayer;
    private float damage;
    private float speed;
    private float timer;

    public void Setup(LayerMask _targetLayer, float _damage, float _speed, Vector3 _position, float _angle)
    {
        targetLayer = _targetLayer;
        damage = _damage;
        speed = _speed;
        transform.position = _position;
        transform.rotation = Quaternion.Euler(0f, 0f, _angle);
        timer = 0f;
    }

    private void Update()
    {
        // Debug.Log("총알 이동 중...");
        // 앞으로 전진
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        timer += Time.deltaTime;
        if (timer >= lifeTime) { ReturnToPool(); }
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if ((targetLayer.value & (1 << _collision.gameObject.layer)) == 0) return;
        if (_collision.TryGetComponent(out DamageableEntity target))
        {
            target.TakeDamage(damage);
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        ObjectPool.Instance.ReturnProjectile(this);
    }
}
