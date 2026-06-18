using DG.Tweening;
using UnityEngine;

public class EndingPanel : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    private bool _isAnimDone = false;

    private void OnEnable()
    {
        _isAnimDone = false;
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1f, 0.5f).SetDelay(3f).OnComplete(() => _isAnimDone = true);
    }

    public void OnBackBtn()
    {
        if (!_isAnimDone) return;

        AudioManager.Instance.PlaySound(AudioType.s_click);
        UIManager.Instance.SetMenuPanel();
    }
}
