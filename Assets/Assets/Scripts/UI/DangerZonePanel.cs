using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DangerZonePanel : MonoBehaviour
{
    [SerializeField] private Image healthImg;
    [SerializeField] private TextMeshProUGUI maskStateTxt;
    [SerializeField] private TextMeshProUGUI medkitCountTxt;

    private UIManager _uiManager => GameManager.Instance.UIManager;
    private AudioManager _audioManager => GameManager.Instance.AudioManager;

    public void OnPauseBtn()
    {
        _audioManager.PlaySound(AudioType.s_click);
        _uiManager.SetSettingPanel(true);
    }

    public void UpdateMaskState(bool state)
    {
        maskStateTxt.SetText(state ? "Mask: On" : "Mask: Off");
    }

    public void UpdateMedkitCount(int count)
    {
        medkitCountTxt.SetText($"{count}");
    }

    public void UpdateHealthBar(float percentage)
    {
        healthImg.fillAmount = percentage;
    }
}
