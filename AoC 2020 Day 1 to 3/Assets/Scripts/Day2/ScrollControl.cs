using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScrollControl : MonoBehaviour
{
    public GameObject textLine;
    public GameObject parentPanel;
    public TextMeshProUGUI validPasswordsNum;

    private string[] input;
    private int readInputIndex = 0;
    private GameObject[] textLines = new GameObject[10];
    private int textLinesIndex = 0;
    private int validNum = 0;

    public bool finishedScrollingInput = false;
    public bool finishedLoadingInput = false;

    private int iterationsBeforeSpeedup = 12;
    private int iterations = 0;
    private int speed = 0;

    public void LoadInput()
    {
        input = System.IO.File.ReadAllLines("./Assets/Input/Day2input.txt");
        Debug.Log("Input Loaded to Scroll Area");
        finishedLoadingInput = true;
    }

    public void ClearScrollArea()
    {
        foreach(GameObject obj in textLines)
        {
            Destroy(obj);
        }
        readInputIndex = 0;
        textLinesIndex = 0;
        validNum = 0;
        finishedLoadingInput = false;
        finishedScrollingInput = false;
        iterations = 0;
        speed = 0;
    }

    public IEnumerator NextLine(int part)
    {
        Destroy(textLines[textLinesIndex]); // rather than destroy, we can have some animation that fades and then destroys after some seconds.
        textLines[textLinesIndex] = Instantiate(textLine, parentPanel.transform, false);
        textLines[textLinesIndex].GetComponent<TextControl>().Init(input[readInputIndex]);

        if (part.Equals(1))
        {
            yield return textLines[textLinesIndex].GetComponent<TextControl>().CheckPassword(speed);
        }
        if (part.Equals(2))
        {
            yield return textLines[textLinesIndex].GetComponent<TextControl>().CheckPasswordPart2(speed);
        }
        
        while (!textLines[textLinesIndex].GetComponent<TextControl>().finishedParsing)
        {
            yield return new WaitForSeconds(0.1f);
        }

        if (textLines[textLinesIndex].GetComponent<TextControl>().isValid)
        {
            validNum++;
            validPasswordsNum.text = validNum.ToString();
        }

        textLinesIndex = (textLinesIndex+1) % 10;
        readInputIndex++;

        if (readInputIndex.Equals(input.Length))
        {
            finishedScrollingInput = true;
        }

        iterations++;
        if(iterations < iterationsBeforeSpeedup)
        {
            yield return new WaitForSeconds(0.1f);
        }
        else if (iterations < iterationsBeforeSpeedup*5)
        {
            yield return new WaitForSeconds(0.04f);
            speed = 1;
        }
        else
        {
            yield return new WaitForSeconds(0.0001f);
        }
    }
}
