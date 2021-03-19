using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food2
{
    public HashSet<string> ingredients = new HashSet<string>();
    public List<string> allergens = new List<string>();

    public Food2(HashSet<string> ingred, List<string> allerg)
    {
        ingredients = new HashSet<string>(ingred);
        allergens = new List<string>(allerg);
    }
}
