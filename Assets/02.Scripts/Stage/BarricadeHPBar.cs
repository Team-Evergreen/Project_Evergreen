using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarricadeHPBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private TMP_Text hpText;

    private Barricade currentBarricade;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Bind(Barricade barricade)
    {
        Unbind();

        currentBarricade = barricade;

        if (currentBarricade == null)
        {
            gameObject.SetActive(false);
            return;
        }

        currentBarricade.OnHPChanged += UpdateHP;
        currentBarricade.OnDestroyed += Hide;

        gameObject.SetActive(true);
        UpdateHP(currentBarricade.CurrentHP, currentBarricade.MaxHP);
    }

    public void Unbind()
    {
        if (currentBarricade == null)
            return;

        currentBarricade.OnHPChanged -= UpdateHP;
        currentBarricade.OnDestroyed -= Hide;
        currentBarricade = null;
    }

    private void UpdateHP(float current, float max)
    {
        if (fillImage != null)
            fillImage.fillAmount = Mathf.Clamp01(current / max);

        if (hpText != null)
            hpText.text = $"{Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)}";
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        Unbind();
    }
}