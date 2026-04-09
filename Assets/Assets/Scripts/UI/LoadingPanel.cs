using TMPro;
using UnityEngine;

public class LoadingPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _loadingTxt;

    private string[] strings = new string[] { "Loading", "Loading .", "Loading . .", "Loading . . ." };
    private int _index = 0;
    private float _timer = 0f;

    private const float UPDATE_INTERVAL = 0.25f; // Update every 0.25 seconds

    private void OnEnable()
    {
        _index = 0;
        _timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= UPDATE_INTERVAL)
        {
            _timer = 0f;
            _index = (_index + 1) % strings.Length; // Loop through the strings
            _loadingTxt.text = strings[_index]; // Update the loading text
        }
    }
}
