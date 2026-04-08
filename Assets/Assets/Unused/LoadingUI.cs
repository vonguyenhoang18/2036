using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] private Image progressImg;

    private void OnEnable()
    {
        progressImg.fillAmount = 0f;
    }

    public void LoadingGame(float timeLoading, Action callBack = null)
    {
        gameObject.SetActive(true);
        progressImg.fillAmount = 0;
        progressImg.DOFillAmount(1f, timeLoading).OnComplete(() =>
        {
            callBack?.Invoke();
        });
        
    }
}
