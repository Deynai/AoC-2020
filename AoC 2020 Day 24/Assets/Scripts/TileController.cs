using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileController : MonoBehaviour
{
    public TileBase whiteTile;
    public TileBase blackTile;
    public TileBase[] numberTiles;
    public TileBase patternTile;

    public Tilemap ActiveTilemap;
    public Tilemap BackgroundTilemap;
    public Tilemap numberTilemap;

    public void RunTest()
    {
        (int x, int y)[] tiles = { (0, 0), (1, 0), (3, 0), (0, -2), (0, -4) };

        foreach((int x, int y) pos in tiles)
        {
            int posy = pos.y < 0 ? pos.y - 1 : pos.y;
            ActiveTilemap.SetTile(new Vector3Int(pos.x + (posy/2), pos.y, 0), blackTile);
        }
    }

    public void DrawActiveTiles(Dictionary<(int x, int y), int> active_tiles)
    {
        ActiveTilemap.ClearAllTiles();
        //numberTilemap.ClearAllTiles();

        foreach((int x, int y) pos in active_tiles.Keys)
        {
            int posy = pos.y < 0 ? pos.y - 1 : pos.y;
            ActiveTilemap.SetTile(new Vector3Int(pos.x + (posy / 2), pos.y, 0), blackTile);
            //numberTilemap.SetTile(new Vector3Int(pos.x + (posy / 2), pos.y, 0), patternTile);
        }
    }

    public void DrawNeighbourTiles(Dictionary<(int x, int y), int> neighbour_tiles)
    {
        //BackgroundTilemap.ClearAllTiles();
        //numberTilemap.ClearAllTiles();

       foreach((int x, int y) pos in neighbour_tiles.Keys)
        {
            int posy = pos.y < 0 ? pos.y - 1 : pos.y;
            BackgroundTilemap.SetTile(new Vector3Int(pos.x + (posy / 2), pos.y, 0), whiteTile);
            //numberTilemap.SetTile(new Vector3Int(pos.x + (posy / 2), pos.y, 0), numberTiles[neighbour_tiles[pos] - 1]);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
