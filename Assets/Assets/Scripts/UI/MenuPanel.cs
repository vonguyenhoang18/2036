using UnityEngine;
using TMPro;
using DG.Tweening;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] private CanvasGroup survivalBtn;

    [SerializeField] private CanvasGroup notiPopup;
    [SerializeField] private RectTransform notiBG;
    [SerializeField] private TextMeshProUGUI notiTxt;
    [SerializeField] private TextMeshProUGUI versionTxt;

    private bool _survivalEnabled = false;

    private const string SURVIVAL_KEY = "SURVIVAL_ENABLED";

    private void OnEnable()
    {
        Init();
        versionTxt.text = $"Version {Application.version}";
    }
    
    private void Init()
    {
        HideNotiPopup(true);

        _survivalEnabled = PlayerPrefs.GetInt(SURVIVAL_KEY, 0) == 1;
        survivalBtn.alpha = _survivalEnabled ? 1f : 0.5f;
    }

    public void OnStartBtn()
    {
        AudioManager.Instance.PlaySound(AudioType.s_click);
        GameObject loading = UIManager.Instance.ShowPopup(Popup.Loading);
        loading.GetComponent<LoadingPanel>().EndLoading(1f, () =>
        {
            if (MapManager.Instance.CurrentLevel == 1)
            {
                UIManager.Instance.ShowPopup(Popup.Tutorial);
            }
            else
            {
                MapManager.Instance.InitDangerZoneMap();
            }
        });
    }

    public void OnSurvivalBtn()
    {
        AudioManager.Instance.PlaySound(AudioType.s_click);
        if (_survivalEnabled)
        {
            GameObject loading = UIManager.Instance.ShowPopup(Popup.Loading);
            loading.GetComponent<LoadingPanel>().EndLoading(1f, () =>
            {
                MapManager.Instance.InitDangerZoneMap();
            });
        }
        else
        {
            ShowNotiPopup();
        }
    }

    public void OnSettingBtn()
    {
        AudioManager.Instance.PlaySound(AudioType.s_click);
        UIManager.Instance.ShowPopup(Popup.SettingMain);
    }

    private void ShowNotiPopup()
    {
        notiPopup.DOKill();
        notiBG.DOKill();
        notiPopup.alpha = 0f;
        notiBG.anchoredPosition = new Vector2(notiBG.anchoredPosition.x, -100f);
        notiPopup.gameObject.SetActive(true);
        notiTxt.SetText("You need to finish story first to unlock this mode!");
        DOTween.Sequence()
            .Append(notiPopup.DOFade(1f, 1f))
            .Join(notiBG.DOAnchorPosY(0f, 1f))
            .AppendInterval(1f)
            .OnComplete(() => HideNotiPopup(false));
    }

    private void HideNotiPopup(bool immediately)
    {
        if (immediately)
        {
            notiPopup.alpha = 0f;
            notiPopup.gameObject.SetActive(false);
        }
        else
        {
            notiPopup.DOFade(0f, 1f).OnComplete(() =>
            {
                notiPopup.alpha = 0f;
                notiPopup.gameObject.SetActive(false);
            });
        }
    }
}