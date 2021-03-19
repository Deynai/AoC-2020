using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class MainController : MonoBehaviour
{
    private void Main()
    {
        string[] input = System.IO.File.ReadAllLines("./Assets/Input/day4input.txt");

        int i = 0;
        List<string> passports = new List<string>();

        while(i < input.Length)
        {
            string temp = "";

            while (i < input.Length && input[i].Length >= 1) // or !input[i][0].equals('\n') ? (stop on empty line)
            {
                temp += input[i].Trim('\n',' ');
                temp += " ";
                i++;
            }

            passports.Add(temp.Trim());
            i++;
        }

        List<Dictionary<string, string>> ppdict = new List<Dictionary<string, string>>();
        foreach(string pp in passports)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string[] ppsplit = pp.Split(' ');
            foreach(string entry in ppsplit)
            {
                string[] pair = entry.Split(':');
                dict.Add(pair[0],pair[1]);
            }
            ppdict.Add(dict);
        }

        // part 1:
        int part1 = ppdict.FindAll(pp => (pp.Count == 8) || (!pp.ContainsKey("cid") && pp.Count == 7)).Count;

        Debug.Log("Valid passports for part 1: " + part1);

        // part 2:

        Regex hcl_regex = new Regex("^#[0-9a-f]{6}$"); // # followed by exactly 6 of digits 0-9 and letters a-f
        Regex pid_regex = new Regex("^[0-9]{9}$"); // exactly 9 digits of 0-9
        Regex ecl_regex = new Regex("^amb$|^blu$|^brn$|^gry$|^grn$|^hzl$|^oth$"); // exactly a 3 letter string equal to one of 7 options
        Regex hgt_cm_regex = new Regex("^[0-9]{3}cm$"); // exactly 3 digits 0-9 followed by letters "cm"
        Regex hgt_in_regex = new Regex("^[0-9]{2}in$"); // exactly 2 digits 0-9 followed by letters "in"

        int part2 = ppdict.FindAll(pp => (pp.Count == 8) || (!pp.ContainsKey("cid") && pp.Count == 7)) // cid
                            .FindAll(pp => int.Parse(pp["byr"]) >= 1920 && int.Parse(pp["byr"]) <= 2002) // byr
                            .FindAll(pp => int.Parse(pp["iyr"]) >= 2010 && int.Parse(pp["iyr"]) <= 2020) // iyr 
                            .FindAll(pp => int.Parse(pp["eyr"]) >= 2020 && int.Parse(pp["eyr"]) <= 2030) // eyr
                            .FindAll(pp => hcl_regex.IsMatch(pp["hcl"])) // hcl
                            .FindAll(pp => pid_regex.IsMatch(pp["pid"])) // pid
                            .FindAll(pp => ecl_regex.IsMatch(pp["ecl"])) // ecl
                            .FindAll(pp => ((hgt_cm_regex.IsMatch(pp["hgt"])) && int.Parse(pp["hgt"].Substring(0,pp["hgt"].Length-2)) >= 150 && int.Parse(pp["hgt"].Substring(0, pp["hgt"].Length - 2)) <= 193) ||
                                            ((hgt_in_regex.IsMatch(pp["hgt"])) && int.Parse(pp["hgt"].Substring(0, pp["hgt"].Length - 2)) >= 59 && int.Parse(pp["hgt"].Substring(0, pp["hgt"].Length - 2)) <= 76)) // hgt
                            .Count;

        Debug.Log("Valid passports for part 2: " + part2);
    }

    // Start is called before the first frame update
    void Start()
    {
        Main();
    }
}
