using System.Collections.Generic;
using UnityEngine;

public class DangerZone : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform itemMap;
    
    private const int COLUMN_SIZE = 21;
    private const int ROW_SIZE = 21;
    private const int MEDKIT_COUNT = 5;
    private const int OBSTACLE_COUNT = 50;
    private const int ITEM_GAP = 3;
    
    private MapObject[,] _map = new MapObject[COLUMN_SIZE, ROW_SIZE];
    private List<(int x, int y)> _objectCoordinates = new();

    private LevelManager _levelManager => GameManager.Instance.LevelManager;

    public void InitMap()
    {
        GenerateGrid();
        _levelManager.StartLevel();
    }

    private void GenerateGrid()
    {
        float totalWidth = (COLUMN_SIZE - 1) * ITEM_GAP;
        float totalHeight = (ROW_SIZE - 1) * ITEM_GAP;
        float xOffset = totalWidth / 2f;
        float yOffset = totalHeight / 2f;
        int id = 1;
        
        for (int j = 0; j < ROW_SIZE; j++)
        {
            for (int i = 0; i < COLUMN_SIZE; i++)
            {
                var item = Instantiate(itemPrefab, itemMap);
                float xPos = i * ITEM_GAP - xOffset;
                float yPos = j * ITEM_GAP - yOffset;
                item.transform.localPosition = new Vector2(xPos, yPos);
                
                MapObject mapItem = item.GetComponent<MapObject>();
                mapItem.SetDebug(i, j, id++);
                _map[i, j] = mapItem;
                
                // The edge of the map is red, with the size is 1
                if (i == COLUMN_SIZE - 1 || i == 0 || 
                    j == ROW_SIZE - 1 || j == 0)
                {
                    mapItem.SetColor(Color.red);
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

        InitExit();
        InitMedKit();
        InitObstacles();
    }

    private void InitExit()
    {
        float centerX = COLUMN_SIZE / 2f;
        float centerY = ROW_SIZE / 2f;

        // Max distance from center to corner (excluding edge border)
        float maxDist = Vector2.Distance(
            new Vector2(centerX, centerY),
            new Vector2(1, 1) // closest inner corner to (0,0)
        );

        // Target distance for exit is 80% of maxDist
        float targetDist = maxDist * 0.8f;

        // Find the coordinate in _objectCoordinates closest to targetDist from center
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

        if (bestIndex >= 0)
        {
            var (ex, ey) = _objectCoordinates[bestIndex];
            _map[ex, ey].SetColor(Color.blue);
            _map[ex, ey].InitItem(ObjectType.Exit);
            _objectCoordinates.RemoveAt(bestIndex);
        }
    }

    private void InitMedKit()
    {
        // Randomly select MEDKIT_COUNT coordinates and set to blue
        for (int k = 0; k < MEDKIT_COUNT && _objectCoordinates.Count > 0; k++)
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
        // Randomly select ITEM_COUNT coordinates and set to green
        for (int k = 0; k < OBSTACLE_COUNT && _objectCoordinates.Count > 0; k++)
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
        Vector2 center = new Vector2(COLUMN_SIZE / 2f, ROW_SIZE / 2f);
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
