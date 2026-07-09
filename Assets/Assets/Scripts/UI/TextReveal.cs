using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextReveal : MonoBehaviour
{
    private float startDelay = 0.5f;
    private float letterDelay = 0.03f;

    private TextMeshProUGUI _text;
    private Coroutine _revealCoroutine;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        _revealCoroutine = StartCoroutine(RevealRoutine());
    }

    private void OnDisable()
    {
        if (_revealCoroutine != null)
            StopCoroutine(_revealCoroutine);
        _revealCoroutine = null;
    }

    private IEnumerator RevealRoutine()
    {
        _text.ForceMeshUpdate();

        int totalChars = _text.textInfo.characterCount;
        _text.maxVisibleCharacters = 0;

        if (startDelay > 0f)
            yield return new WaitForSeconds(startDelay);

        WaitForSeconds wait = new WaitForSeconds(letterDelay);
        for (int visible = 1; visible <= totalChars; visible++)
        {
            _text.maxVisibleCharacters = visible;
            yield return wait;
        }

        _revealCoroutine = null;
    }
}
