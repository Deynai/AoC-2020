using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using TMPro;

public class MainController : MonoBehaviour
{
    public GameObject tilemapObject;
    private TilemapController tilemapController;

    public GameObject occupiedseatsText;

    private IEnumerator Main()
    {
        string[] input = System.IO.File.ReadAllLines("./Assets/Input/day11input.txt");
        int[,] inputgrid = new int[input.Length, input[0].Length];
        for(int i = 0; i < input.Length; i++)
        {
            char[] line = input[i].ToCharArray();

            for(int j = 0; j < input[0].Length; j++)
            {
                if (line[j].Equals('.')){
                    inputgrid[i,j] = 0;
                }
                else if (line[j].Equals('L'))
                {
                    inputgrid[i,j] = 1;
                }
                else
                {
                    inputgrid[i,j] = 2;
                }
            }
        }

        tilemapController.DrawTiles(inputgrid);

        yield return Part1(inputgrid);

        yield return new WaitForSeconds(5.0f);

        yield return Part2(inputgrid);
    }

    private IEnumerator Part1(int[,] inputgrid)
    {
        int[,] gridcopy = (int[,]) inputgrid.Clone();
        int[,] gridcopy2 = (int[,]) inputgrid.Clone();

        bool hasChanged = true;

        while (hasChanged)
        {
            hasChanged = CalculateOneStep(gridcopy, gridcopy2);
            // tilemapController.DrawTiles(gridcopy2);
            // yield return new WaitForSeconds(0.1f);
            hasChanged = CalculateOneStep(gridcopy2, gridcopy);
            //tilemapController.DrawTiles(gridcopy);

            occupiedseatsText.GetComponent<TextMeshProUGUI>().text = CountSeats(gridcopy).ToString();

            yield return new WaitForSeconds(0.3f);
        }

        //Debug.Log("Occupied seats: " + count);
    }

    private IEnumerator Part2(int [,] inputgrid)
    {
        int[,] gridcopy = (int[,])inputgrid.Clone();
        int[,] gridcopy2 = (int[,])inputgrid.Clone();

        bool hasChanged = true;

        while (hasChanged)
        {
            hasChanged = CalculateOneStep2(gridcopy, gridcopy2);
            //tilemapController.DrawTiles(gridcopy2);
            //yield return new WaitForSeconds(0.5f);
            
            hasChanged = CalculateOneStep2(gridcopy2, gridcopy);
            //tilemapController.DrawTiles(gridcopy);

            occupiedseatsText.GetComponent<TextMeshProUGUI>().text = CountSeats(gridcopy).ToString();

            yield return new WaitForSeconds(0.3f);
        }
    }

    private int CountSeats(int [,] array)
    {
        int count = 0;
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                if (array[i, j].Equals(2))
                {
                    count++;
                }
            }
        }
        return count;
    }

    private bool CalculateOneStep(int[,] array1, int[,] array2)
    {
        bool hasChanged = false;

        for(int i = 0; i < array1.GetLength(0); i++)
        {
            for(int j = 0; j < array1.GetLength(1); j++)
            {
                if (array1[i, j].Equals(1) && SumAdjacent(i, j, array1, 2).Equals(0))
                {
                    array2[i, j] = 2;
                    tilemapController.DrawTile(i, j, 2);
                    hasChanged = true;
                }
                else if (array1[i, j].Equals(2) && SumAdjacent(i, j, array1, 2) >= 4)
                {
                    array2[i, j] = 1;
                    tilemapController.DrawTile(i, j, 1);
                    hasChanged = true;
                }
                else
                {
                    array2[i, j] = array1[i, j];
                }
            }
        }

        return hasChanged;
    }

    private bool CalculateOneStep2(int[,] array1, int[,] array2)
    {
        bool hasChanged = false;

        for (int i = 0; i < array1.GetLength(0); i++)
        {
            for (int j = 0; j < array1.GetLength(1); j++)
            {
                if (array1[i, j].Equals(1) && SumAdjacent2(i, j, array1, 2, 1).Equals(0))
                {
                    array2[i, j] = 2;
                    tilemapController.DrawTile(i, j, 2); //
                    hasChanged = true;
                }
                else if (array1[i, j].Equals(2) && SumAdjacent2(i, j, array1, 2, 1) >= 5)
                {
                    array2[i, j] = 1;
                    tilemapController.DrawTile(i, j, 1); //
                    hasChanged = true;
                }
                else
                {
                    array2[i, j] = array1[i, j];
                    //tilemapController.DrawTile(i, j, array1[i,j]);
                }
            }
        }

        return hasChanged;
    }

    private int SumAdjacent(int i, int j, int[,] array, int val)
    {
         int sum = 0;

        if (i.Equals(0))
        {
            if (j.Equals(0))
            {
                sum += array[i + 1, j].Equals(val) ? 1 : 0;
                sum += array[i + 1, j + 1].Equals(val) ? 1 : 0;
                sum += array[i, j + 1].Equals(val) ? 1 : 0;
            }
            else if (j.Equals(array.GetLength(1) - 1))
            {
                sum += array[i + 1, j].Equals(val) ? 1 : 0;
                sum += array[i + 1, j - 1].Equals(val) ? 1 : 0;
                sum += array[i, j - 1].Equals(val) ? 1 : 0;
            }
            else
            {
                sum += array[i + 1, j].Equals(val) ? 1 : 0;
                sum += array[i + 1, j + 1].Equals(val) ? 1 : 0;
                sum += array[i, j + 1].Equals(val) ? 1 : 0;
                sum += array[i + 1, j - 1].Equals(val) ? 1 : 0;
                sum += array[i, j - 1].Equals(val) ? 1 : 0;
            }
        }

        else if (i.Equals(array.GetLength(0) - 1))
        {
            if (j.Equals(0))
            {
                sum += array[i - 1, j].Equals(val) ? 1 : 0;
                sum += array[i - 1, j + 1].Equals(val) ? 1 : 0;
                sum += array[i, j + 1].Equals(val) ? 1 : 0;
            }
            else if (j.Equals(array.GetLength(1) - 1))
            {
                sum += array[i - 1, j].Equals(val) ? 1 : 0;
                sum += array[i - 1, j - 1].Equals(val) ? 1 : 0;
                sum += array[i, j - 1].Equals(val) ? 1 : 0;
            }
            else
            {
                sum += array[i - 1, j].Equals(val) ? 1 : 0;
                sum += array[i - 1, j + 1].Equals(val) ? 1 : 0;
                sum += array[i, j + 1].Equals(val) ? 1 : 0;
                sum += array[i - 1, j - 1].Equals(val) ? 1 : 0;
                sum += array[i, j - 1].Equals(val) ? 1 : 0;
            }
        }

        else
        {
            if (j.Equals(0))
            {
                sum += array[i - 1, j].Equals(val) ? 1 : 0;
                sum += array[i - 1, j+1].Equals(val) ? 1 : 0;
                sum += array[i, j+1].Equals(val) ? 1 : 0;
                sum += array[i+1, j].Equals(val) ? 1 : 0;
                sum += array[i + 1, j + 1].Equals(val) ? 1 : 0;
            }
            else if (j.Equals(array.GetLength(1) - 1))
            {
                sum += array[i - 1, j].Equals(val) ? 1 : 0;
                sum += array[i - 1, j - 1].Equals(val) ? 1 : 0;
                sum += array[i, j - 1].Equals(val) ? 1 : 0;
                sum += array[i + 1, j].Equals(val) ? 1 : 0;
                sum += array[i + 1, j - 1].Equals(val) ? 1 : 0;
            }
            else
            {
                sum += array[i - 1, j].Equals(val) ? 1 : 0;
                sum += array[i - 1, j + 1].Equals(val) ? 1 : 0;
                sum += array[i, j + 1].Equals(val) ? 1 : 0;
                sum += array[i + 1, j].Equals(val) ? 1 : 0;
                sum += array[i + 1, j + 1].Equals(val) ? 1 : 0;
                sum += array[i - 1, j - 1].Equals(val) ? 1 : 0;
                sum += array[i, j-1].Equals(val) ? 1 : 0;
                sum += array[i+1, j-1].Equals(val) ? 1 : 0;
            }
        }

        return sum;
    }

    private int SumAdjacent2(int i, int j, int[,] array, int val, int block)
    {
        int sum = 0;
        for(int y = -1; y < 2; y++)
        {
            for(int z = -1; z < 2; z++)
            {
                if(y.Equals(0) && z.Equals(0))
                {
                    continue;
                }
                sum += GetAdjacentDir(i, j, y, z, array, val, block);
            }
        }

        return sum;
    }

    private int GetAdjacentDir(int i, int j, int di, int dj, int[,] array, int val, int block)
    {
        int n = 1;

        while(i + di*n >= 0 && i + di*n <= array.GetLength(0) - 1 && j + dj * n >= 0 && j + dj * n <= array.GetLength(1) - 1)
        {
            if (array[i + di * n, j + dj * n].Equals(val))
            {
                return 1;
            }
            else if(array[i + di * n, j + dj * n].Equals(block))
            {
                return 0;
            }
            else
            {
                n++;
            }
        }

        return 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        tilemapController = tilemapObject.GetComponent<TilemapController>();
        StartCoroutine("Main");
    }
}
