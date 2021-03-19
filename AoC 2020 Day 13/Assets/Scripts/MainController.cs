using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Numerics;

public class MainController : MonoBehaviour
{
    private void Main()
    {
        string[] input = System.IO.File.ReadAllLines("./Assets/Input/day13input.txt");
        string[] buses = input[1].Split(',');
        List<int> time_offsets = new List<int>();
        Dictionary<int, int> schedule = new Dictionary<int, int>();

        for(int i = 0; i < buses.Length; i++)
        {
            if (!buses[i].Equals("x"))
            {
                time_offsets.Add(i);
                schedule.Add(i, int.Parse(buses[i]));
            }
        }

        //Part1(int.Parse(input[0]), buses, time_offsets);

        Part2Big(schedule, time_offsets);
    }

    private void Part1(int time, string[] buses, List<int> time_offsets)
    {
        int minimum = 1000; // some high value
        int bus_of_min = 0;

        foreach(int offset in time_offsets)
        {
            int bus = int.Parse(buses[offset]);
            int time_delay = bus - (time % bus);

            if(time_delay < minimum)
            {
                minimum = time_delay;
                bus_of_min = bus;
            }
        }

        Debug.Log("Part1 - Time Delay * Bus ID: " + (minimum * bus_of_min));
    }

    private void Part2(Dictionary<int,int> schedule, List<int> time_offsets)
    {
        long solution = 0;
        long a = time_offsets[0];
        long am = schedule[time_offsets[0]];

        for(int i = 1; i < time_offsets.Count; i++)
        {
            
            long b = (schedule[time_offsets[i]] - (time_offsets[i] % schedule[time_offsets[i]]));
            long bm = schedule[time_offsets[i]];
            long[] bez = GetBezoutCoefficients(am, bm);

            long mod = (am * bm);
            long term1 = (((bez[1] * bm) % mod) * a) % mod;
            long term2 = (((bez[0] * am) % mod) * b) % mod;
            solution = (am*bm + term1 + term2) % (am*bm);
            a = solution;
            am = am * bm;

            Debug.Log("Solution at step " + i + ": " + solution);
        }
    }

    private void Part2Big(Dictionary<int, int> schedule, List<int> time_offsets)
    {
        BigInteger solution = BigInteger.Zero;
        BigInteger a = time_offsets[0];
        BigInteger am = schedule[time_offsets[0]];

        for (int i = 1; i < time_offsets.Count; i++)
        {

            BigInteger b = (schedule[time_offsets[i]] - (time_offsets[i] % schedule[time_offsets[i]]));
            BigInteger bm = schedule[time_offsets[i]];
            BigInteger[] bez = GetBezoutCoefficientsBig(am, bm);

            BigInteger mod = (am * bm);
            BigInteger term1 = (((bez[1] * bm) % mod) * a) % mod;
            BigInteger term2 = (((bez[0] * am) % mod) * b) % mod;
            solution = (am * bm + term1 + term2) % (am * bm);
            a = solution;
            am = am * bm;

            Debug.Log("Solution at step " + i + ": " + solution);
        }
    }

    private long[] GetBezoutCoefficients(long am, long bm)
    {
        List<long[]> index = new List<long[]>();

        long[] one = { 0, am, 1, 0 };
        long[] two = { 0, bm, 0, 1 };
        index.Add(one);
        index.Add(two);

        while (!index[index.Count - 1][1].Equals(0))
        {
            int i = index.Count - 1;
            long[] newIndex = { 0, 0, 0, 0 };
            newIndex[0] = index[i - 1][1] / index[i][1];
            newIndex[1] = index[i - 1][1] % index[i][1];
            newIndex[2] = index[i - 1][2] - newIndex[0] * index[i][2];
            newIndex[3] = index[i - 1][3] - newIndex[0] * index[i][3];
            index.Add(newIndex);
        }

        long[] bezoutCoefficients = { index[index.Count - 2][2], index[index.Count - 2][3] };

        return bezoutCoefficients;
    }

    private BigInteger[] GetBezoutCoefficientsBig(BigInteger am, BigInteger bm)
    {
        List<BigInteger[]> index = new List<BigInteger[]>();

        BigInteger[] one = { 0, am, 1, 0 };
        BigInteger[] two = { 0, bm, 0, 1 };
        index.Add(one);
        index.Add(two);

        while (!index[index.Count - 1][1].Equals(0))
        {
            int i = index.Count - 1;
            BigInteger[] newIndex = { 0, 0, 0, 0 };
            newIndex[0] = index[i - 1][1] / index[i][1];
            newIndex[1] = index[i - 1][1] % index[i][1];
            newIndex[2] = index[i - 1][2] - newIndex[0] * index[i][2];
            newIndex[3] = index[i - 1][3] - newIndex[0] * index[i][3];
            index.Add(newIndex);
        }

        BigInteger[] bezoutCoefficients = { index[index.Count - 2][2], index[index.Count - 2][3] };

        return bezoutCoefficients;
    }

    // Start is called before the first frame update
    void Start()
    {
        Main();   
    }
}
