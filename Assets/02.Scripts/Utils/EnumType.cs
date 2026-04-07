namespace Utils.EnumType
{
    public enum GameState
    {
        None,
        Title,
        MainMenu,
        InGame,
        Pause,
        GameOver
    }
    public enum PlayerState
    {
        None,
        Idle,
        Walk,
        Run,
        Jump,
        Attack
    }

    public enum EnemyState
    {
        None,
        Idle,
        Walk,
        Run,
        Attack,
        Die
    }

    // 무기 타입
    public enum WeaponType
    {
        // 플레이어용 (Person-held)
        Pistol,
        Rifle,
        Sniper,
        Axe,

        // 트럭용 (Vehicle-mounted)
        Flamethrower,
        Turret
    }

    // 무기 종류
    public enum EquipTaype
    {
        Player,
        Vehicle
    }
}