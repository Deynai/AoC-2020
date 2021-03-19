using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MainController : MonoBehaviour
{
    float TIMEOUT = 300f;
    float timedelta = 0f;
    bool timed = false;

    private class Cup
    {
        public int label;
        public Cup next;
        public Cup prev;

        public Cup(int l)
        {
            label = l;
        }
    }

    private void Main()
    {
        List<int> input = "974618352".ToCharArray().Select(p => int.Parse(p.ToString())).ToList();

        Dictionary<int, Cup> cupDict = new Dictionary<int, Cup>();
        InitCups(cupDict, input);

        int number_of_moves = 100;
        Part1(cupDict, input[0], number_of_moves);

        Dictionary<int, Cup> cupDict2 = new Dictionary<int, Cup>();

        List<int> input2 = new List<int>(input);
        for(int i = 10; i < 1000001; i++)
        {
            input2.Add(i);
        }

        InitCups(cupDict2, input2);
        int number_of_moves2 = 10000000;
        Part2(cupDict2, input2[0], number_of_moves2);
    }
    private void Part1(Dictionary<int, Cup> cupDict, int selectedCup, int moves)
    {
        Cup cup = cupDict[selectedCup];

        CurrentOrder(cupDict[1]);

        for (int i = 0; i < moves; i++)
        {
            MakeTurn(cupDict, cup);
            cup = cup.next;
        }

        Debug.Log($"Final Order: {string.Join("", CurrentOrder(cupDict[1]))}");
    }

    private void Part2(Dictionary<int, Cup> cupDict2, int selectedCup, int moves)
    {
        Cup cup = cupDict2[selectedCup];

        for (int i = 0; i < moves; i++)
        {
            if (timed)
            {
                Debug.Log("Timed out");
                break;
            }

            MakeTurn(cupDict2, cup);
            cup = cup.next;
        }

        Debug.Log($"Final Order: {cupDict2[1].next.label} and {cupDict2[1].next.next.label}");
        Debug.Log($"Multiplying: {(long)cupDict2[1].next.label * cupDict2[1].next.next.label}");
    }

    private Cup MakeTurn(Dictionary<int, Cup> cupDict, Cup cup)
    {
        Cup threeCupHead = cup.next;
        Cup threeCupTail = cup.next.next.next;

        // snip out the 3 cups
        cup.next = threeCupTail.next;
        threeCupTail.next.prev = cup;

        // find next suitable selected cup
        int label = cup.label;
        bool foundLabel = false;

        while (!foundLabel)
        {
            label = 1 + (label - 2 + cupDict.Count) % (cupDict.Count);
            if (threeCupHead.label.Equals(label) || threeCupHead.next.label.Equals(label) || threeCupHead.next.next.label.Equals(label))
            {
                continue;
            }
            else
            {
                foundLabel = true;
                cup = cupDict[label];
            }
        }

        // connect set of 3 cups to the right of the new selected cup
        cup.next.prev = threeCupTail;
        threeCupTail.next = cup.next;
        cup.next = threeCupHead;
        threeCupHead.prev = cup;

        return cup;
    }

    private List<int> CurrentOrder(Cup cup)
    {
        List<int> order = new List<int>();
        Cup iterCup = cup.next;

        while (!iterCup.label.Equals(1))
        {
            order.Add(iterCup.label);
            iterCup = iterCup.next;
        }

        return order;
    }

    private void InitCups(Dictionary<int, Cup> cupDict, List<int> input)
    {
        Cup cup = new Cup(input[0]);
        cupDict.Add(cup.label, cup);

        for (int i = 1; i < input.Count; i++)
        {
            Cup nextCup = new Cup(input[i]);
            nextCup.prev = cup;
            cup.next = nextCup;
            cupDict.Add(nextCup.label, nextCup);
            cup = nextCup;
        }

        cupDict[input[0]].prev = cupDict[input[input.Count - 1]];
        cupDict[input[input.Count - 1]].next = cupDict[input[0]];
    }

    void Start()
    {
        Main();
    }

    private void Update()
    {
        timedelta += Time.deltaTime;
        if(timedelta > TIMEOUT)
        {
            timed = true;
        }
    }
}
