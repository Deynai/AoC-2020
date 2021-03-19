using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapController : MonoBehaviour
{
    public TileBase[] tile; // 0 floor, 1 seat, 2 person
    private Tilemap tilemap;

    public void DrawTiles(int[,] tiles)
    {
        tilemap.ClearAllTiles();
        for(int i = 0; i < tiles.GetLength(0); i++)
        {
            for(int j = 0; j < tiles.GetLength(1); j++)
            {
                tilemap.SetTile(new Vector3Int(j, -i, 0), tile[tiles[i,j]]);
            }
        }
    }

    public void DrawTile(int i, int j, int val)
    {
        tilemap.SetTile(new Vector3Int(j, -i, 0), tile[val]);
    }

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }
}
