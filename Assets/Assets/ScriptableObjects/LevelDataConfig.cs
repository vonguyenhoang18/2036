using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "LevelDataConfig", menuName = "Scriptable Objects/LevelDataConfig")]
public class LevelDataConfig : ScriptableObject
{
    public List<LevelData> allLevels = new List<LevelData>();

    public LevelData GetLevelData(int levelId)
    {
        return allLevels.Find(level => level.levelId == levelId);
    }
}

[System.Serializable]
public class LevelData
{
    public int levelId;
    public int columns;
    public int rows;
    public int medkitCount;
    public int obstacleCount;
    public int swampCount;
    public int smokeCount;
}
