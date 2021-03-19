using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;

public class MainController : MonoBehaviour
{
    private const int TD = 10; // tile dimension
    private const int PD = 12; // puzzle dimension
    private const int TD_POW = 1024;

    public GameObject whiteSquare;
    public GameObject blackSquare;
    public GameObject greenSquare;
    public GameObject panel;

    private IEnumerator Main()
    {
        string[] input = System.IO.File.ReadAllLines("./Assets/Input/day20input.txt");
        List<Tile> tiles = new List<Tile>();

        for(int i = 0; i < (input.Length+1)/(TD+2); i++)
        {
            string[] tile_str = new string[(TD + 2)];
            Array.ConstrainedCopy(input, i * (TD + 2), tile_str, 0, (TD + 1));
            Tile newtile = new Tile(tile_str);
            tiles.Add(newtile);
        }

        Part1(tiles);

        Tile[,] puzzle = ConstructPuzzle(tiles);

        //yield return PrintAllTiles(puzzle);

        yield return TrimTileBorders(puzzle);

        //yield return PrintAllTiles(puzzle);

        int[,] intpuzzle = TilesToInt(puzzle);

        //yield return PrintAllIntTiles(intpuzzle);

        int[] count = new int[8]; for (int i = 0; i < 8; i++) 
        { 
            count[i] = 0;
        }
        int rot = 0;
        count[rot] = FindMonsters(intpuzzle);
        yield return PrintAllIntTiles(intpuzzle);
        yield return new WaitForSeconds(5.0f);

        for (int i = 0; i < 3; i++)
        {
            rot++;
            intpuzzle = RotatePuzzle(intpuzzle);
            yield return PrintAllIntTiles(intpuzzle);
            yield return new WaitForSeconds(5.0f);
            count[rot] = FindMonsters(intpuzzle);
        }

        rot++;
        intpuzzle = FlipPuzzle(intpuzzle);
        count[rot] = FindMonsters(intpuzzle);
        yield return PrintAllIntTiles(intpuzzle);
        yield return new WaitForSeconds(5.0f);

        for (int i = 0; i < 3; i++)
        {
            rot++;
            intpuzzle = RotatePuzzle(intpuzzle);
            count[rot] = FindMonsters(intpuzzle);
            yield return PrintAllIntTiles(intpuzzle);
            yield return new WaitForSeconds(5.0f);
        }

        int ones = 0;
        for(int i = 0; i<intpuzzle.GetLength(0); i++)
        {
            for (int j = 0; j < intpuzzle.GetLength(0); j++)
            {
                ones += intpuzzle[i, j].Equals(1) ? 1 : 0;
            }
        }

        for (int i = 0; i < 8; i++)
        {
            if(count[i] > 0)
            {
                int waves = ones;
                Debug.Log("Waves: " + waves);

                Debug.Log("Number of Monsters: " + count[i]);
            }
        }
    }

    private void Part1(List<Tile> tiles)
    {
        foreach(Tile startTile in tiles)
        {
            foreach(Tile compareTile in tiles)
            {
                if (startTile.Equals(compareTile)) { continue; }

                foreach(int s_side in startTile.intsides)
                {
                    foreach(int c_side in compareTile.intsides)
                    {
                        if(s_side.Equals(c_side) || s_side.Equals(Flip(c_side)))
                        {
                            startTile.AddMatch(compareTile.id, s_side);
                            startTile.AddMatch(compareTile.id, Flip(s_side));
                        }
                    }
                }
            }
        }

        int[] counts = new int[10]; for (int i = 0; i < 10; i++) { counts[i] = 0; }
        long prod = 1;

        foreach(Tile tile in tiles)
        {
            counts[tile.matches.Count]++;

            if (tile.matches.Count.Equals(4))
            {
                prod *= tile.id;
            }
        }
        Debug.Log("Product of corners: " + prod);
    }

    private IEnumerator TrimTileBorders(Tile[,] tiles)
    {
        for (int i = 0; i < PD; i++)
        {
            for (int j = 0; j < PD; j++)
            {
                tiles[i, j].TrimBorders();
            }
        }

        yield break;
    }

    private Tile[,] ConstructPuzzle(List<Tile> tiles)
    {
        Tile[,] puzzle = new Tile[PD, PD];

        puzzle[0, 0] = tiles.Where(t => t.matches.Count.Equals(4)).First();
        tiles.Remove(puzzle[0, 0]);

        // rotate first piece until the matching sides are 1 and 2 - might be buggy depending on some inputs, but ours is already in the right place.
        int bp = 0;
        while(!puzzle[0,0].matches[0].Item2.Equals(puzzle[0,0].intsides[1]) || !puzzle[0, 0].matches[2].Item2.Equals(puzzle[0, 0].intsides[2]))
        {
            puzzle[0, 0].RotateTile(1);
            bp++;
            if(bp > 20)
            {
                break;
            }
        }

        for(int i = 0; i < PD; i++)
        {
            if (!i.Equals(0))
            {
                int sideValue = puzzle[i - 1, 0].intsides[2];
                int nextTileId = puzzle[i - 1, 0].matches.Where(tup => tup.Item2.Equals(sideValue)).Select(tup => tup.Item1).First();
                Tile nextTile = tiles.Where(t => t.id.Equals(nextTileId)).First();
                tiles.Remove(nextTile); // might not be needed - just speeds up searching a tiny bit
                MatchSide(2, sideValue, nextTile);
                puzzle[i, 0] = nextTile;
            }

            for(int j = 1; j < PD; j++)
            {
                int sideValue = puzzle[i, j - 1].intsides[1];
                int nextTileId = puzzle[i, j - 1].matches.Where(tup => tup.Item2.Equals(sideValue)).Select(tup => tup.Item1).First();
                Tile nextTile = tiles.Where(t => t.id.Equals(nextTileId)).First();
                tiles.Remove(nextTile); // might not be needed - just speeds up searching a tiny bit
                MatchSide(1, sideValue, nextTile);
                puzzle[i, j] = nextTile;
            }
        }

        return puzzle;
    }

    private int[,] TilesToInt(Tile[,] puzzle)
    {
        int[,] intpuzzle = new int[PD * (TD-2), PD * (TD-2)];

        for(int i = 0; i < PD*(TD-2); i++)
        {
            for(int j = 0; j < PD* (TD - 2); j++)
            {
                intpuzzle[i, j] = puzzle[i / (TD - 2), j / (TD - 2)].grid[i % (TD - 2), j % (TD - 2)];
            }
        }

        return intpuzzle;
    }

    private int FindMonsters(int[,] p)
    {
        int count = 0;

        for(int i = 0; i < (PD* (TD - 2)) -2; i++)
        {
            for (int j = 0; j < (PD * (TD - 2)) -19; j++)
            {
                if ((p[i, j + 18]
                    + p[i+1,j] + p[i + 1,j+5] + p[i + 1,j + 6] + p[i + 1, j + 11] + p[i + 1, j + 12] + p[i + 1, j + 17] + p[i + 1, j + 18] + p[i + 1, j + 19]  
                    + p[i+2, j+1] + p[i + 2, j + 4] + p[i + 2, j + 7] + p[i + 2, j + 10] + p[i + 2, j + 13] + p[i + 2, j + 16]).Equals(15))
                {
                    p[i, j + 18] = 20;
                    p[i + 1, j] = 20; p[i + 1, j + 5] = 20; p[i + 1, j + 6] = 20; p[i + 1, j + 11] = 20; p[i + 1, j + 12] = 20; p[i + 1, j + 17] = 20; p[i + 1, j + 18] = 20; p[i + 1, j + 19] = 20;
                    p[i + 2, j + 1] = 20; p[i + 2, j + 4] = 20; p[i + 2, j + 7] = 20; p[i + 2, j + 10] = 20; p[i + 2, j + 13] = 20; p[i + 2, j + 16] = 20;
                    count++;
                }
            }
        }

        return count;
    }

    private int[,] RotatePuzzle(int [,] puzzle)
    {
        int[,] newPuzzle = new int[(TD - 2) * PD, (TD - 2) * PD];
        for(int i = 0; i < (TD - 2) * PD; i++)
        {
            for (int j = 0; j < (TD - 2) * PD; j++)
            {
                newPuzzle[j, (TD - 2) * PD - 1 - i] = puzzle[i, j];
            }
        }
        return newPuzzle;
    }

    private int[,] FlipPuzzle(int[,] puzzle)
    {
        int[,] newPuzzle = new int[(TD - 2) * PD, (TD - 2) * PD];
        for (int i = 0; i < (TD - 2) * PD; i++)
        {
            for (int j = 0; j < (TD - 2) * PD; j++)
            {
                newPuzzle[i, (TD - 2) * PD - 1 - j] = puzzle[i, j];
            }
        }
        return newPuzzle;
    }

    private void MatchSide(int side, int num, Tile nextTile)
    {
        int matchside = (side+2) % 4;

        if (!nextTile.intsides.Contains(num))
        {
            nextTile.RotateTile(2);

            if (!nextTile.intsides.Contains(num))
            {
                Debug.Log("Error in MatchSide(): Tile does not contain matching side. Id: " + nextTile.id);
                return;
            }
        }

        if (!nextTile.intsides[matchside].Equals(num))
        {
            for(int i = 0; i<3; i++)
            {
                nextTile.RotateTile(1);
                if (nextTile.intsides[matchside].Equals(num))
                {
                    break;
                }
            }

            if (!nextTile.intsides[matchside].Equals(num))
            {

                nextTile.FlipTile();
            }

            if (!nextTile.intsides[matchside].Equals(num))
            {
                for (int i = 0; i < 3; i++)
                {
                    nextTile.RotateTile(1);
                    if (nextTile.intsides[matchside].Equals(num))
                    {
                        break;
                    }
                }
            }
        }
    }

    private int Flip(int num)
    {
        int flipNum = 0;
        int pow = TD - 1;
        while(num != 0)
        {
            flipNum += (num & 1) << pow;
            num = num >> 1;
            pow -= 1;
        }

        return flipNum;
    }

    private IEnumerator PrintTile(Tile tile, int x, int y)
    {
        /*
        foreach(Transform child in panel.transform)
        {
            Destroy(child.gameObject);
        }
        */

        for(int i = 0; i < tile.grid.GetLength(0); i++)
        {
            for (int j = 0; j < tile.grid.GetLength(0); j++)
            {
                if (tile.grid[i, j].Equals(1))
                {
                    GameObject sq = Instantiate(blackSquare, panel.transform, false);
                    sq.transform.localPosition = new Vector2((j * 50) - 250 + x, (-i * 50 + 250) - y);
                }
                else
                {
                    GameObject sq = Instantiate(whiteSquare, panel.transform, false);
                    sq.transform.localPosition = new Vector2((j * 50) - 250 + x, (-i * 50 + 250) - y);
                }
            }
        }

        yield break;
    }

    private IEnumerator PrintAllTiles(Tile[,] tiles)
    {
        for(int i = 0; i < tiles.GetLength(0); i++)
        {
            for(int j = 0; j < tiles.GetLength(0); j++)
            {
                yield return PrintTile(tiles[i, j], j * 50 * tiles[0,0].grid.GetLength(0), i * 50 * tiles[0, 0].grid.GetLength(0));
                yield return new WaitForSeconds(0.5f);
            }
        }

        yield break;
    }

    private IEnumerator PrintAllIntTiles(int[,] puzzle)
    {
        foreach (Transform child in panel.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < puzzle.GetLength(0); i++)
        {
            for (int j = 0; j < puzzle.GetLength(0); j++)
            {
                if (puzzle[i, j].Equals(1))
                {
                    GameObject sq = Instantiate(blackSquare, panel.transform, false);
                    sq.transform.localPosition = new Vector2((j * 50) - 250, (-i * 50 + 250));
                }
                else if(puzzle[i,j].Equals(0))
                {
                    GameObject sq = Instantiate(whiteSquare, panel.transform, false);
                    sq.transform.localPosition = new Vector2((j * 50) - 250, (-i * 50 + 250));
                }
                else
                {
                    GameObject sq = Instantiate(blackSquare, panel.transform, false);
                    sq.GetComponent<Image>().color = Color.green;
                    sq.transform.localPosition = new Vector2((j * 50) - 250, (-i * 50 + 250));
                }
            }
        }
        yield break;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Main");
    }
}
