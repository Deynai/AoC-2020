using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;
using System;
using System.Diagnostics;

public class MainController : MonoBehaviour
{
    private void Main()
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Regex reg = new Regex("\\d+");
        string[] input = System.IO.File.ReadLines("./Assets/Input/day16input.txt").Where(a => reg.Matches(a).Count.Equals(4)).ToArray();

        List<int[]> criteria = System.IO.File.ReadLines("./Assets/Input/day16input.txt").Where(a => reg.Matches(a).Count.Equals(4)).Select(a => reg.Matches(a).Cast<Match>().Select(m => int.Parse(m.Value)).ToArray()).ToList();
        List<int[]> inputTickets = System.IO.File.ReadLines("./Assets/Input/day16input.txt").Where(a => reg.Matches(a).Count.Equals(20)).Select(a => Array.ConvertAll(a.Split(','), s => int.Parse(s))).ToList();

        Part1(criteria, inputTickets);

        Part2(criteria, inputTickets);

        sw.Stop();
        UnityEngine.Debug.Log(sw.ElapsedMilliseconds + "ms");
    }

    private void Part1(List<int[]> criteria, List<int[]> inputTickets)
    {
        int min = 99999;
        int max = 0;
        int sum = 0;

        // this is dubious - theoretically there can be a gap in the middle, but a quick inspection of the input shows this isn't the case.
        foreach (int[] bounds in criteria)
        {
            min = Mathf.Min(bounds[0], min);
            max = Mathf.Max(bounds[3], max);
        }

        List<int> lines_to_delete = new List<int>();

        for (int i = 1; i < inputTickets.Count; i++)
        {
            foreach (int num in inputTickets[i])
            {
                if (num < min || num > max)
                {
                    sum += num;
                    lines_to_delete.Add(i);
                }
            }
        }

        UnityEngine.Debug.Log("Error Rate: " + sum);

        // Important to order by descending, deleting the later ones first, to maintain index positions
        foreach (int i in lines_to_delete.OrderByDescending(i => i))
        {
            inputTickets.RemoveAt(i); 
        }
    }

    private void Part2(List<int[]> criteria, List<int[]> inputTickets)
    {
        List<int>[] validColumns = new List<int>[20];

        for(int i = 0; i < validColumns.Length; i++)
        {
            List<int> newlist = new List<int>();
            validColumns[i] = newlist;

            for(int j = 0; j < inputTickets[0].Length; j++)
            {
                bool isValid = true;
                foreach (int[] num in inputTickets)
                {
                    if (!isValidBounds(num[j], criteria[i]))
                    {
                        isValid = false;
                    }
                }

                if (isValid)
                {
                    validColumns[i].Add(j);
                }
            }
        }

        // We now have a list[20] where the ith element contains the index of the columns which are valid for the bounds of the ith criterion.

        // We can now reduce each list by the process of elimination - if one element contains only one valid column, we can remove that column from all other criterion - if there is to be a unique solution then it should reduce easily.

        bool hasChanged = true;
        int loopbreak = 0;
        while (hasChanged)
        {
            hasChanged = false;
            for(int i = 0; i < validColumns.Length; i++)
            {
                if (validColumns[i].Count.Equals(1))
                {
                    for(int j = 0; j < validColumns.Length; j++)
                    {
                        if (j.Equals(i) || validColumns[j].Count.Equals(1))
                        {
                            continue;
                        }
                        else
                        {
                            validColumns[j].Remove(validColumns[i][0]);
                            hasChanged = true;
                        }

                        loopbreak++;
                        if(loopbreak >= 1000000)
                        {
                            UnityEngine.Debug.Log("loopbreak triggered");
                            return;
                        }
                    }
                }
            }

        }

        // We have now matched up each column with a single criterion, now we need only multiply the first 6 criteria values of our ticket.

        long product = 1;

        for(int i = 0; i < 6; i++)
        {
            // inputTickets[0] is our ticket, validColumns[i][0] corresponds to the column of criterion i
            product *= inputTickets[0][validColumns[i][0]];
        }

        UnityEngine.Debug.Log("Ticket Product: " + product);
    }

    private bool isValidBounds(int num, int[] bounds)
    {
        return ((num >= bounds[0] && num <= bounds[1]) || (num >= bounds[2] && num <= bounds[3]));
    }

    void Start()
    {
        Main();
    }
}
