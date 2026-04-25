using UnityEngine;

// Test용 UI 버튼으로 무기 변경
public class WeaponChangeUI : MonoBehaviour
{
    [SerializeField] private PlayerWeaponController PlayerWeaponController;

    [SerializeField] private NewWeaponData pistolData;
    [SerializeField] private NewWeaponData rifleData;
    [SerializeField] private NewWeaponData shotgunData;
    [SerializeField] private NewWeaponData sniperData;
    [SerializeField] private NewWeaponData axeData;

    public void EquipPistol() => PlayerWeaponController.SetWeapon(pistolData);
    public void EquipRifle() => PlayerWeaponController.SetWeapon(rifleData);
    public void EquipShotgun() => PlayerWeaponController.SetWeapon(shotgunData);
    public void EquipSniper() => PlayerWeaponController.SetWeapon(sniperData);
    public void EquipAxe() => PlayerWeaponController.SetWeapon(axeData);
}