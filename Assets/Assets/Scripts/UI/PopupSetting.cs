using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PopupSetting : MonoBehaviour
{
    [SerializeField] private Image musicImg;
    [SerializeField] private Image soundImg;

    [SerializeField] private Sprite musicOn;
    [SerializeField] private Sprite musicOff;
    [SerializeField] private Sprite soundOn;
    [SerializeField] private Sprite soundOff;

    [SerializeField] private bool _isSettingMain;

    private bool _musicEnable = true;
    private bool _soundEnable = true;

    private void OnEnable()
    {
        UpdateMusicIcon();
        UpdateSoundIcon();
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            OnCloseBtn();
        }
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
        if (!_isSettingMain)
        {
            CharacterManager.Instance.SetPause(false);
        }
    }

    private void UpdateMusicIcon()
    {
        _musicEnable = AudioManager.Instance.MusicEnabled;
        musicImg.sprite = _musicEnable ? musicOn : musicOff;
    }

    private void UpdateSoundIcon() {
        _soundEnable = AudioManager.Instance.SoundEnabled;
        soundImg.sprite = _soundEnable ? soundOn : soundOff;
    }

    public void OnBackBtn()
    {
        AudioManager.Instance.PlaySound(AudioType.s_click);
        GameObject loading = UIManager.Instance.ShowPopup(Popup.Loading);
        loading.GetComponent<LoadingPanel>().EndLoading(1f, () =>
        {
            UIManager.Instance.HidePopup();
            UIManager.Instance.SetMenuPanel();
        });
    }
}
