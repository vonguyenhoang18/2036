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

    private UIManager _uiManager => GameManager.Instance.UIManager;
    private InputManager _inputManager => GameManager.Instance.InputManager;
    private MapManager _mapManager => GameManager.Instance.MapManager;
    private ItemManager _itemManager => GameManager.Instance.ItemManager;
    private CharacterManager _characterManager => GameManager.Instance.CharacterManager;
    private AudioManager _audioManager => GameManager.Instance.AudioManager;
    private InventoryManager _inventoryManager => GameManager.Instance.InventoryManager;

    public void InitMap()
    {
        _currentLevelData = levelDataConfig.GetLevelData(_mapManager.CurrentLevel);
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

    private void GenerateGrid()
    {
        int columns = _currentLevelData.columns;
        int rows = _currentLevelData.rows;

        _map = new MapObject[columns, rows];
        float totalWidth = (columns - 1) * GameConstant.ITEM_GAP;
        float totalHeight = (rows - 1) * GameConstant.ITEM_GAP;
        float xOffset = totalWidth / 2f;
        float yOffset = totalHeight / 2f;
        int id = 1;
        
        for (int j = 0; j < _currentLevelData.rows; j++)
        {
            for (int i = 0; i < columns; i++)
            {
                var item = Instantiate(itemPrefab, itemMap);
                float xPos = i * GameConstant.ITEM_GAP - xOffset;
                float yPos = j * GameConstant.ITEM_GAP - yOffset;
                item.transform.localPosition = new Vector2(xPos, yPos);
                
                MapObject mapItem = item.GetComponent<MapObject>();
                mapItem.SetDebug(i, j, id++);
                _map[i, j] = mapItem;
                
                // The edge of the map is red, with the size is 1
                if (i == columns - 1 || i == 0 || 
                    j == rows - 1 || j == 0)
                {
                    mapItem.SetColor(Color.red);
                    mapItem.InitItem(ObjectType.Edge);
                }
                // The center of the map is yellow, with the size is 3x3
                else if (IsCenterCoordinates(i, j))
                {
                    mapItem.SetColor(Color.yellow);
                }
                else
                {
                    _objectCoordinates.Add((i, j));
                }
            }
        }

        Debug.Log($"DevHoang CurrentLevel {_mapManager.CurrentLevel}");
        InitExit();
        if (_mapManager.CurrentLevel == 4)
        {
            InitNPCA();
        }
        InitMedKit();
        InitObstacles();
    }

    private void InitObjectAtTargetDistance(ObjectType objectType, Color color, float distanceFraction = 0.8f)
    {
        int columns = _currentLevelData.columns;
        int rows = _currentLevelData.rows;
        float centerX = columns / 2f;
        float centerY = rows / 2f;

        float maxDist = Vector2.Distance(
            new Vector2(centerX, centerY),
            new Vector2(1, 1)
        );
        float targetDist = maxDist * distanceFraction;

        int bestIndex = -1;
        float bestDelta = float.MaxValue;

        for (int i = 0; i < _objectCoordinates.Count; i++)
        {
            var (x, y) = _objectCoordinates[i];
            float dist = Vector2.Distance(new Vector2(x, y), new Vector2(centerX, centerY));
            float delta = Mathf.Abs(dist - targetDist);
            if (delta < bestDelta)
            {
                bestDelta = delta;
                bestIndex = i;
            }
        }

        Debug.Log($"!@#DevHoang bestIndex {bestIndex}");
        if (bestIndex < 0) return;

        var (ex, ey) = _objectCoordinates[bestIndex];
        _map[ex, ey].SetColor(color);
        _map[ex, ey].InitItem(objectType);
        _objectCoordinates.RemoveAt(bestIndex);
    }

    private void InitExit()
    {
        InitObjectAtTargetDistance(ObjectType.Exit, Color.blue, 0.8f);
    }

    private void InitNPCA()
    {
        InitObjectAtTargetDistance(ObjectType.NPCA, Color.blue, 0.8f);
    }

    private void InitMedKit()
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

    private bool IsCenterCoordinates(int col, int row)
    {
        int columns = _currentLevelData.columns;
        int rows = _currentLevelData.rows;

        Vector2 center = new Vector2(columns / 2f, rows / 2f);
        HashSet<(int, int)> centerOffsets = new()
        {
            ((int)center.x, (int)center.y),
            
            ((int)center.x, (int)center.y + 1),
            ((int)center.x, (int)center.y - 1),
            ((int)center.x + 1, (int)center.y),
            ((int)center.x - 1, (int)center.y),
            
            ((int)center.x - 1, (int)center.y - 1),
            ((int)center.x + 1, (int)center.y + 1),
            ((int)center.x - 1, (int)center.y + 1),
            ((int)center.x + 1, (int)center.y - 1),
        };

        return centerOffsets.Contains((col, row));
    }
}
