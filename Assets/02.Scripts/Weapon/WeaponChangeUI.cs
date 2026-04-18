using UnityEngine;

// Test용 UI 버튼으로 무기 변경
public class WeaponChangeUI : MonoBehaviour
{
    [SerializeField] private PlayerShooting playerShooting;

    [SerializeField] private NewWeaponData pistolData;
    [SerializeField] private NewWeaponData rifleData;
    [SerializeField] private NewWeaponData shotgunData;
    [SerializeField] private NewWeaponData sniperData;
    [SerializeField] private NewWeaponData axeData;

    public void EquipPistol() => playerShooting.SetWeapon(pistolData);
    public void EquipRifle() => playerShooting.SetWeapon(rifleData);
    public void EquipShotgun() => playerShooting.SetWeapon(shotgunData);
    public void EquipSniper() => playerShooting.SetWeapon(sniperData);
    public void EquipAxe() => playerShooting.SetWeapon(axeData);
}