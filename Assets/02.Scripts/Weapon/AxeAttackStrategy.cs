using Cysharp.Threading.Tasks;
using UnityEngine;

public class AxeAttackStrategy : IPlayerAttackStrategy
{
    private const float RangeDisplayDuration = 0.12f;
    private readonly Collider2D[] results = new Collider2D[20];
    private bool isShowingRange;

    public void Attack(PlayerWeaponController _weaponController, NewWeaponData _weaponData)
    {
        AxeWeaponData data = _weaponData as AxeWeaponData;
        if (data == null || _weaponController == null) return;

        ExecuteDamage(_weaponController, data);
        ShowAxeRangeAsync(_weaponController, data).Forget();
    }

    private void ExecuteDamage(PlayerWeaponController _weaponController, AxeWeaponData _data)
    {
        Vector2 center = _weaponController.transform.position;
        Vector2 forward = _weaponController.ShootDirection();

        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(_weaponController.enemyLayer);

        int count = Physics2D.OverlapCircle(center, _data.AttackRadius, contactFilter, results);

        for (int i = 0; i < count; i++)
        {
            Collider2D targetCollider = results[i];
            results[i] = null;

            if (targetCollider == null) continue;

            Vector2 toTarget = (Vector2)targetCollider.transform.position - center;
            if (toTarget.sqrMagnitude <= Mathf.Epsilon) continue;

            float angle = Vector2.Angle(forward, toTarget);
            if (angle > _data.AttackAngle * 0.5f) continue;

            DamageableEntity target = targetCollider.GetComponentInParent<DamageableEntity>();
            if (target != null) target.TakeDamage(_data.Damage);
        }
    }

    private async UniTaskVoid ShowAxeRangeAsync(PlayerWeaponController _weaponController, AxeWeaponData _data)
    {
        if (isShowingRange || _weaponController == null || _weaponController.sniperLine == null) return;

        isShowingRange = true;
        LineRenderer line = _weaponController.sniperLine;

        try
        {
            line.gameObject.SetActive(true);

            float elapsed = 0f;
            while (elapsed < RangeDisplayDuration)
            {
                if (_weaponController == null || _weaponController.currentWeaponData != _data) return;

                DrawFan(
                    line,
                    _weaponController.transform.position,
                    _weaponController.ShootDirection(),
                    _data.AttackRadius,
                    _data.AttackAngle,
                    16
                );

                elapsed += Time.deltaTime;
                await UniTask.Yield(PlayerLoopTiming.Update);
            }
        }
        finally
        {
            isShowingRange = false;

            if (_weaponController != null && _weaponController.currentWeaponData == _data && line != null)
            {
                line.positionCount = 0;
                line.gameObject.SetActive(false);
            }
        }
    }

    private void DrawFan(LineRenderer _line, Vector2 _center, Vector2 _forward, float _radius, float _angle, int _segments)
    {
        float halfAngle = _angle * 0.5f;

        _line.positionCount = _segments + 3;
        _line.startWidth = 0.05f;
        _line.endWidth = 0.05f;
        _line.startColor = Color.magenta;
        _line.endColor = Color.magenta;

        _line.SetPosition(0, _center);

        for (int i = 0; i <= _segments; i++)
        {
            float t = i / (float)_segments;
            float currentAngle = Mathf.Lerp(-halfAngle, halfAngle, t);
            Vector2 dir = RotateVector(_forward.normalized, currentAngle);
            Vector2 point = _center + dir * _radius;

            _line.SetPosition(i + 1, point);
        }

        _line.SetPosition(_segments + 2, _center);
    }

    private Vector2 RotateVector(Vector2 _direction, float _angle)
    {
        float rad = _angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        return new Vector2(
            _direction.x * cos - _direction.y * sin,
            _direction.x * sin + _direction.y * cos
        ).normalized;
    }
}
