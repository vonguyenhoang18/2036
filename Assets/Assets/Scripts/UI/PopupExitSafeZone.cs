using System.Runtime.CompilerServices;
using UnityEngine;

public class PopupExitSafeZone : MonoBehaviour
{
    public void OnYesBtn()
    {
        AudioManager.Instance.PlaySound(AudioType.s_click);
        UIManager.Instance.HidePopup();
        GameObject loading = UIManager.Instance.ShowPopup(Popup.Loading);
        loading.GetComponent<LoadingPanel>().EndLoading(1f, () => {
            UIManager.Instance.HidePopup();
            MapManager.Instance.InitDangerZoneMap();
            CharacterManager.Instance.SetPause(false);
        });
    }

    public void OnNoBtn()
    {
        AudioManager.Instance.PlaySound(AudioType.s_click);
        UIManager.Instance.HidePopup();
        CharacterManager.Instance.SetPause(false);
    }
}
