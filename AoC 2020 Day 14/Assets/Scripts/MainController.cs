using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class MainController : MonoBehaviour
{
    private static Regex mask_regex = new Regex("[10X]*$");
    private static Regex mem_regex = new Regex("\\d+");

    private void Main()
    {
        string[] input = System.IO.File.ReadAllLines("./Assets/Input/day14input.txt");

        Part1(input);

        Part2(input);
    }

    private void Part1(string[] input)
    {
        MaskSum masksum = new MaskSum();
    
        foreach(string line in input)
        {
            if (line[1].Equals('a')) // mask
            {
                Match m = mask_regex.Match(line);
                masksum.SetMask(m.Value);
            }
            else // mem address
            {
                MatchCollection matches = mem_regex.Matches(line);
                masksum.WriteMemory(int.Parse(matches[0].Value), long.Parse(matches[1].Value));
            }
        }

        Debug.Log("Sum in Memory: " + masksum.GetSum());
    }

    private void Part2(string[] input)
    {
        MaskAddress maskaddress = new MaskAddress();

        foreach(string line in input)
        {
            if (line[1].Equals('a')) // mask
            {
                Match m = mask_regex.Match(line);
                maskaddress.SetMask(m.Value);
            }
            else // mem address
            {
                MatchCollection matches = mem_regex.Matches(line);
                maskaddress.WriteMemory(long.Parse(matches[0].Value), long.Parse(matches[1].Value));
            }
        }

        Debug.Log("Sum in Memory: " + maskaddress.GetSum());
    }

    // Start is called before the first frame update
    void Start()
    {
        Main();   
    }
}
