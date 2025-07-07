using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public Transform grid;
    public int gridSizeX = 4;
    public int gridSizeZ = 4;
    private Vector3 tileSize;

    public List<TileController> tiles = new List<TileController>();

    void Start()
    {
        CalculateTileSize();
        GenerateGrid();
    }

    void CalculateTileSize()
    {
        if (tilePrefab.TryGetComponent(out Renderer renderer))
        {
            tileSize = renderer.bounds.size;
        }
        else
        {
            Debug.LogWarning("Tile prefab has no renderer. Defaulting to size 1.");
            tileSize = Vector3.one;
        }
    }

    void GenerateGrid()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
           
            for (int z = 0; z < gridSizeZ; z++)
            {
                Vector3 pos = new Vector3(
                    x * tileSize.x + tileSize.x / 2f,
                    grid.position.y,
                    z * tileSize.z + tileSize.z / 2f
                );

                GameObject tile = Instantiate(tilePrefab, pos, Quaternion.identity, grid);
                tile.name = $"Tile_{x}_{z}";

                TileController controller = tile.GetComponent<TileController>();
                tiles.Add(controller);
                
            }
        }
    }

    public void HighlightTiles(bool active)
        {
            foreach (TileController tile in tiles)
            {
            
                tile.SetActive(active);
            }
        }

        public Vector3 GetTilePosition(TileController tile)
        {
            return tile.transform.position;
        }
}
