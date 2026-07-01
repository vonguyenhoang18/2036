public enum ObjectType
{
    Unknown = -1,
    Exit,
    Medkit,
    Obstacle,
    Edge,
    Swamp,
    Smoke,
    NPCA,
}

public enum Direction
{
    Left,
    Right,
}

public enum Panel
{
    Menu,
    DangerZone,
    SafeZone,
    Ending,
    Loading,
}

public enum Popup
{
    Tutorial,
    SettingMain,
    SettingSub,
    Loading,
    Result,
    Tutorial2,
    Dialogue,
}

public enum Dialogue
{
    Level4Start,
    Level4NPCA,
    Level5Start,
    Level5End,
    Level4ExitNoti,
    CutScene1_1,
    CutScene1_2,
}

public static class GameConstant
{
    public const float MAX_HP = 100f;
    public const float PLAYER_SPEED = 5f;
    
    public const int HEALING_AMOUNT = 30;
    public const int DAMAGE_AMOUNT = 10;
    
    public const float DRAIN_INTERVAL = 1f;
    public const float DAMAGE_INTERVAL = 0.3f;
    public const float BREATHING_INTERVAL = 0.5f;

    public const float DRAIN_MASK_ON_AMOUNT = 0.33f;
    public const float DRAIN_MASK_OFF_AMOUNT = 3f;
    public const float DAMAGE_OVER_TIME_AMOUNT = 5f;

    public const int LEVEL_PROLOUGE_COUNT = 5;

    public const int SAFE_ZONE_COLUMNS = 11;
    public const int SAFE_ZONE_ROWS = 11;

    public const int ITEM_GAP = 3;

    public const float MASK_RADIUS_ON = 0.142f;
    public const float MASK_RADIUS_OFF = 0.5f;

    public const float KNOCKBACK_DISTANCE = 2f;
    public const float KNOCKBACK_DURATION = 0.5f;

    public const float DIALOGUE_SCALE = 1.2f;

    public static int AdjustHP(ObjectType itemType)
    {
        switch (itemType)
        {
            case ObjectType.Medkit:
                return HEALING_AMOUNT;
            case ObjectType.Smoke:
            case ObjectType.Swamp:  
                return -DAMAGE_AMOUNT;
            default:
                return 0;
        }
    }
}