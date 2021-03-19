using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainController : MonoBehaviour
{
    private void Main()
    {
        string[] string_input = System.IO.File.ReadAllLines("./Assets/Input/day10input.txt");
        int[] input = new int[string_input.Length];
        for(int i = 0; i < string_input.Length; i++)
        {
            input[i] = int.Parse(string_input[i]);
        }
        Array.Sort(input);

        Part1(input);

        //Part2(input);

        //Memo_Test();

    }

    private void Part1(int[] input)
    {
        int[] counts = new int[4] { 0, 0, 0, 0 };
        int joltage = 0;

        for(int i = 0; i < input.Length; i++)
        {
            counts[input[i] - joltage]++;
            joltage = input[i];
        }

        // final adapter
        joltage += 3;
        counts[3]++;

        Debug.Log("0: " + counts[0] + "\t1: " + counts[1] + "\t2: " + counts[2] + "\t3: " + counts[3] + "\tProduct: " + (counts[1]*counts[3]));
    }

    private void Part2(int[] input)
    {
        List<int> groups = new List<int>();

        int i = 0;
        int count = 0;
        int joltage = 0;

        while(i < input.Length)
        {
            if((input[i] - joltage).Equals(1))
            {
                count++;
                joltage = input[i];
            }
            else
            {
                if (!count.Equals(0))
                {
                    groups.Add(count);
                }
                count = 0;
                joltage = input[i];
            }
            i++;
        }

        if (!count.Equals(0))
        {
            groups.Add(count);
        }

        List<long> memo_data = new List<long>();
        memo_data.Add(1L); memo_data.Add(2L); memo_data.Add(4L);

        List<long> fib_groups = new List<long>();
        long product = 1;
        foreach(int num in groups)
        {
            long result = FindFibSum(num, memo_data);
            fib_groups.Add(result);
            product *= result;
        }

        Debug.Log("Part2 Permutations: " + product);
    }

    private void Memo_Test()
    {
        List<long> memo_data = new List<long>();
        memo_data.Add(1L); memo_data.Add(2L); memo_data.Add(4L);

        var watch = new System.Diagnostics.Stopwatch();
        watch.Start();

        Debug.Log(FindFibSum(40, memo_data));

        watch.Stop();
        Debug.Log("First Memo Test Time Elapsed: " + watch.ElapsedMilliseconds + "ms");

        watch.Reset();
        watch.Start();

        Debug.Log(FindFibSum(40, memo_data));
        watch.Stop();

        Debug.Log("Second Memo Test Time Elapsed: " + watch.ElapsedMilliseconds + "ms");

        watch.Reset();
        watch.Start();

        Debug.Log(FindFibSum(40));

        watch.Stop();

        Debug.Log("Third Memo Test, [no memo]: " + watch.ElapsedMilliseconds + "ms");
    }

    private long FindFibSum(int n, List<long> memo_data)
    {
        if(n-1 < memo_data.Count)
        {
            return memo_data[n - 1];
        }
        else
        {
            long value = FindFibSum(n - 1, memo_data) + FindFibSum(n - 2, memo_data) + FindFibSum(n - 3, memo_data);
            memo_data.Add(value);
            return value;
        }
    }

    private long FindFibSum(int n)
    {
        if (n.Equals(1))
        {
            return 1;
        }
        else if (n.Equals(2))
        {
            return 2;
        }
        else if (n.Equals(3))
        {
            return 4;
        }
        else
        {
            return FindFibSum(n - 1) + FindFibSum(n - 2) + FindFibSum(n - 3);
            //return 0;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Main();
    }
}
