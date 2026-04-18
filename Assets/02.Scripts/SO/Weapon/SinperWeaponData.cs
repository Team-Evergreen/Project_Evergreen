using UnityEngine;

[CreateAssetMenu(fileName = "SniperWeaponData", menuName = "Game/Weapon/Sniper")]
public class SniperWeaponData : NewWeaponData
{
    [SerializeField] private float chargeDuration = 1f;
    [SerializeField] private float lineStartWidth = 0.03f;
    [SerializeField] private float lineEndWidth = 0.18f;
    [SerializeField] private Color chargeColor = Color.yellow;
    [SerializeField] private Color fireColor = Color.red;

    public float ChargeDuration => chargeDuration;
    public float LineStartWidth => lineStartWidth;
    public float LineEndWidth => lineEndWidth;
    public Color ChargeColor => chargeColor;
    public Color FireColor => fireColor;
}
