using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;
using System;
using System.Diagnostics;

public class MainRewrite : MonoBehaviour
{
    private void Main()
    {
        Stopwatch sw = new Stopwatch();

        sw.Start();

        var input = Regex.Replace(System.IO.File.ReadAllText("./Assets/Input/day21input.txt"), @"\(|\)|\r", "")
                    .Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(line => line.Split(new string[] { " contains " }, StringSplitOptions.None))
                    .Select(groups =>
                        (
                            Allergens: groups[1].Split(new string[] { ", " }, StringSplitOptions.None),
                            Ingredients: groups[0].Split(new string[] { " " }, StringSplitOptions.None)
                        )
                    );

        var intersection = input
                    .SelectMany(it => it.Allergens.Select(Allergen => (Allergen, Ingredients: it.Ingredients)))
                    .GroupBy(name => name.Allergen)
                    .Select(group => (Allergen: group.Key, Ingredients: group.Select(p => p.Ingredients).Skip(1).Aggregate(new List<string>(group.Select(q => q.Ingredients).First()), (h, e) => h.Intersect(e).ToList())))
                    .OrderBy(possible_poisons => possible_poisons.Ingredients.Count())
                    .ToList();

        var part1 = input
                    .SelectMany(it => it.Ingredients)
                    .Where(it => !intersection.SelectMany(p => p.Ingredients).Contains(it))
                    .Count();

        UnityEngine.Debug.Log(part1);

        int singles = 0;
        while (!singles.Equals(8))
        {
            singles = 0;
            for (int i = 0; i < intersection.Count; i++)
            {
                if (intersection[i].Ingredients.Count.Equals(1))
                {
                    singles++;
                    for (int j = 0; j < intersection.Count; j++)
                    {
                        if (!i.Equals(j))
                        {
                            intersection[j] = (intersection[j].Allergen, intersection[j].Ingredients.Except(intersection[i].Ingredients).ToList());
                        }
                    }
                }
            }
        }

        var part2 = intersection.OrderBy(p => p.Allergen).Select(q => q.Ingredients.First()).ToList();

        UnityEngine.Debug.Log($"Part 2: {string.Join(",", part2)}");

        sw.Stop();

        UnityEngine.Debug.Log(sw.ElapsedMilliseconds + "ms");
    }

    void Start()
    {
        Main();
    }
}