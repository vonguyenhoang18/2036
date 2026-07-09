using System;
using UnityEngine;
using DG.Tweening;

public class PanelAnim : MonoBehaviour
{
    [SerializeField] private CanvasGroup panelCanvas;
    [SerializeField] private Transform panelContent;

    public void ShowPanel()
    {
        panelCanvas.alpha = 0f;
        panelCanvas.DOFade(1f, 0.3f);
        if (panelContent != null)
        {
            panelContent.localScale = Vector3.zero;
            DOTween.Sequence()
                .Append(panelContent.DOScale(Vector3.one * 1.2f, 0.2f).SetEase(Ease.OutQuad))
                .Append(panelContent.DOScale(Vector3.one, 0.15f).SetEase(Ease.InOutQuad));
        }
    }

    public void HidePanel()
    {
        panelCanvas.DOFade(0f, 0.25f)
            .OnComplete(() => panelCanvas.gameObject.SetActive(false));
        panelContent?.DOScale(Vector3.one * 0.8f, 0.25f);
    }
}
