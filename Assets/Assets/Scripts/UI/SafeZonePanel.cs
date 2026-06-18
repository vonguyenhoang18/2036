using UnityEngine;

public class SafeZonePanel : MonoBehaviour
{
    public void OnPauseBtn()
    {
        AudioManager.Instance.PlaySound(AudioType.s_click);
        UIManager.Instance.ShowPopup(Popup.SettingSub);
    }
}
