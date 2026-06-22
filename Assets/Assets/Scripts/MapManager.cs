using System.Runtime.CompilerServices;
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

    public bool FinishObjective = true;

    private void Awake()
    {
        if (instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        Init();
    }

    private void Init()
    {
        _currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
    }

    public void InitDangerZoneMap()
    {
        dangerZone.gameObject.SetActive(true);
        safeZone.gameObject.SetActive(false);

        dangerZone.InitMap();
        CharacterManager.Instance.Init();
        UIManager.Instance.SetDangerZonePanel();

        if (_currentLevel == 4)
        {
            // Level 4 show dialogue
            GameObject go = UIManager.Instance.ShowPopup(Popup.Dialogue);
            go.GetComponent<PopupDialogue>().SetDialogue(Dialogue.Level4Start);
        }
        if (_currentLevel == 5)
        {
            // Level 5 show dialogue
            GameObject go = UIManager.Instance.ShowPopup(Popup.Dialogue);
            go.GetComponent<PopupDialogue>().SetDialogue(Dialogue.Level5Start);
        }
    }

    public void InitSafeZoneMap()
    {
        dangerZone.gameObject.SetActive(false);
        safeZone.gameObject.SetActive(true);

        safeZone.InitMap();
        CharacterManager.Instance.Init();
        UIManager.Instance.SetSafeZonePanel();
    }

    public void WinLevel()
    {
        Debug.Log($"Win");
        _currentLevel++;
        PlayerPrefs.SetInt("CurrentLevel", _currentLevel);
        GameObject loading = UIManager.Instance.ShowPopup(Popup.Loading);
        loading.GetComponent<LoadingPanel>().EndLoading(1f, () => {
            if (_currentLevel > GameConstant.LEVEL_PROLOUGE_COUNT)
            {
                _currentLevel = 1;
                PlayerPrefs.SetInt("CurrentLevel", _currentLevel);
                InventoryManager.Instance.SetMedKit(0);
                UIManager.Instance.SetEndingPanel();
            }
            else if (_currentLevel == 2)
            {
                // Level 2 show tutorial medkit
                UIManager.Instance.ShowPopup(Popup.Tutorial2);
            }
            else
            {
                InitDangerZoneMap();
            }
        });
    }

    public void LoseLevel()
    {
        Debug.Log($"Lose");
        InitDangerZoneMap();
    }
}
