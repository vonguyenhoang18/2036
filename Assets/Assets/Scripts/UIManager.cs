using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private MenuPanel menuPanel;
    [SerializeField] private TutorialPanel tutorialPanel;
    [SerializeField] private DangerZonePanel dangerZonePanel;
    [SerializeField] private SafeZonePanel safeZonePanel;
    [SerializeField] private SettingPanel settingPanel;
    [SerializeField] private ResultPanel resultPanel;
    [SerializeField] private LoadingPanel loadingPanel;

    public MenuPanel MenuPanel => menuPanel;
    public TutorialPanel TutorialPanel => tutorialPanel;
    public DangerZonePanel DangerZonePanel => dangerZonePanel;
    public SafeZonePanel SafeZonePanel => safeZonePanel;
    public SettingPanel SettingPanel => settingPanel;
    public ResultPanel ResultPanel => resultPanel;
    public LoadingPanel LoadingPanel => loadingPanel;

    private void Start()
    {
        SetMenuPanel();
    }

    private void DisableAllUI()
    {
        if (menuPanel.gameObject.activeInHierarchy)
        {
            menuPanel.gameObject.SetActive(false);
        }
        if (tutorialPanel.gameObject.activeInHierarchy)
        {
            tutorialPanel.gameObject.SetActive(false);
        }
        if (dangerZonePanel.gameObject.activeInHierarchy)
        {
            dangerZonePanel.gameObject.SetActive(false);
        }
        if (safeZonePanel.gameObject.activeInHierarchy)
        {
            safeZonePanel.gameObject.SetActive(false);
        }
        if (settingPanel.gameObject.activeInHierarchy)
        {
            settingPanel.gameObject.SetActive(false);
        }
        if (resultPanel.gameObject.activeInHierarchy)
        {
            resultPanel.gameObject.SetActive(false);
        }
        if (loadingPanel.gameObject.activeInHierarchy)
        {
            loadingPanel.gameObject.SetActive(false);
        }
    }

    public void SetMenuPanel()
    {
        DisableAllUI();
        menuPanel.gameObject.SetActive(true);
    }

    public void SetTutorialPanel()
    {
        DisableAllUI();
        tutorialPanel.gameObject.SetActive(true);
    }

    public void SetDangerZonePanel()
    {
        DisableAllUI();
        dangerZonePanel.gameObject.SetActive(true);
    }

    public void SetSafeZonePanel()
    {
        DisableAllUI();
        safeZonePanel.gameObject.SetActive(true);
    }

    public void SetSettingPanel(bool isActive)
    {
        // Popup
        settingPanel.gameObject.SetActive(isActive);
    }

    public void SetResultPanel(bool isWin)
    {
        DisableAllUI();
        resultPanel.gameObject.SetActive(true);
    }

    public void SetLoadingPanel(bool isActive)
    {
        // Popup
        loadingPanel.gameObject.SetActive(isActive);
    }
}
