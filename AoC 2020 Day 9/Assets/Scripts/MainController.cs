using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainController : MonoBehaviour
{
    private static int HISTORY_LENGTH = 25;
    //private static int PREAMBLE_LENGTH = 25;

    private void Main()
    {
        string[] string_input = System.IO.File.ReadAllLines("./Assets/Input/day9input.txt");
        long[] input = new long[string_input.Length];
        for(int i = 0; i < string_input.Length; i++)
        {
            input[i] = long.Parse(string_input[i]);
        }

        long part2target = Part1(input);

        Debug.Log("Encryption Weakness: " + Part2(input, part2target));
    }

    private long Part1(long[] input)
    {
        long[] history = new long[HISTORY_LENGTH];
        Array.Copy(input, history, HISTORY_LENGTH);

        for(int i = HISTORY_LENGTH; i < input.Length; i++)
        {
            if(!FindSumInHistory(input[i], history))
            {
                Debug.Log("First Invalid Number: " + input[i]);
                return input[i];
            }
            else
            {
                history[(i - HISTORY_LENGTH) % HISTORY_LENGTH] = input[i];
            }
        }

        return 0;
    }

    private long Part2(long[] input, long target)
    {
        int tail = 0;
        int head = 0;

        while (tail <= head && head < input.Length - 1)
        {
            long sum = SumArrayRange(input, tail, head);

            if (sum < target)
            {
                head++;
            }
            else if (sum > target)
            {
                tail++;
            }
            else
            {

                Debug.Log("Tail Index: " + tail + "\nHead Index: " + head);
                return FindContigiousRange(input, tail, head);
            }
        }

        return 0;
    }

    private long FindContigiousRange(long[] input, int tail, int head)
    {
        long[] range = new long[head - tail];
        Array.ConstrainedCopy(input, tail, range, 0, head - tail);
        Array.Sort(range);

        return (range[0] + range[range.Length - 1]);
    }

    private long SumArrayRange(long[] array, int tail, int head)
    {
        long sum = 0;
        for(int i = tail; i < head+1; i++)
        {
            sum += array[i];
        }
        return sum;
    }
   
    private bool FindSumInHistory(long target, long[] history)
    {
        long[] historycopy = new long[HISTORY_LENGTH];
        Array.Copy(history, historycopy, HISTORY_LENGTH);
        Array.Sort(historycopy);

        int i = 0;
        int j = HISTORY_LENGTH - 1;

        while(i < j)
        {
            long sum = historycopy[i] + historycopy[j];
            if(sum < target)
            {
                i++;
            }
            else if(sum > target)
            {
                j--;
            }
            else
            {
                return true;
            }
        }

        return false;
    }



    // Start is called before the first frame update
    void Start()
    {
        Main();
    }
}
