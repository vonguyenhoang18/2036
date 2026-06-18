using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance
    {
        get { return instance; }
    }

    private static MapManager instance = null;

    [SerializeField] private DangerZone dangerZone;
    [SerializeField] private SafeZone safeZone;

    private int _currentLevel = 1;
    public int CurrentLevel => _currentLevel;

    private void Awake()
    {
        if (instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void InitDangerZoneMap()
    {
        dangerZone.gameObject.SetActive(true);
        safeZone.gameObject.SetActive(false);

        UIManager.Instance.ShowPopup(Popup.Loading);
        //_uiManager.LoadingPanel.EndLoading(2f, () =>
        //{
        //    dangerZone.InitMap();
        //    _characterManager.Init();
        //    _uiManager.SetDangerZonePanel();
        //});
    }

    public void InitSafeZoneMap()
    {
        dangerZone.gameObject.SetActive(false);
        safeZone.gameObject.SetActive(true);

        UIManager.Instance.ShowPopup(Popup.Loading);
        //_uiManager.LoadingPanel.EndLoading(2f, () =>
        //{
        //    safeZone.InitMap();
        //    _characterManager.Init();
        //    _uiManager.SetSafeZonePanel();
        //});
    }

    public void WinLevel()
    {
        _currentLevel++;
        if (_currentLevel > GameConstant.LEVEL_PROLOUGE_COUNT)
        {
            Debug.Log("You will go to SafeZone");
            InitSafeZoneMap();
        }
        else
        {
            Debug.Log($"Congratulations! You've completed level prolouge {_currentLevel}");
            InitDangerZoneMap();
        }
    }

    public void LoseLevel()
    {

    }
}
