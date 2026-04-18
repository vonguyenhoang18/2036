public enum ObjectType
{
    Unknown = -1,
    Exit,
    Medkit,
    Obstacle,
    Swamp,
    Smoke,
}

public enum Direction
{
    Left,
    Right,
}

public class LevelData
{
    public int columns;
    public int rows;
    public int exitCount;
    public int medkitCount;
    public int objectCount;
    public int swampCount;
    public int smokeCount;
    public float limitX;
    public float limitY;
}

public static class GameConstant
{
    public const float MAX_HP = 100f;
    public const float PLAYER_SPEED = 5f;
    
    public const int HEAL_AMOUNT = 30;
    public const int DAMAGE_AMOUNT = 10;
    
    public const float DRAIN_INTERVAL = 1f;
    public const float DAMAGE_INTERVAL = 0.3f;
    public const float BREATHING_INTERVAL = 0.5f;

    public const float DRAIN_MASK_ON_AMOUNT = 0.33f;
    public const float DRAIN_MASK_OFF_AMOUNT = 3f;
    public const float DAMAGE_OVER_TIME_AMOUNT = 5f;

    public const float ITEM_GAP = 5f;
    public const int LEVEL_COUNT = 5;
    
    public static readonly LevelData[] LevelDatas =
    {
        new LevelData
        {
            columns = 1,
            rows = 1,
            exitCount = 1,
            medkitCount = 0,
            objectCount = 0,
            swampCount = 0,
            smokeCount = 3,
            limitX = 6f,
            limitY = 6f,
        },
        new LevelData
        {
            columns = 2,
            rows = 1,
            exitCount = 1,
            medkitCount = 2,
            objectCount = 4,
            swampCount = 0,
            smokeCount = 6,
            limitX = 13f,
            limitY = 6f,
        },
        new LevelData
        {
            columns = 3,
            rows = 2,
            exitCount = 1,
            medkitCount = 4,
            objectCount = 10,
            swampCount = 4,
            smokeCount = 10,
            limitX = 20f,
            limitY = 13f,
        },
        new LevelData
        {
            columns = 4,
            rows = 3,
            exitCount = 1,
            medkitCount = 5,
            objectCount = 21,
            swampCount = 10,
            smokeCount = 15,
            limitX = 27f,
            limitY = 20f,
        },
        new LevelData
        {
            columns = 3,
            rows = 4,
            exitCount = 1,
            medkitCount = 4,
            objectCount = 25,
            swampCount = 15,
            smokeCount = 13,
            limitX = 20f,
            limitY = 27f,
        }
    };
    
    public static int AdjustHP(ObjectType itemType)
    {
        switch (itemType)
        {
            case ObjectType.Medkit:
                return HEAL_AMOUNT;
            case ObjectType.Smoke:
            case ObjectType.Swamp:  
                return -DAMAGE_AMOUNT;
            default:
                return 0;
        }
    }
}