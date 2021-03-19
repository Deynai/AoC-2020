using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Diagnostics;

public class MainController : MonoBehaviour
{
    private Regex num = new Regex("[\\d]+");
    private Regex op = new Regex("[\\+\\*]");
    private Regex brackets = new Regex("\\([\\d\\+\\* ]*\\)");
    private Regex addition = new Regex("[\\d]+ ?\\+ ?[\\d]+");

    private void Main()
    {
        string[] input = System.IO.File.ReadAllLines("./Assets/Input/day18input.txt");

        Stopwatch sw = new Stopwatch();
        sw.Start();
        Part1(input);

        Part2(input);
        sw.Stop();
        UnityEngine.Debug.Log(sw.ElapsedMilliseconds + "ms");
    }

    private void Part1(string[] input)
    {
        long sum = 0;
        foreach(string line in input)
        {
            sum += ParseLine(line);
            if(sum < 0)
            {
                UnityEngine.Debug.Log("Error: long overflow");
                break;
            }
        }

        UnityEngine.Debug.Log(sum);
    }

    private void Part2(string[] input)
    {
        long sum = 0;
        foreach (string line in input)
        {
            sum += ParseLine2(line);
            if (sum < 0)
            {
                UnityEngine.Debug.Log("Error: long overflow");
                break;
            }
        }

        UnityEngine.Debug.Log(sum);
    }

    private long ParseLine(string line)
    {
        string bracketLine = "(" + line + ")";
        Match m = brackets.Match(bracketLine);

        MatchEvaluator myEval = new MatchEvaluator(this.ParseBracket);

        int breakProtection = 0;

        while (m.Success)
        {
            bracketLine = brackets.Replace(bracketLine, myEval);
            m = brackets.Match(bracketLine);

            breakProtection++;
            if(breakProtection >= 100)
            {
                UnityEngine.Debug.Log("Error: hit break protection");
                break;
            }
        }

        return long.Parse(bracketLine);
    }

    private long ParseLine2(string line)
    {
        string bracketLine = "(" + line + ")";
        Match m = brackets.Match(bracketLine);

        MatchEvaluator myEval = new MatchEvaluator(this.ParseBracket2);

        int breakProtection = 0;

        while (m.Success)
        {
            bracketLine = brackets.Replace(bracketLine, myEval);
            m = brackets.Match(bracketLine);

            breakProtection++;
            if (breakProtection >= 200)
            {
                UnityEngine.Debug.Log("Error: hit break protection");
                break;
            }
        }

        return long.Parse(bracketLine);
    }

    private string ParseBracket(Match bracket)
    {
        MatchCollection nums = num.Matches(bracket.Value);
        MatchCollection ops = op.Matches(bracket.Value);

        int index = nums.Count;
        long result = long.Parse(nums[0].Value);

        for(int i = 0; i < index-1; i++)
        {
            result = (ops[i].Value.Equals("*") ? result * long.Parse(nums[i + 1].Value) : result + long.Parse(nums[i + 1].Value));
        }

        return result.ToString();
    }

    private string ParseBracket2(Match bracket)
    {
        string match_string = bracket.Value;

        Match ma = addition.Match(match_string);
        MatchEvaluator additionEval = new MatchEvaluator(this.ParseAddition);
        int bp = 0;

        while (ma.Success)
        {
            match_string = addition.Replace(match_string, additionEval);
            ma = addition.Match(match_string);

            bp++;
            if (bp >= 100)
            {
                UnityEngine.Debug.Log("Error: hit break protection");
                break;
            }
        }

        MatchCollection nums = num.Matches(match_string);
        MatchCollection ops = op.Matches(match_string);
        int index = nums.Count;
        long result = long.Parse(nums[0].Value);

        for (int i = 0; i < index - 1; i++)
        {
            result *= long.Parse(nums[i + 1].Value);
        }

        return result.ToString();
    }

    private string ParseAddition(Match m)
    {
        MatchCollection nums = num.Matches(m.Value);

        return (long.Parse(nums[0].Value) + long.Parse(nums[1].Value)).ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        Main();
    }
}
