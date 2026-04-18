using Cysharp.Threading.Tasks;
using UnityEngine;

public class SniperAttackStrategy : IPlayerAttackStrategy
{
    private bool isCharging;
    private float nextAttackTime;

    public void Attack(PlayerShooting _shooting, NewWeaponData _weaponData)
    {
        SniperWeaponData data = _weaponData as SniperWeaponData;
        if (data == null || isCharging || Time.time < nextAttackTime) return;

        RunSniperAttack(_shooting, data).Forget();
    }

    private async UniTaskVoid RunSniperAttack(PlayerShooting _shooting, SniperWeaponData _data)
    {
        isCharging = true;
        bool fired = false;
        LineRenderer line = _shooting != null ? _shooting.sniperLine : null;

        try
        {
            if (line != null)
            {
                line.gameObject.SetActive(true);
                line.positionCount = 2;
            }

            float duration = Mathf.Max(0f, _data.ChargeDuration);
            float elapsed = 0f;

            while (elapsed < duration)
            {
                if (!CanContinue(_shooting, _data)) return;

                elapsed += Time.deltaTime;
                float t = duration > 0f ? Mathf.Clamp01(elapsed / duration) : 1f;
                DrawChargeLine(_shooting, _data, line, t);

                await UniTask.Yield(PlayerLoopTiming.Update);
            }

            if (!CanContinue(_shooting, _data)) return;

            DrawChargeLine(_shooting, _data, line, 1f);

            if (line != null)
            {
                line.startColor = _data.FireColor;
                line.endColor = _data.FireColor;
            }

            FireRayCast(_shooting, _data);
            fired = true;

            await UniTask.Yield(PlayerLoopTiming.Update);
        }
        finally
        {
            if (line != null)
            {
                line.positionCount = 0;
                line.gameObject.SetActive(false);
            }

            isCharging = false;
            if (fired) nextAttackTime = Time.time + _data.AttackInterval;
        }
    }

    private bool CanContinue(PlayerShooting _shooting, SniperWeaponData _data)
    {
        return _shooting != null
            && _shooting.FirePoint != null
            && _shooting.currentWeaponData == _data;
    }

    private void DrawChargeLine(PlayerShooting _shooting, SniperWeaponData _data, LineRenderer _line, float _t)
    {
        if (_line == null || _shooting == null || _shooting.FirePoint == null) return;

        Vector3 start = _shooting.FirePoint.position;
        Vector3 end = start + (Vector3)(_shooting.ShootDirection() * 200f);

        _line.SetPosition(0, start);
        _line.SetPosition(1, end);

        float width = Mathf.Lerp(_data.LineStartWidth, _data.LineEndWidth, _t);
        _line.startWidth = width;
        _line.endWidth = width;
        _line.startColor = _data.ChargeColor;
        _line.endColor = _data.ChargeColor;
    }

    private void FireRayCast(PlayerShooting _shooting, SniperWeaponData _data)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(
            _shooting.FirePoint.position,
            _shooting.ShootDirection(),
            Mathf.Infinity,
            _shooting.enemyLayer
        );

        for (int i = 0; i < hits.Length; i++)
        {
            Collider2D hitCollider = hits[i].collider;
            if (hitCollider == null) continue;

            if (hitCollider.TryGetComponent(out DamageableEntity target))
                target.TakeDamage(_data.Damage);
        }
    }
}
