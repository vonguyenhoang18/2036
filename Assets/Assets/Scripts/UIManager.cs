using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private MenuPanel menuPanel;
    [SerializeField] private GamePanel gamePanel;
    [SerializeField] private SettingPanel settingPanel;
    [SerializeField] private ResultPanel resultPanel;
    [SerializeField] private LoadingPanel loadingPanel;

    private void Awake()
    {
        SetLoadingPanel();
        //loadingPanel.UpdateProgress(100f, 3f); // Loading to 100% in 3 secs
    }

    private void DisableAllUI()
    {
        if (menuPanel.gameObject.activeInHierarchy)
        {
            menuPanel.gameObject.SetActive(false);
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

    private void SetGamePanel()
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

    private void SetLoadingPanel()
    {
        DisableAllUI();
        loadingPanel.gameObject.SetActive(true);
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
