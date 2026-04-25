using System;
using UnityEngine;

public class Barricade : DamageableEntity
{
    [SerializeField] private int maxHP = 100;

    private float currentHP;
    private bool canTakeDamage;

    public event Action<float, float> OnHPChanged;
    public event Action OnDestroyed;

    public float CurrentHP => currentHP;
    public float MaxHP => maxHP;

    public void Init(int maxHP)
    {
        this.maxHP = maxHP;
        currentHP = maxHP;
        canTakeDamage = false;

        gameObject.SetActive(true);
        OnHPChanged?.Invoke(currentHP, this.maxHP);
    }

    public void SetDamageable(bool value)
    {
        canTakeDamage = value;
    }

    public override void TakeDamage(float damage)
    {
        if (!canTakeDamage)
            return;

        if (currentHP <= 0f)
            return;

        currentHP = Mathf.Max(0f, currentHP - damage);
        OnHPChanged?.Invoke(currentHP, maxHP);

        if (currentHP <= 0f)
            DestroyBarricade();
    }

    private void DestroyBarricade()
    {
        gameObject.SetActive(false);
        OnDestroyed?.Invoke();
    }
}