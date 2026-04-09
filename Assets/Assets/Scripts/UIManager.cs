using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private MenuPanel menuPanel;
    [SerializeField] private TutorialPanel tutorialPanel;
    [SerializeField] private GamePanel gamePanel;
    [SerializeField] private SettingPanel settingPanel;
    [SerializeField] private ResultPanel resultPanel;
    [SerializeField] private LoadingPanel loadingPanel;

    private void Awake()
    {
        //SetLoadingPanel();
        //loadingPanel.UpdateProgress(100f, 3f); // Loading to 100% in 3 secs
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
        if (gamePanel.gameObject.activeInHierarchy)
        {
            gamePanel.gameObject.SetActive(false);
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

    public void SetGamePanel()
    {
        DisableAllUI();
        gamePanel.gameObject.SetActive(true);
        //gamePanel.GenerateLevel();
    }

    public void SetSettingPanel(bool isActive)
    {
        settingPanel.gameObject.SetActive(isActive);
    }

    public void SetResultPanel(bool isWin)
    {
        DisableAllUI();
        resultPanel.gameObject.SetActive(true);
        //resultPanel.SetResult(isWin);
    }

    public void SetLoadingPanel(bool isActive)
    {
        loadingPanel.gameObject.SetActive(isActive);
    }

    public void StartGame()
    {
        SetGamePanel();
    }

    public void RetryLevel()
    {
        SetGamePanel();
    }

    public void NextLevel()
    {
        SetGamePanel();
    }
}
