using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupResult : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleTxt;
    [SerializeField] private Button nextLevelBtn;
    [SerializeField] private Button retryBtn;

    public void ShowResult(bool isWin)
    {
        AudioManager.Instance.PlaySound(isWin ? AudioType.s_winGame : AudioType.s_loseGame);
        titleTxt.text = isWin ? "You Win!" : "You Lose!";
        titleTxt.color = isWin ? Color.yellow : Color.red;
        nextLevelBtn.gameObject.SetActive(isWin);
        retryBtn.gameObject.SetActive(!isWin);

        CharacterManager.Instance.SetPause(true);
    }

    public void OnNextLevelBtn()
    {
        AudioManager.Instance.PlaySound(AudioType.s_click);
        MapManager.Instance.WinLevel();
    }

    public void OnRetryBtn() {
        AudioManager.Instance.PlaySound(AudioType.s_click);
        MapManager.Instance.LoseLevel();
        GameObject loading = UIManager.Instance.ShowPopup(Popup.Loading);
        loading.GetComponent<LoadingPanel>().EndLoading(1f, () => { });
    }

    public void OnMainMenuBtn()
    {
        AudioManager.Instance.PlaySound(AudioType.s_click);
        GameObject loading = UIManager.Instance.ShowPopup(Popup.Loading);
        loading.GetComponent<LoadingPanel>().EndLoading(1f, () =>
        {
            UIManager.Instance.SetMenuPanel();
        });
    }
}
