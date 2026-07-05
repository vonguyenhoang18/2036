using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance
    {
        get { return instance; }
    }

    private static UIManager instance = null;

    [SerializeField] private MenuPanel menuPanel;
    [SerializeField] private DangerZonePanel dangerZonePanel;
    [SerializeField] private SafeZonePanel safeZonePanel;
    [SerializeField] private EndingPanel endingPanel;

    [SerializeField] private Transform popupContent;
    [SerializeField] private GameObject[] popups;

    public MenuPanel MenuPanel => menuPanel;
    public DangerZonePanel DangerZonePanel => dangerZonePanel;
    public SafeZonePanel SafeZonePanel => safeZonePanel;
    public EndingPanel EndingPanel => endingPanel;

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

    private void Start()
    {
        if (menuPanel != null)
        {
            SetMenuPanel();
        }
    }

    private void DisableAllUI()
    {
        if (menuPanel.gameObject.activeInHierarchy)
        {
            menuPanel.gameObject.SetActive(false);
        }
        if (dangerZonePanel.gameObject.activeInHierarchy)
        {
            dangerZonePanel.gameObject.SetActive(false);
        }
        if (safeZonePanel.gameObject.activeInHierarchy)
        {
            safeZonePanel.gameObject.SetActive(false);
        }
        if (endingPanel.gameObject.activeInHierarchy)
        {
            endingPanel.gameObject.SetActive(false);
        }
    }

    public void SetMenuPanel()
    {
        DisableAllUI();
        menuPanel.gameObject.SetActive(true);

        AudioManager.Instance.PlayMusic(AudioType.m_mainMenu);
    }

    public void SetDangerZonePanel()
    {
        DisableAllUI();
        dangerZonePanel.gameObject.SetActive(true);

        AudioManager.Instance.PlayMusic(AudioType.m_gameplay);
    }

    public void SetSafeZonePanel()
    {
        DisableAllUI();
        safeZonePanel.gameObject.SetActive(true);
    }

    public void SetEndingPanel()
    {
        DisableAllUI();
        endingPanel.gameObject.SetActive(true);

        AudioManager.Instance.PlayMusic(AudioType.m_ending);
    }

    public GameObject ShowPopup(Popup popup)
    {
        ClearPopup();
        GameObject go = Instantiate(popups[(int)popup], popupContent);
        return go;
    }

    public void HidePopup()
    {
        ClearPopup();
    }

    private void ClearPopup()
    {
        foreach (Transform child in popupContent)
        {
            Destroy(child.gameObject);
        }
    }
}
