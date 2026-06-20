using System;
using TMPro;
using UnityEngine;

public class LoadingPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loadingTxt;

    private string[] strings = new string[] { "Loading", "Loading .", "Loading . .", "Loading . . ." };
    private int _index = 0;
    private float _timer = 0f;
    private float _duration = 0f;
    private float _endDuration = 0f;
    private Action _callback;

    private const float UPDATE_INTERVAL = 0.25f; // Update every 0.25 seconds

    private void OnEnable()
    {
        _index = 0;
        _timer = 0f;
        _duration = 0f;
        _endDuration = 0f;
        _callback = null;
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= UPDATE_INTERVAL)
        {
            _timer = 0f;
            _index = (_index + 1) % strings.Length; // Loop through the strings
            loadingTxt.text = strings[_index]; // Update the loading text
        }

        if (_callback != null)
        {
            _duration += Time.deltaTime;
            if (_duration >= _endDuration)
            {
                UIManager.Instance.HidePopup();
                _callback.Invoke();
            }
        }
    }

    public void EndLoading(float duration, Action callback)
    {
        _endDuration = duration;
        _callback = callback;
    }
}
