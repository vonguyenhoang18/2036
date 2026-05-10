using UnityEngine;

public class SafeZonePanel : MonoBehaviour
{
    private UIManager _uiManager => GameManager.Instance.UIManager;
    private AudioManager _audioManager => GameManager.Instance.AudioManager;

    public void OnPauseBtn()
    {
        _audioManager.PlaySound(AudioType.s_click);
        _uiManager.SetSettingPanel(true);
    }
}
