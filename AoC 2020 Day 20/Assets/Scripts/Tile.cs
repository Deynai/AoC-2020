using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System;

public class Tile
{
    public int id;
    public int[,] grid = new int[10,10];
    public int[] intsides = new int[4];
    public List<Tuple<int, int>> matches = new List<Tuple<int, int>>();
    
    private static Regex reg_id = new Regex("[\\d]+");

    public Tile(string[] tile_str)
    {
        id = int.Parse(reg_id.Match(tile_str[0]).Value);
        for (int i = 0; i < 10; i++)
        {
            for(int j = 0; j < 10; j++)
            {
                grid[i,j] = tile_str[i + 1][j].Equals('#') ? 1 : 0;
            }
        }

        CalcIntSides();
    }

    // rotate the tile 1, 2, or 3 times
    public void RotateTile(int r)
    {
        int[,] newGrid = new int[10, 10];
        for(int i = 0; i < 4; i++) { intsides[i] = 0; }

        if (r.Equals(1))
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    newGrid[j, 9-i] = grid[i,j];
                }
            }
        }

        if (r.Equals(2))
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    newGrid[9-i, 9 - j] = grid[i, j];
                }
            }
        }

        if (r.Equals(3))
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    newGrid[9 - j, i] = grid[i, j];
                }
            }
        }

        grid = newGrid;
        CalcIntSides();
    }

    public void FlipTile()
    {
        // we're just going to flip horizontally, about a vertical axis
        int[,] newGrid = new int[10, 10];

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                newGrid[i, 9 - j] = grid[i, j];
            }
        }

        grid = newGrid;
        CalcIntSides();
    }

    public void TrimBorders()
    {
        int[,] noBorderGrid = new int[grid.GetLength(0) - 2, grid.GetLength(1) - 2];

        for(int i = 1; i < grid.GetLength(0)-1; i++)
        {
            for(int j = 1; j < grid.GetLength(1)-1; j++)
            {
                noBorderGrid[i - 1, j - 1] = grid[i, j];
            }
        }

        grid = noBorderGrid;
    }

    private void CalcIntSides()
    {
        for (int i = 0; i < 4; i++) { intsides[i] = 0; }

        for (int i = 0; i < 10; i++)
        {
            if (grid[0, i].Equals(1))
            {
                intsides[0] += (int)Math.Pow(2, (9 - i));
            }
            if (grid[9, i].Equals(1))
            {
                intsides[2] += (int)Math.Pow(2, (9 - i));
            }
            if (grid[i, 0].Equals(1))
            {
                intsides[3] += (int)Math.Pow(2, (9 - i));
            }
            if (grid[i, 9].Equals(1))
            {
                intsides[1] += (int)Math.Pow(2, (9 - i));
            }
        }
    }

    public void AddMatch(int id, int side)
    {
        matches.Add(new Tuple<int,int>(id, side));
    }
}
