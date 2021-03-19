using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Text.RegularExpressions;

public class MainController : MonoBehaviour
{
    private void Main()
    {
        List<int> input = System.IO.File.ReadAllLines("./Assets/Input/day22input.txt")
                          .Where(p => Regex.Match(p, "^[\\d]+").Success)
                          .Select(p => int.Parse(p))
                          .ToList();

        Part1(input);

        Part2(input);
    }

    private void Part1(List<int> input)
    {
        // Doesn't work if the input is odd or imbalanced.
        List<int> p1 = input.GetRange(0, input.Count / 2);
        List<int> p2 = input.GetRange(input.Count / 2, input.Count / 2);

        (int, int) winner = PlayCards(p1, p2);
        Debug.Log($"Player {winner.Item1} wins with a score of {winner.Item2}.");
    }

    private void Part2(List<int> input)
    {
        List<int> p1 = input.GetRange(0, input.Count / 2);
        List<int> p2 = input.GetRange(input.Count / 2, input.Count / 2);

        Hashtable ht = new Hashtable();

        int winner = PlayRecursion(p1, p2, 0);

        Debug.Log($"Winning Player: {winner}");
    }

    private (int, int) PlayCards(List<int> p1, List<int> p2)
    {
        int bp = 10000;
        while(!p1.Count.Equals(0) && !p2.Count.Equals(0))
        {
            if (bp--.Equals(0)) { Debug.Log("While() breakpoint hit."); break; }

            int card1 = p1[0]; int card2 = p2[0];
            p1.RemoveAt(0); p2.RemoveAt(0);

            if(card1 > card2)
            {
                p1.Add(card1);
                p1.Add(card2);
            }
            else
            {
                p2.Add(card2);
                p2.Add(card1);
            }
        }

        return p1.Count.Equals(0) ? (2, Score(p2)) : (1, Score(p1));
    }

    private int PlayRecursion(List<int> p1, List<int> p2, int depth)
    {
        int bp = 10000;
        Hashtable ht = new Hashtable();

        while (!p1.Count.Equals(0) && !p2.Count.Equals(0))
        {
            if (bp--.Equals(0)) { Debug.Log("While() breakpoint hit."); break; }

            int hash = Hash(p1, p2);
            if (ht.ContainsKey(hash))
            {
                //Debug.Log($"Hash duplicate found: {hash}, p1: {string.Join(",", p1)}, p2: {string.Join(",", p2)}");
                return 1;
            }
            else
            {
                ht.Add(hash, 1);
            }
            
            int card1 = p1[0]; int card2 = p2[0];
            p1.RemoveAt(0); p2.RemoveAt(0);

            if (p1.Count >= card1 && p2.Count >= card2)
            {
                List<int> newp1 = p1.GetRange(0, card1);
                List<int> newp2 = p2.GetRange(0, card2);

                int winner = PlayRecursion(newp1, newp2, depth+1);

                if (winner.Equals(1))
                {
                    p1.Add(card1);
                    p1.Add(card2);
                }
                else
                {
                    p2.Add(card2);
                    p2.Add(card1);
                }
            }

            else if (card1 > card2)
            {
                p1.Add(card1);
                p1.Add(card2);
            }
            else
            {
                p2.Add(card2);
                p2.Add(card1);
            }
        }
        if (depth.Equals(0))
        {
            Debug.Log($"Winning players score: {(p1.Count.Equals(0) ? Score(p2) : Score(p1))}");
        }

        return p1.Count.Equals(0) ? 2 : 1;
    }

    private int Hash(List<int> p1, List<int> p2)
    {
        int h = p1.Count;
        for(int i = 0; i < p1.Count; i++)
        {
            h = unchecked(h * 101 + p1[i]);
        }
        for(int i = 0; i < p2.Count; i++)
        {
            h = unchecked(h * 113 + p2[i]);
        }
        return h;
    }

    private int Score(List<int> deck)
    {
        int sum = 0;
        for(int i = 0; i < deck.Count; i++)
        {
            sum += (deck.Count-i) * deck[i];
        }
        return sum;
    }

    void Start()
    {
        Main();
    }
}
