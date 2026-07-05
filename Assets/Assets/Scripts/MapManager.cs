using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        CameraManager.Instance.InitDangerZone();
        CharacterManager.Instance.ChangeMaskState(true);

        if (_currentLevel == 1)
        {
            CheckpointManager.Instance.SetCheckpoint(Checkpoint.Level1);
        }
        else if (_currentLevel == 4)
        {
            // Level 4 show dialogue
            GameObject go = UIManager.Instance.ShowPopup(Popup.Dialogue);
            go.GetComponent<PopupDialogue>().SetDialogue(Checkpoint.Level4_Start);
            CheckpointManager.Instance.SetCheckpoint(Checkpoint.Level4_Start);
        }
        else if (_currentLevel == 5)
        {
            // Level 5 show dialogue
            GameObject go = UIManager.Instance.ShowPopup(Popup.Dialogue);
            go.GetComponent<PopupDialogue>().SetDialogue(Checkpoint.Level5_Start);
            CheckpointManager.Instance.SetCheckpoint(Checkpoint.Level5_Start);
        }
    }

    public void InitSafeZoneMap()
    {
        dangerZone.gameObject.SetActive(false);
        safeZone.gameObject.SetActive(true);

        safeZone.InitMap();
        CharacterManager.Instance.Init();
        UIManager.Instance.SetSafeZonePanel();
        CameraManager.Instance.InitSafeZone();
        CharacterManager.Instance.ChangeMaskState(false);
    }

    public void WinLevel()
    {
        _currentLevel++;
        PlayerPrefs.SetInt("CurrentLevel", _currentLevel);
        GameObject loading = UIManager.Instance.ShowPopup(Popup.Loading);
        loading.GetComponent<LoadingPanel>().EndLoading(1f, () => {
            if (_currentLevel == 2)
            {
                UIManager.Instance.HidePopup();
                // Level 2 show tutorial medkit
                UIManager.Instance.ShowPopup(Popup.Tutorial2);
            }
            else if (_currentLevel == 6)
            {
                InventoryManager.Instance.SetMedKit(0);
                SceneManager.LoadScene("CutScene1");
            }
            else if (_currentLevel > 6)
            {
                _currentLevel--;
                PlayerPrefs.SetInt("CurrentLevel", _currentLevel);
                UIManager.Instance.HidePopup();
                InitSafeZoneMap();
            }
            else
            {
                UIManager.Instance.HidePopup();
                InitDangerZoneMap();
            }
        });
    }

    public void LoseLevel()
    {
        InitDangerZoneMap();
    }

    public void SetLevel(int level)
    {
        _currentLevel = level;
        PlayerPrefs.SetInt("CurrentLevel", level);
    }
}
