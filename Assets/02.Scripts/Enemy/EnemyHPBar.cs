using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyHPBar : MonoBehaviour
{
    [SerializeField] private Transform fill;
    [SerializeField] private Transform delayFill;

    // 딜레이 시작 시간
    [SerializeField] private float delayTime = 0.2f;
    // delay Fill이 따라가는 속도
    [SerializeField] private float followSpeed = 4f;

    private float maxHP;
    private float targetRatio = 1f;
    private float delayTimer = 0f;
    private bool isDelayActive = false;

    public void Init(float _maxHP)
    {
        this.maxHP = _maxHP;

        SetRatio(1f);
        targetRatio = 1f;
        delayTimer = 0f;
        isDelayActive = false;

        gameObject.SetActive(false);
    }

    public void SetHP(float _currentHP)
    {
        float ratio = Mathf.Clamp01(_currentHP / maxHP);

        // 피격 시 즉시 HP바 감소
        SetScaleX(fill, ratio);

        // DelayFill 목표 설정
        targetRatio = ratio;
        delayTimer = delayTime;
        isDelayActive = true;

        gameObject.SetActive(ratio < 1f && ratio > 0f);
    }

    private void Update()
    {
        if (!isDelayActive) return;

        if (delayTimer > 0f)
        {
            delayTimer -= Time.deltaTime;
            return;
        }

        float current = delayFill.localScale.x;

        float next = Mathf.MoveTowards(current, targetRatio, followSpeed * Time.deltaTime);

        SetScaleX(delayFill, next);

        if (Mathf.Abs(next - targetRatio) < 0.001f)
        {
            SetScaleX(delayFill, targetRatio);
            isDelayActive = false;
        }
    }

    private void SetRatio(float _ratio)
    {
        SetScaleX(fill, _ratio);
        SetScaleX(delayFill, _ratio);
    }

    private void SetScaleX(Transform _target, float _x)
    {
        Vector3 scale = _target.localScale;
        scale.x = _x;
        _target.localScale = scale;
    }
}
