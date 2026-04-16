using UnityEngine;

public class InfiniteTileMap : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;

    private float tileSizeX = 13.5f;
    private float tileSizeY = 7.9f;

    private GameObject[,] tiles = new GameObject[3, 3];
    private Vector2Int tileOrigin;

    void Start()
    {
        tileOrigin = new Vector2Int(-1, -1);
        for (int row = 0; row < 3; row++)
            for (int col = 0; col < 3; col++)
                SpawnTile(col, row);
    }

    void SpawnTile(int col, int row)
    {
        Vector2Int tileCoord = tileOrigin + new Vector2Int(col, row);
        Vector3 worldPos = new Vector3(
            tileCoord.x * tileSizeX,
            tileCoord.y * tileSizeY,
            0
        );
        tiles[col, row] = Instantiate(tilePrefab, worldPos, Quaternion.identity, transform);
        tiles[col, row].name = $"Tile({tileCoord.x},{tileCoord.y})";
    }

    void Update()
    {
        CheckAndRecycleTiles();
    }

    void CheckAndRecycleTiles()
    {
        Transform player = Camera.main.transform;

        // Tile edge = tileSizeX/2 + tileSizeX * n
        // So divide by tileSizeX, offset by 0.5, then floor → gives tile index at edge boundary
        Vector2Int charTile = new Vector2Int(
            Mathf.FloorToInt(player.position.x / tileSizeX + 0.5f),
            Mathf.FloorToInt(player.position.y / tileSizeY + 0.5f)
        );

        Vector2Int expectedOrigin = charTile - new Vector2Int(1, 1);
        if (expectedOrigin == tileOrigin) return;

        tileOrigin = expectedOrigin;

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                Vector2Int tileCoord = tileOrigin + new Vector2Int(col, row);
                tiles[col, row].transform.position = new Vector3(
                    tileCoord.x * tileSizeX,
                    tileCoord.y * tileSizeY,
                    0
                );
                // Update tile content for tileCoord here
            }
        }
    }
}