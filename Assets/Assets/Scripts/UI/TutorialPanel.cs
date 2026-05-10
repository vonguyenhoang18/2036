using UnityEngine;

public class TutorialPanel : MonoBehaviour
{
    [SerializeField] private GameObject[] _tabs;

    private int _currentTabId = 0;

    private UIManager _uiManager => GameManager.Instance.UIManager;
    private AudioManager _audioManager => GameManager.Instance.AudioManager;

    private void OnEnable()
    {
        _currentTabId = 0;
        ShowTab(_currentTabId);
    }

    public void OnSkipBtn()
    {
        _audioManager.PlaySound(AudioType.s_click);
        _uiManager.SetMenuPanel();
    }

    public void OnNextBtn()
    {
        if (_currentTabId == _tabs.Length - 1)
        {
            return;
        }

        _audioManager.PlaySound(AudioType.s_click);
        _currentTabId++;
        ShowTab(_currentTabId);
    }

    public void OnPreviousBtn()
    {
        if (_currentTabId == 0)
        {
            return;
        }

        _audioManager.PlaySound(AudioType.s_click);
        _currentTabId--;
        ShowTab(_currentTabId);
    }

    private void ShowTab(int tabId)
    {
        for (int i = 0; i < _tabs.Length; i++)
        {
            _tabs[i].SetActive(i == tabId);
        }
    }
}
