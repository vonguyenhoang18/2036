using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SafeZone : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform itemMap;

    // ─── Editor Button ───────────────────────────────────────────────
#if UNITY_EDITOR
    [CustomEditor(typeof(SafeZone))]
    public class SafeZoneEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            GUILayout.Space(8);
            if (GUILayout.Button("Init Map (Edge Only)", GUILayout.Height(30)))
            {
                ((SafeZone)target).InitMap();
            }
        }
    }
#endif

    // ─── Runtime / Context-Menu entry ────────────────────────────────
    [ContextMenu("Init Map")]
    public void InitMap()
    {
        // Clear previous children
        for (int i = itemMap.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(itemMap.GetChild(i).gameObject);
        }

        int columns = GameConstant.SAFE_ZONE_COLUMNS;
        int rows = GameConstant.SAFE_ZONE_ROWS;

        float totalWidth = (columns - 1) * GameConstant.ITEM_GAP;
        float totalHeight = (rows - 1) * GameConstant.ITEM_GAP;
        float xOffset = totalWidth / 2f;
        float yOffset = totalHeight / 2f;

        for (int j = 0; j < rows; j++)
        {
            for (int i = 0; i < columns; i++)
            {
                // Skip non-edge cells
                bool isEdge = (i == 0 || i == columns - 1 ||
                               j == 0 || j == rows - 1);
                if (!isEdge) continue;

                var item = Instantiate(itemPrefab, itemMap);
                item.name = $"Edge_{i}_{j}";

                float xPos = i * GameConstant.ITEM_GAP - xOffset;
                float yPos = j * GameConstant.ITEM_GAP - yOffset;
                item.transform.localPosition = new Vector2(xPos, yPos);

                // Optional: tint if your prefab has a MapObject component
                if (item.TryGetComponent<MapObject>(out var mapItem))
                {
                    mapItem.SetColor(Color.red);
                    mapItem.InitItem(ObjectType.Edge);
                }
            }
        }
    }
}
