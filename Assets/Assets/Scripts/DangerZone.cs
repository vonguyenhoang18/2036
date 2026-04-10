using System.Collections.Generic;
using UnityEngine;

public class DangerZone : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform mapParent;
    
    private const int COLUMN_SIZE = 11;
    private const int ROW_SIZE = 11;
    private const int ITEM_COUNT = 20;
    private const int ITEM_GAP = 3;
    
    private MapItem[,] _map = new MapItem[COLUMN_SIZE, ROW_SIZE];
    
    public void InitMap()
    {
        GenerateGrid();
        
    }

    private void GenerateGrid()
    {
        List<(int x, int y)> availableCoordinates = new();
        
        float totalWidth = (COLUMN_SIZE - 1) * ITEM_GAP;
        float totalHeight = (ROW_SIZE - 1) * ITEM_GAP;
        float xOffset = totalWidth / 2f;
        float yOffset = totalHeight / 2f;
        int id = 1;
        
        for (int j = 0; j < ROW_SIZE; j++)
        {
            for (int i = 0; i < COLUMN_SIZE; i++)
            {
                var item = Instantiate(itemPrefab, mapParent);
                float xPos = i * ITEM_GAP - xOffset;
                float yPos = j * ITEM_GAP - yOffset;
                item.transform.localPosition = new Vector2(xPos, yPos);
                
                MapItem mapItem = item.GetComponent<MapItem>();
                mapItem.SetDebug(i, j, id++);
                _map[i, j] = mapItem;
                
                if (i == COLUMN_SIZE - 1 || i == 0 || 
                    j == ROW_SIZE - 1 || j == 0)
                {
                    mapItem.SetColor(Color.red);
                } 
                else if (IsCenterCoordinates(i, j))
                {
                    mapItem.SetColor(Color.yellow);
                }
                else
                {
                    availableCoordinates.Add((i, j));
                }
            }
        }
        
        // Randomly select ITEM_COUNT coordinates and set to green
        for (int k = 0; k < ITEM_COUNT && availableCoordinates.Count > 0; k++)
        {
            int randomIndex = Random.Range(0, availableCoordinates.Count);
            var (x, y) = availableCoordinates[randomIndex];
            _map[x, y].SetColor(Color.green);
            availableCoordinates.RemoveAt(randomIndex);
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
    
    private void GenerateItems()
    {
        
    }
}
