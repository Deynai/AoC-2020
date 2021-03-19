using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;

public class MainController : MonoBehaviour
{
    private void Main()
    {
        BagNode shinyGoldBag = new BagNode("shiny-gold");
        shinyGoldBag.AddList();
        ConstructTree();

        Part1(shinyGoldBag);

        Part2(shinyGoldBag);
    }
    private void Part1(BagNode shinyGoldBag)
    {
        List<BagNode> foundParents = new List<BagNode>();
        Debug.Log(CountParents(shinyGoldBag, foundParents));
    }

    private int CountParents(BagNode bag, List<BagNode> foundParents)
    {
        if (bag.parents.Count.Equals(0))
        {
            return 0;
        }

        foreach (BagNode parent in bag.parents)
        {
            if(foundParents.Where(b => b.name.Equals(parent.name)).ToArray().Length.Equals(0)){
                foundParents.Add(parent);
            }
            
            CountParents(parent, foundParents);
        }

        return foundParents.Count;
    }

    private void Part2(BagNode shinyGoldBag)
    {
        Debug.Log(CountChildren(shinyGoldBag));
    }

    private int CountChildren(BagNode bag)
    {
        if (bag.children.Count.Equals(0))
        {
            return 0;
        }

        int sum = 0;
        foreach (BagNode child in bag.children.Keys)
        {
            sum += bag.children[child] + bag.children[child]*CountChildren(child);
        }

        return sum;
    }

    private void ConstructTree()
    {
        string input = System.IO.File.ReadAllText("./Assets/Input/day7input.txt");
        input = input.Replace(" ", "-");
        input = Regex.Replace(input, "(-bags-contain-)", "", 0, new System.TimeSpan(0, 0, 2));
        input = Regex.Replace(input, "-bags-contain-|-bag(s)?(,-)?(\\.)?|no-other-bags\\.", "", 0, new System.TimeSpan(0, 0, 2));
        input = Regex.Replace(input, "(\\d)-", ",$1 ", 0, new System.TimeSpan(0, 0, 2));
        string[] inputlines = input.Split(new string[] { "\r\n" }, 0);
        
        foreach(string line in inputlines)
        {
            string[] linesplit = line.Split(',');
            BagNode bag = BagNode.SearchList(linesplit[0]);
            if (bag.Equals(BagNode.nullbag))
            {
                bag = new BagNode(linesplit[0]);
            }
            bag.AddList();

            int i = 1;
            while (i < linesplit.Length)
            {
                int num = int.Parse(linesplit[i].ElementAt(0).ToString());
                string name = linesplit[i].Substring(2);

                bag.AddChild(num, name);
                i++;
            }
        }

        Debug.Log(BagNode.allBags.Count);
    }

    // Start is called before the first frame update
    void Start()
    {
        Main();
    }
}
