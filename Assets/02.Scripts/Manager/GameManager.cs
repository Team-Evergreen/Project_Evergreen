using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }
    public PlayerController playerController;
    public PlayerWeaponController playerWeaponController;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        Init();
    }

    private void Init()
    {
        Application.targetFrameRate = 65;
        Screen.SetResolution(1080, 1920, true);
    }

    public void PlayerEnabled(bool _enabled)
    {
        playerController.SetInputEnabled(_enabled);
        playerWeaponController.SetWeaponEnabled();
    }
}