using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;
using System;
using System.Diagnostics;

public class MainController : MonoBehaviour
{
    private Regex reg_rules = new Regex("^[\\d]*:");
    private Regex reg_data = new Regex("^[ab]+");
    private Regex reg_first_digit = new Regex("[\\d]+");
    private Regex reg_first_letters = new Regex("^[ab]+[ab ]*");

    private class Rule
    {
        public int id;
        public List<string> str_options = new List<string>();

        public Rule(string r)
        {
            id = int.Parse(r.Substring(0, r.IndexOf(':')));
            string subr = r.Substring(r.IndexOf(':') + 1);
            
            if(subr.Contains("a") || subr.Contains("b"))
            {
                str_options.Add(subr[subr.IndexOf("\"") + 1].ToString());
            }
            else
            {
                str_options = subr.Split('|').Select(a => a.Trim()).ToList();
            }
        }
    }

    private void Main()
    {
        string[] rules_input = System.IO.File.ReadLines("./Assets/Input/day19input.txt").Where(a => reg_rules.Match(a).Success).ToArray();
        string[] data = System.IO.File.ReadLines("./Assets/Input/day19input.txt").Where(a => reg_data.Match(a).Success).Select(a=>a).ToArray();

        Rule[] rules = rules_input.Select(a => new Rule(a)).ToArray();
        Array.Sort(rules, delegate (Rule x, Rule y) { return x.id.CompareTo(y.id); });

        //Part1(rules, data);

        rules[8] = new Rule("8: 42 | 42 8");
        rules[11] = new Rule("11: 42 31 | 42 11 31");

        Stopwatch sw = new Stopwatch();
        sw.Start();
        Part2(rules, data);
        sw.Stop();
        UnityEngine.Debug.Log(sw.ElapsedMilliseconds + "ms");
    }

    private void Part1(Rule[] rules, string[] data)
    {
        List<string> valid_strings = new List<string>();
        int count = 0;

        foreach(string line in data)
        {
            if(RecurseRule(valid_strings, rules, "0", data, line))
            {
                count++;
            }
        }

        UnityEngine.Debug.Log("Part 1: " + count);
    }

    private void Part2(Rule[] rules, string[] data)
    {
        List<string> valid_strings = new List<string>();
        int count = 0;

        foreach (string line in data)
        {
            if (RecurseRule(valid_strings, rules, "0", data, line))
            {
                count++;
            }
        }

        UnityEngine.Debug.Log("Part 2: " + count);
    }

    private bool RecurseRule(List<string> valid_strings, Rule[] rules, string curr, string[] data, string match_requirement)
    {
        Match firstdigit = reg_first_digit.Match(curr);
        Match firstletters = reg_first_letters.Match(curr);

        if (firstletters.Success)
        {
            Regex firstlettersMatch = new Regex("^" + firstletters.Value.Replace(" ", string.Empty));

            if (!firstlettersMatch.Match(match_requirement).Success)
            {
                return false;
            }
        }

        if (!firstdigit.Success)
        {
            if (match_requirement.Equals(curr.Replace(" ", string.Empty)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            Rule rule = rules[int.Parse(firstdigit.Value)];
            foreach(string opt in rule.str_options)
            {
                string currcopy = curr;
                currcopy = reg_first_digit.Replace(currcopy, opt, 1);
                if(RecurseRule(valid_strings, rules, currcopy, data, match_requirement))
                {
                    return true;
                }
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
