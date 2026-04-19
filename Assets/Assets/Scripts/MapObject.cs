using TMPro;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    [SerializeField] private SpriteRenderer debugSize;
    [SerializeField] private TextMeshProUGUI debugText;

    [SerializeField] private Transform content;
    [SerializeField] private GameObject exitPrefab;
    [SerializeField] private GameObject medkitPrefab;
    [SerializeField] private GameObject[] objectsPrefab;
    [SerializeField] private GameObject edgePrefab;

    private bool _isDebug = false;
    private ObjectType _itemType = ObjectType.Unknown;

    public void SetDebug(int col, int row, int id)
    {
        debugSize.gameObject.SetActive(_isDebug);
        debugText.gameObject.SetActive(_isDebug);
        debugText.SetText($"({col}, {row}) {id}");
    }

    public void SetColor(Color color)
    {
        debugSize.gameObject.SetActive(_isDebug);
        debugSize.color = new Color(color.r, color.g, color.b, 1f);
    }

    public void InitItem(ObjectType itemType)
    {
        _itemType = itemType;
        switch (itemType)
        {
            case ObjectType.Exit:
                Instantiate(exitPrefab, content);
                break;
            case ObjectType.Medkit:
                Instantiate(medkitPrefab, content);
                break;
            case ObjectType.Obstacle:
                var randomIndex = Random.Range(0, objectsPrefab.Length);
                Instantiate(objectsPrefab[randomIndex], content);
                break;
            case ObjectType.Edge:
                Instantiate(edgePrefab, content);
                break;
        }
    }
}
