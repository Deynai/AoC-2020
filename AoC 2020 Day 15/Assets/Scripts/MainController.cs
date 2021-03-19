using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using System.Linq;

public class MainController : MonoBehaviour
{
    private void Main()
    {
        Stopwatch sw = new Stopwatch();

        sw.Start();

        long[] input = System.IO.File.ReadAllLines("./Assets/Input/day15input.txt")[0].Split(',').Select(a => long.Parse(a)).ToArray();

        UnityEngine.Debug.Log("(1) 2020th Number mentioned: " + Part1(input));

        //UnityEngine.Debug.Log("(2) 30000000th Number mentioned: " + Part2(input));

        UnityEngine.Debug.Log("(3) 30000000th Number mentioned: " + Part3(input));

        sw.Stop();

        UnityEngine.Debug.Log(sw.ElapsedMilliseconds + "ms");
    }

    private long Part1(long[] input)
    {
        Dictionary<long, NumberHistory> numbers = new Dictionary<long, NumberHistory>();
        long TARGET = 2020;
        long lastNumber = 0;

        for (int i = 0; i < input.Length; i++)
        {
            NumberHistory newNum = new NumberHistory();
            newNum.AddMention(i);
            numbers.Add(input[i], newNum);
            lastNumber = input[i];
        }

        for (int i = input.Length; i < TARGET; i++)
        {
            long thisNumber = numbers[lastNumber].GetLastMention(i);

            if (!numbers.ContainsKey(thisNumber))
            {
                NumberHistory newNum = new NumberHistory();
                numbers.Add(thisNumber, newNum);
            }

            numbers[thisNumber].AddMention(i);

            lastNumber = thisNumber;
        }

        return lastNumber;
    }

    private long Part2(long[] input)
    {
        Stopwatch sw = new Stopwatch();

        sw.Start();

        Dictionary<int, int> numbers = new Dictionary<int, int>();
        long TARGET = 30000000;
        int lastNumber = 0;

        for (int i = 0; i < input.Length-1; i++)
        {
            numbers.Add((int) input[i], i);
        }

        lastNumber = (int) input[input.Length - 1];

        for (int i = input.Length; i < TARGET; i++)
        {
            if (!numbers.ContainsKey(lastNumber))
            {
                numbers.Add(lastNumber, i - 1);
                lastNumber = 0;
            }

            else
            {
                int difference = i - 1 - numbers[lastNumber];
                numbers.Remove(lastNumber);
                numbers.Add(lastNumber, i - 1);
                lastNumber = difference;
            }
        }

        sw.Stop();

        UnityEngine.Debug.Log(sw.ElapsedMilliseconds + "ms");

        return lastNumber;
    }

    private long Part3(long[] input)
    {
        Stopwatch sw = new Stopwatch();

        sw.Start();

        int TARGET = 30000000;
        int[] numbers = new int[TARGET+1];
        int lastNumber = 0;

        for (int i = 1; i < input.Length; i++)
        {
            numbers[input[i-1]] = (int) i;
        }

        lastNumber = (int) input[input.Length - 1];

        for (int i = input.Length+1; i < TARGET+1; i++)
        {
            if (numbers[lastNumber].Equals(0))
            {
                numbers[lastNumber] = (int)i - 1;
                lastNumber = 0;
            }

            else
            {
                int difference = (int)i - 1 - numbers[lastNumber];
                numbers[lastNumber] = (int)i - 1;
                lastNumber = difference;
            }
        }

        return lastNumber;
    }

    // Start is called before the first frame update
    void Start()
    {
        Main();
    }
}
