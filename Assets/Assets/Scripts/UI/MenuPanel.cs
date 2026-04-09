using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    private UIManager _uiManager => GameManager.Instance.UIManager;

    public void OnStartBtn()
    {
        _uiManager.SetGamePanel();
    }

    public void OnTutorialBtn()
    {
        _uiManager.SetTutorialPanel();
    }

    public void OnQuitBtn()
    {
        Application.Quit();
    }

    public void OnSettingBtn()
    {
        _uiManager.SetSettingPanel(true);
    }
}
