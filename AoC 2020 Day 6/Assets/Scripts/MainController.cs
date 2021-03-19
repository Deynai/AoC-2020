using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;

public class MainController : MonoBehaviour
{
    private void Main()
    {
        Part1();
        Part2();
    }

    private void Part1()
    {
        // Split the file by double \r\n, giving a single string for each group.
        string[] input = Regex.Replace(System.IO.File.ReadAllText("./Assets/Input/day6input.txt"), "(.{1})\r\n", "$1", 0, new System.TimeSpan(0, 0, 5)).Split(new string[] { "\r\n" }, System.StringSplitOptions.None);

        int total_sum = 0;
        foreach (string str in input)
        {
            // Flag for each character
            int[] found_letters = new int[26] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            foreach (char ch in str)
            {
                found_letters[ch - 'a'] = 1;
            }

            int count = found_letters.Sum();
            total_sum += count;
        }

        Debug.Log("Part 1: " + total_sum);
    }

    private void Part2()
    {
        // Split the file by line, keeping each line separate instead of merging groups.
        string[] input2 = System.IO.File.ReadAllText("./Assets/Input/day6input.txt").Split(new string[] { "\r\n" }, System.StringSplitOptions.None);

        int total_sum2 = 0;
        for (int i = 0; i < input2.Length; i++)
        {
            // Start with the first line in each group, and for each char in it, remove it if we can't find that char in the subsequent lines
            List<char> baseChars = new List<char>(input2[i].ToCharArray());
            i++;

            while (i < input2.Length && input2[i].Length >= 1)
            {
                baseChars.RemoveAll(c => !new List<char>(input2[i].ToCharArray()).Contains(c));
                i++;
            }

            total_sum2 += baseChars.Count;
        }

        Debug.Log("Part 2: " + total_sum2);
    }

    void Start()
    {
        Main();
    }
}
