using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    [SerializeField] private Image _musicImg;
    [SerializeField] private Image _soundImg;

    [SerializeField] private Sprite _musicOn;
    [SerializeField] private Sprite _musicOff;
    [SerializeField] private Sprite _soundOn;
    [SerializeField] private Sprite _soundOff;

    private bool _musicEnable = true;
    private bool _soundEnable = true;

    private void OnEnable()
    {
        UpdateMusicIcon();
        UpdateSoundIcon();
    }

    public void OnMusicBtn()
    {
        AudioManager.Instance.PlaySound(AudioType.s_click);
        _musicEnable = !_musicEnable;
        AudioManager.Instance.ToggleMusic(_musicEnable);
        UpdateMusicIcon();
    }

    public void OnSoundBtn()
    {
        AudioManager.Instance.PlaySound(AudioType.s_click);
        _soundEnable = !_soundEnable;
        AudioManager.Instance.ToggleSound(_soundEnable);
        UpdateSoundIcon();
    }

    public void OnCloseBtn()
    {
        AudioManager.Instance.PlaySound(AudioType.s_click);
        UIManager.Instance.HidePopup();
    }

    private void UpdateMusicIcon()
    {
        _musicEnable = AudioManager.Instance.MusicEnabled;
        _musicImg.sprite = _musicEnable ? _musicOn : _musicOff;
    }

    private void UpdateSoundIcon() {
        _soundEnable = AudioManager.Instance.SoundEnabled;
        _soundImg.sprite = _soundEnable ? _soundOn : _soundOff;
    }

    public void OnBackBtn()
    {
        AudioManager.Instance.PlaySound(AudioType.s_click);
        //DevHoang: Call MapManager to destroy map
        UIManager.Instance.HidePopup();
    }
}
