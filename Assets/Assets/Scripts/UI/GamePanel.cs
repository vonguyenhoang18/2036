using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    [SerializeField] private Image healthImg;
    [SerializeField] private TextMeshProUGUI maskStateTxt;
    [SerializeField] private TextMeshProUGUI medkitCountTxt;

    private UIManager _uiManager => GameManager.Instance.UIManager;

    public void OnPauseBtn()
    {
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
