using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food
{
    public int id;
    public List<string> ingredients = new List<string>();
    public List<string> allergens = new List<string>();

    public Food(int i, List<string> ingred, List<string> allerg)
    {
        id = i;
        ingredients = new List<string>(ingred);
        allergens = new List<string>(allerg);
    }
}
