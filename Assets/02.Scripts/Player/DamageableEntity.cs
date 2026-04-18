using UnityEngine;

// 데미지를 받을 수 있는 대상 공통 부모
// Enemy, Player 상속해서 사용중

public abstract class DamageableEntity : MonoBehaviour
{
    public abstract void TakeDamage(float _damage);
}