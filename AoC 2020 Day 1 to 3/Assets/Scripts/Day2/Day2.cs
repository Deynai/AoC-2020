using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Day2 : MonoBehaviour
{
    public GameObject scrollArea;

    private bool finishedPart1 = false;

    private IEnumerator part1()
    {
        scrollArea.GetComponent<ScrollControl>().LoadInput();

        while (!scrollArea.GetComponent<ScrollControl>().finishedLoadingInput)
        {
            yield return new WaitForSeconds(0.1f);
        }

        while (!scrollArea.GetComponent<ScrollControl>().finishedScrollingInput)
        {
            yield return scrollArea.GetComponent<ScrollControl>().NextLine(1);
        }

        finishedPart1 = true;
    }

    private IEnumerator part2()
    {
        while (!finishedPart1)
        {
            yield return new WaitForSeconds(3.0f);
        }
        yield return new WaitForSeconds(7.0f);

        scrollArea.GetComponent<ScrollControl>().ClearScrollArea();

        scrollArea.GetComponent<ScrollControl>().LoadInput();

        while (!scrollArea.GetComponent<ScrollControl>().finishedLoadingInput)
        {
            yield return new WaitForSeconds(0.1f);
        }

        while (!scrollArea.GetComponent<ScrollControl>().finishedScrollingInput)
        {
            yield return scrollArea.GetComponent<ScrollControl>().NextLine(2);
        }
    }

    // Start is called before the first frame update

    void Start()
    {
        StartCoroutine("part1");

        StartCoroutine("part2");
        /*
        string password = str.Split(':')[1].Trim();
        int lower = int.Parse(str.Split(' ')[0].Split('-')[0]);
        int upper = int.Parse(str.Split(' ')[0].Split('-')[1]);
        char letter = str.Split(' ')[1].ToCharArray()[0];
        */
    }
}
