using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BagNode
{
    public string name;
    public Dictionary<BagNode, int> children;
    public List<BagNode> parents;

    public Dictionary<BagNode,int> GetAllChildren()
    {
        return children;
    }
    public List<BagNode> GetAllParents()
    {
        return parents;
    }

    public static List<BagNode> allBags = new List<BagNode>();
    public static BagNode nullbag = new BagNode("nullbag");
    
    public BagNode(string n)
    {
        name = n;
        children = new Dictionary<BagNode, int>();
        parents = new List<BagNode>();
    }

    public void AddChild(int num, string name)
    {
        BagNode child = SearchList(name);

        if (child.Equals(BagNode.nullbag))
        {
            // create new child, add to list
            child = new BagNode(name);
            allBags.Add(child);
        }

        // attach this child to the bagnode, and set its parent
        children.Add(child, num);
        child.AddParent(this);
    }

    public void AddParent(BagNode parent)
    {
        parents.Add(parent);
    }

    // Operations on the list of bags

    public void AddList()
    {
        if (SearchList(name).Equals(BagNode.nullbag))
        {
            //BagNode newBag = new BagNode(name);
            allBags.Add(this);
        }
    }

    public static BagNode SearchList(string n)
    {
        BagNode[] bags = allBags.Where(bag => bag.name.Equals(n)).ToArray();

        return bags.Length.Equals(0) ? BagNode.nullbag : bags[0];
    }

    
}
