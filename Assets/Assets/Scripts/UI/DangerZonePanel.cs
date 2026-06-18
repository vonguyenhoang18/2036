using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DangerZonePanel : MonoBehaviour
{
    [SerializeField] private Image healthImg;
    [SerializeField] private TextMeshProUGUI maskStateTxt;
    [SerializeField] private TextMeshProUGUI medkitCountTxt;
    [SerializeField] private CircleVisionController maskVision;

    private Tween _maskTween;

    public void OnPauseBtn()
    {
        AudioManager.Instance.PlaySound(AudioType.s_click);
        UIManager.Instance.ShowPopup(Popup.SettingSub);
    }

    public void UpdateMaskState(bool state)
    {
        maskStateTxt.SetText(state ? "Mask: On" : "Mask: Off");

        float targetRadius = state ? GameConstant.MASK_RADIUS_ON : GameConstant.MASK_RADIUS_OFF;
        _maskTween?.Kill();
        _maskTween = DOTween.To(
            () => maskVision.outerRadius,
            x => maskVision.SetRadius(x),
            targetRadius,
            0.5f
        ).SetEase(Ease.InOutSine)
         .OnComplete(() => maskVision.SetRadius(targetRadius));
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
