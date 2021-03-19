using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;
using System;
using System.Diagnostics;

public class MainController : MonoBehaviour
{
    private void Main()
    {
        Stopwatch sw = new Stopwatch();

        sw.Start();

        Dictionary<string, List<List<string>>> allergen_table = new Dictionary<string, List<List<string>>>();
        List<Food> food_table = new List<Food>();
        List<string> all_non_allergy_ingredients = new List<string>();

        InitialiseLists(food_table, allergen_table);

        Part1(food_table, allergen_table, all_non_allergy_ingredients);

        RemoveAllNonAllergy(food_table, allergen_table, all_non_allergy_ingredients);

        List<List<string>>  allergen_ingredients = Part2(food_table, allergen_table, all_non_allergy_ingredients);

        UnityEngine.Debug.Log(allergen_ingredients[0][0] + "," + allergen_ingredients[1][0] + "," + allergen_ingredients[2][0] + "," + allergen_ingredients[3][0] + "," + allergen_ingredients[4][0] + "," + allergen_ingredients[5][0] + "," + allergen_ingredients[6][0] + "," + allergen_ingredients[7][0]);

        sw.Stop();

        UnityEngine.Debug.Log(sw.ElapsedMilliseconds + "ms");
    }

    private void Part1(List<Food> food_table, Dictionary<string, List<List<string>>> allergen_table, List<string> all_non_allergy_ingredients)
    {
        List<string> all_per_allergy_ingredients = new List<string>();

        foreach(string k in allergen_table.Keys)
        {
            List<string> intersection_allergy_ingredients = allergen_table[k][0];

            for(int i = 1; i < allergen_table[k].Count; i++)
            {
                intersection_allergy_ingredients = FindIntersection(intersection_allergy_ingredients, allergen_table[k][i]);
            }

            foreach(string ingredient in intersection_allergy_ingredients)
            {
                if (!all_per_allergy_ingredients.Contains(ingredient))
                {
                    all_per_allergy_ingredients.Add(ingredient);
                }
            }
        }

        foreach(Food food in food_table)
        {
            List<string> no_possible_allergy_food = RemoveListFromList(food.ingredients, all_per_allergy_ingredients);

            foreach(string ingredient in no_possible_allergy_food)
            {
                all_non_allergy_ingredients.Add(ingredient);
            }
        }

        UnityEngine.Debug.Log("Number of Ingredients that cannot have allergies: " + all_non_allergy_ingredients.Count);
    }

    private List<List<string>> Part2(List<Food> food_table, Dictionary<string, List<List<string>>> allergen_table, List<string> all_non_allergy_ingredients)
    {
        List<List<string>> allergen_ingredient = new List<List<string>>();

        List<string> allergen_keys = new List<string>();

        foreach (string k in allergen_table.Keys)
        {
            allergen_keys.Add(k);
        }

        allergen_keys.Sort();

        foreach (string allergen in allergen_keys)
        {
            //UnityEngine.Debug.Log(allergen);

            List<string> allergen_intersection = allergen_table[allergen][0];
            foreach(List<string> list in allergen_table[allergen])
            {
                allergen_intersection = FindIntersection(allergen_intersection, list);
            }

            allergen_ingredient.Add(allergen_intersection);
        }

        int max = 0;
        int max_index = 0;
        for (int i = 0; i < allergen_ingredient.Count; i++)
        {
            if (allergen_ingredient[i].Count > max)
            {
                max_index = i;
                max = allergen_ingredient[i].Count;
            }
        }

        int bp = 0;
        while(allergen_ingredient[max_index].Count > 1)
        {
            for(int i = 0; i < allergen_ingredient.Count; i++)
            {
                if (allergen_ingredient[i].Count.Equals(1))
                {
                    for(int j = 0; j < allergen_ingredient.Count; j++)
                    {
                        if (i.Equals(j)) { continue; }
                        else
                        {
                            allergen_ingredient[j] = RemoveListFromList(allergen_ingredient[j], allergen_ingredient[i]);
                        }
                    }
                }
            }
            bp++;
            if(bp > 1000)
            {
                break;
            }
        }

        return allergen_ingredient;
    }
    
    private void RemoveAllNonAllergy(List<Food> food_table, Dictionary<string, List<List<string>>> allergen_table, List<string> all_non_allergy_ingredients)
    {
        foreach(Food food in food_table)
        {
            food.ingredients = RemoveListFromList(food.ingredients, all_non_allergy_ingredients);
        }
        foreach(string allergen in allergen_table.Keys)
        {
            for(int i = 0; i < allergen_table[allergen].Count; i++)
            {
                allergen_table[allergen][i] = RemoveListFromList(allergen_table[allergen][i], all_non_allergy_ingredients);
            }
        }
    }

    private List<string> FindIntersection(List<string> list1, List<string> list2)
    {
        List<string> newList = new List<string>();

        foreach(string item in list1)
        {
            if (list2.Contains(item))
            {
                newList.Add(item);
            }
        }

        return newList;
    }

    private List<string> InvFindIntersection(List<string> list1, List<string> list2)
    {
        List<string> newList = new List<string>();

        foreach (string item in list1)
        {
            if (!list2.Contains(item))
            {
                newList.Add(item);
            }
        }
        foreach(string item in list2)
        {
            if (!list1.Contains(item))
            {
                newList.Add(item);
            }
        }

        return newList;
    }

    private List<string> RemoveListFromList(List<string> list, List<string> removal)
    {
        List<string> newList = new List<string>();

        foreach (string item in list)
        {
            if (!removal.Contains(item))
            {
                newList.Add(item);
            }
        }

        return newList;
    }

    private void InitialiseLists(List<Food> food_table, Dictionary<string, List<List<string>>> allergen_table)
    {
        Regex reg_word = new Regex("\\w+");

        string[] input = System.IO.File.ReadAllLines("./Assets/Input/day21input.txt");

        int c = 0;
        foreach(string line in input)
        {
            
            MatchCollection m = reg_word.Matches(line);
            List<string> matches = new List<string>();
            List<string> allergens = new List<string>();

            int final_index = 0;
            for (int i = 0; i < m.Count; i++)
            {
                if (m[i].Value.Equals("contains"))
                {
                    final_index = i;
                    break;
                }

                matches.Add(m[i].Value);
            }

            for (int i = final_index+1; i < m.Count; i++)
            {
                allergens.Add(m[i].Value);

                if (!allergen_table.ContainsKey(m[i].Value))
                {
                    allergen_table.Add(m[i].Value, new List<List<string>>());
                }

                allergen_table[m[i].Value].Add(matches);
            }

            Food newFood = new Food(c, matches, allergens);
            food_table.Add(newFood);

            c++;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Main();
    }
}
