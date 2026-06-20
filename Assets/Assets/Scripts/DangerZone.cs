using System.Collections.Generic;
using UnityEngine;

public class DangerZone : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform itemMap;
    [SerializeField] private LevelDataConfig levelDataConfig;
    
    private MapObject[,] _map;
    private List<(int x, int y)> _objectCoordinates = new();
    private LevelData _currentLevelData;

    public void InitMap()
    {
        _currentLevelData = levelDataConfig.GetLevelData(MapManager.Instance.CurrentLevel);
        ClearMap();
        GenerateGrid();
    }

    private void ClearMap()
    {
        foreach (Transform child in itemMap)
        {
            Destroy(child.gameObject);
        }
        _objectCoordinates.Clear();
    }

    private const int EDGE_RINGS = 4;

    private void GenerateGrid()
    {
        int columns = _currentLevelData.columns;
        int rows = _currentLevelData.rows;
        int totalColumns = columns + 2 * EDGE_RINGS;
        int totalRows = rows + 2 * EDGE_RINGS;

        _map = new MapObject[totalColumns, totalRows];
        float totalWidth = (totalColumns - 1) * GameConstant.ITEM_GAP;
        float totalHeight = (totalRows - 1) * GameConstant.ITEM_GAP;
        float xOffset = totalWidth / 2f;
        float yOffset = totalHeight / 2f;
        int id = 1;

        for (int j = 0; j < totalRows; j++)
        {
            for (int i = 0; i < totalColumns; i++)
            {
                var item = Instantiate(itemPrefab, itemMap);
                float xPos = i * GameConstant.ITEM_GAP - xOffset;
                float yPos = j * GameConstant.ITEM_GAP - yOffset;
                item.transform.localPosition = new Vector2(xPos, yPos);

                MapObject mapItem = item.GetComponent<MapObject>();
                mapItem.SetDebug(i, j, id++);
                _map[i, j] = mapItem;

                // 3 outer rings of edge cells
                if (i < EDGE_RINGS || i >= totalColumns - EDGE_RINGS ||
                    j < EDGE_RINGS || j >= totalRows - EDGE_RINGS)
                {
                    mapItem.SetColor(Color.red);
                    mapItem.InitItem(ObjectType.Edge);
                }
                // The center of the map is yellow, with the size is 3x3
                else if (IsCenterCoordinates(i, j, totalColumns, totalRows))
                {
                    mapItem.SetColor(Color.yellow);
                }
                else
                {
                    _objectCoordinates.Add((i, j));
                }
            }
        }

        Debug.Log($"DevHoang CurrentLevel {MapManager.Instance.CurrentLevel}");
        InitExit();
        if (MapManager.Instance.CurrentLevel == 4)
        {
            InitNPCA();
        }
        InitMedKits();
        InitObstacles();
        InitSwamps();
        InitSmokes();
    }

    private void InitObjectAtTargetDistance(ObjectType objectType, Color color, float distanceFraction = 0.8f)
    {
        int totalColumns = _currentLevelData.columns + 2 * EDGE_RINGS;
        int totalRows = _currentLevelData.rows + 2 * EDGE_RINGS;
        float centerX = totalColumns / 2f;
        float centerY = totalRows / 2f;

        float maxDist = Vector2.Distance(
            new Vector2(centerX, centerY),
            new Vector2(EDGE_RINGS, EDGE_RINGS)
        );
        float targetDist = maxDist * distanceFraction;
        float tolerance = GameConstant.ITEM_GAP;

        List<int> candidates = new();
        int fallbackIndex = -1;
        float bestDelta = float.MaxValue;

        for (int i = 0; i < _objectCoordinates.Count; i++)
        {
            var (x, y) = _objectCoordinates[i];
            float dist = Vector2.Distance(new Vector2(x, y), new Vector2(centerX, centerY));
            float delta = Mathf.Abs(dist - targetDist);

            if (delta <= tolerance)
                candidates.Add(i);

            if (delta < bestDelta)
            {
                bestDelta = delta;
                fallbackIndex = i;
            }
        }

        if (candidates.Count == 0 && fallbackIndex >= 0)
            candidates.Add(fallbackIndex);

        if (candidates.Count == 0) return;

        int chosenIndex = candidates[Random.Range(0, candidates.Count)];
        var (ex, ey) = _objectCoordinates[chosenIndex];
        _map[ex, ey].SetColor(color);
        _map[ex, ey].InitItem(objectType);
        _objectCoordinates.RemoveAt(chosenIndex);
    }

    private void InitExit()
    {
        InitObjectAtTargetDistance(ObjectType.Exit, Color.blue, 0.8f);
    }

    private void InitNPCA()
    {
        InitObjectAtTargetDistance(ObjectType.NPCA, Color.blue, 0.8f);
    }

    private void InitMedKits()
    {
        int medkitCount = _currentLevelData.medkitCount;

        // Randomly select MEDKIT_COUNT coordinates and set to blue
        for (int k = 0; k < medkitCount && _objectCoordinates.Count > 0; k++)
        {
            int randomIndex = Random.Range(0, _objectCoordinates.Count);
            var (x, y) = _objectCoordinates[randomIndex];
            _map[x, y].SetColor(Color.cyan);
            _map[x, y].InitItem(ObjectType.Medkit);
            _objectCoordinates.RemoveAt(randomIndex);
        }
    }

    private void InitSwamps()
    {
        int swampCount = _currentLevelData.swampCount;

        // Randomly select ITEM_COUNT coordinates and set to gray
        for (int k = 0; k < swampCount && _objectCoordinates.Count > 0; k++)
        {
            int randomIndex = Random.Range(0, _objectCoordinates.Count);
            var (x, y) = _objectCoordinates[randomIndex];
            _map[x, y].SetColor(Color.gray);
            _map[x, y].InitItem(ObjectType.Swamp);
            _objectCoordinates.RemoveAt(randomIndex);
        }
    }

    private void InitSmokes()
    {
        int smokeCount = _currentLevelData.smokeCount;

        // Randomly select ITEM_COUNT coordinates and set to gray
        for (int k = 0; k < smokeCount && _objectCoordinates.Count > 0; k++)
        {
            int randomIndex = Random.Range(0, _objectCoordinates.Count);
            var (x, y) = _objectCoordinates[randomIndex];
            _map[x, y].SetColor(Color.gray);
            _map[x, y].InitItem(ObjectType.Smoke);
            _objectCoordinates.RemoveAt(randomIndex);
        }
    }

    private void InitObstacles()
    {
        int obstacleCount = _currentLevelData.obstacleCount;

        // Randomly select ITEM_COUNT coordinates and set to green
        for (int k = 0; k < obstacleCount && _objectCoordinates.Count > 0; k++)
        {
            int randomIndex = Random.Range(0, _objectCoordinates.Count);
            var (x, y) = _objectCoordinates[randomIndex];
            _map[x, y].SetColor(Color.green);
            _map[x, y].InitItem(ObjectType.Obstacle);
            _objectCoordinates.RemoveAt(randomIndex);
        }
    }

    private bool IsCenterCoordinates(int col, int row, int totalColumns, int totalRows)
    {
        Vector2 center = new Vector2(totalColumns / 2f, totalRows / 2f);
        HashSet<(int, int)> centerOffsets = new()
        {
            ((int)center.x, (int)center.y),
            
            ((int)center.x, (int)center.y + 1),
            ((int)center.x, (int)center.y - 1),
            ((int)center.x + 1, (int)center.y),
            ((int)center.x - 1, (int)center.y),
            
            //((int)center.x - 1, (int)center.y - 1),
            //((int)center.x + 1, (int)center.y + 1),
            //((int)center.x - 1, (int)center.y + 1),
            //((int)center.x + 1, (int)center.y - 1),
        };

        return centerOffsets.Contains((col, row));
    }
}
