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
    [SerializeField] private GameObject medkit;

    private Tween _maskTween;

    private void OnEnable()
    {
        // Medkit is unlocked after level 1, so hide it in level 1
        medkit.SetActive(MapManager.Instance.CurrentLevel != 1);
    }

    public void OnPauseBtn()
    {
        AudioManager.Instance.PlaySound(AudioType.s_click);
        UIManager.Instance.ShowPopup(Popup.SettingSub);
        CharacterManager.Instance.SetPause(true);
    }

    public void UpdateMaskState(bool state, bool immediately)
    {
        maskStateTxt.SetText(state ? "Mask: On" : "Mask: Off");

        float targetRadius = state ? GameConstant.MASK_RADIUS_ON : GameConstant.MASK_RADIUS_OFF;
        _maskTween?.Kill();
        if (immediately)
        {
            maskVision.SetRadius(targetRadius);
            return;
        }
        _maskTween = DOTween.To(
            () => maskVision.innerRadius,
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
