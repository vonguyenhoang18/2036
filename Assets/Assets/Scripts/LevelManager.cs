using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private MapManager _mapManager => GameManager.Instance.MapManager;

    public int CurrentLevel {get; private set; } = 1;

    public void StartLevel()
    {

    }

    public void WinLevel()
    {
        CurrentLevel++;
        _mapManager.InitDangerZoneMap();
    }

    public void LoseLevel()
    {

    }
}
