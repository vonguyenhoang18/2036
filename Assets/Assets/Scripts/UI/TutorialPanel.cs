using UnityEngine;

public class TutorialPanel : MonoBehaviour
{
    [SerializeField] private GameObject[] _tabs;

    private int _currentTabId = 0;

    private UIManager _uiManager => GameManager.Instance.UIManager;

    public void OnSkipBtn()
    {
        _uiManager.SetMenuPanel();
    }

    public void OnNextBtn()
    {
        if (_currentTabId == _tabs.Length - 1)
        {
            return;
        }

        _currentTabId++;
        ShowTab(_currentTabId);
    }

    public void OnPreviousBtn()
    {
        if (_currentTabId == 0)
        {
            return;
        }

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
