using System.Runtime.CompilerServices;
using UnityEngine;

public class PopupTutorial : MonoBehaviour
{
    [SerializeField] private GameObject previousBtn;
    [SerializeField] private GameObject nextBtn;
    [SerializeField] private GameObject skipBtn;
    [SerializeField] private GameObject[] tabs;

    private int _currentTabId = 0;


    private void OnEnable()
    {
        _currentTabId = 0;
        skipBtn.SetActive(tabs.Length > 1);
        ShowTab();
        AudioManager.Instance.PlayMusic(AudioType.m_mainMenu);
    }

    public void OnSkipBtn()
    {
        AudioManager.Instance.PlaySound(AudioType.s_click);
        StartGame();
    }

    public void OnNextBtn()
    {
        if (_currentTabId == tabs.Length - 1)
        {
            StartGame();
            return;
        }

        AudioManager.Instance.PlaySound(AudioType.s_click);
        _currentTabId++;
        ShowTab();
    }

    public void OnPreviousBtn()
    {
        AudioManager.Instance.PlaySound(AudioType.s_click);
        _currentTabId--;
        ShowTab();
    }

    private void ShowTab()
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].SetActive(i == _currentTabId);
        }

        previousBtn.SetActive(_currentTabId > 0);
        nextBtn.SetActive(_currentTabId < tabs.Length);
    }

    private void StartGame()
    {
        GameObject loading = UIManager.Instance.ShowPopup(Popup.Loading);
        loading.GetComponent<LoadingPanel>().EndLoading(1f, () =>
        {
            UIManager.Instance.HidePopup();
            MapManager.Instance.InitDangerZoneMap();
        });
    }
}
