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

    private AudioManager _audioManager => GameManager.Instance.AudioManager;

    private void OnEnable()
    {
        UpdateMusicIcon();
        UpdateSoundIcon();
    }

    public void OnMusicBtn()
    {
        _musicEnable = !_musicEnable;
        _audioManager.ToggleMusic(_musicEnable);
        UpdateMusicIcon();
    }

    public void OnSoundBtn()
    {
        _soundEnable = !_soundEnable;
        _audioManager.ToggleSound(_soundEnable);
        UpdateSoundIcon();
    }

    public void OnCloseBtn()
    {
        GameManager.Instance.UIManager.SetSettingPanel(false);
    }

    private void UpdateMusicIcon()
    {
        _musicEnable = _audioManager.MusicEnabled;
        _musicImg.sprite = _musicEnable ? _musicOn : _musicOff;
    }

    private void UpdateSoundIcon() {
        _soundEnable = _audioManager.SoundEnabled;
        _soundImg.sprite = _soundEnable ? _soundOn : _soundOff;
    }
}
