using TMPro;
using UnityEngine;

public class MapItem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer debugSize;
    [SerializeField] private TextMeshProUGUI debugText;

    private bool _isDebug = true;

    public void SetDebug(int col, int row, int id)
    {
        debugSize.gameObject.SetActive(_isDebug);
        debugText.gameObject.SetActive(_isDebug);
        debugText.SetText($"({col}, {row}) {id}");
    }

    public void SetColor(Color color)
    {
        debugSize.gameObject.SetActive(_isDebug);
        debugSize.color = color;
    }
}
