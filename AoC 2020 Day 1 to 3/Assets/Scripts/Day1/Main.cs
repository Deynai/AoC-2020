using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public GameObject bar;
    public GameObject parentPanel;

    public TextMeshProUGUI array1;
    public TextMeshProUGUI array2;

    public TextMeshProUGUI[] sumText;
    public TextMeshProUGUI[] productText;
    public TextMeshProUGUI[] part2Text;

    private Color grey = new Color(0.15f, 0.15f, 0.15f, 1.0f);
    private Color blue = new Color(0.0f, 0.667f, 1.0f, 1.0f);
    private Color purple = new Color(0.81f, 0.0f, 1.0f, 1.0f);
    private Color green = Color.green;

    private bool part2finished = false;

    // Standard top-down Merge Sort for an integer array A.
    private IEnumerator MergeSort(int[] A, GameObject[] A_ob, GameObject[] B_ob)
    {
        int L = A.Length;
        int[] B = new int[L];

        int target = 2020;

        for (int i = 0; i < L; i++)
        {
            B[i] = A[i];
            // copy each A_ob into the second array.
            Destroy(B_ob[i]);
            B_ob[i] = Instantiate(bar, parentPanel.transform, false);
            LineControl.Copy(B_ob[i].GetComponent<LineControl>(), A_ob[i].GetComponent<LineControl>());
            B_ob[i].GetComponent<LineControl>().MoveToLocation(i, (A_ob[i].GetComponent<LineControl>().array + 1) % 2);
            yield return new WaitForSeconds(0.01f);
        }

        while (!B_ob[B_ob.Length-1].GetComponent<LineControl>().finishedMoving)
        {
            yield return new WaitForSeconds(0.001f);
        }

        yield return StartCoroutine(SplitSubArray(A, 0, L, B, A_ob, B_ob));

        yield return new WaitForSeconds(3.0f);

        foreach(GameObject obj in A_ob)
        {
            Destroy(obj);
        }

        array1.alpha = 0;

        yield return StartCoroutine(Staircase(A, B_ob, target, 0.1f, -1, 1));

        yield return StartCoroutine(Staircase2(A, B_ob, target));
        /*
        for (int i = 0; i < L; i++)
        {
            A[i] = B[i];
            Destroy(A_ob[i]);
            A_ob[i] = Instantiate(bar, parentPanel.transform, false);
            LineControl.Copy(A_ob[i].GetComponent<LineControl>(), B_ob[i].GetComponent<LineControl>());
            A_ob[i].GetComponent<LineControl>().MoveToLocation(i, (B_ob[i].GetComponent<LineControl>().array + 1) % 2);
        }
        */
    }

    private GameObject DuplicateLine(GameObject obj)
    {
        GameObject line = Instantiate(bar, parentPanel.transform, false);
        LineControl.Copy(line.GetComponent<LineControl>(), obj.GetComponent<LineControl>());
        return line;
    }

    private IEnumerator Staircase2(int[] A, GameObject[] B_ob, int target)
    {
        GameObject selectedLine;
        foreach(TextMeshProUGUI tmp in part2Text)
        {
            tmp.alpha = 1;
        }

        // select i
        for (int i = 0; i < B_ob.Length; i++)
        {
            selectedLine = DuplicateLine(B_ob[i]);
            B_ob[i].GetComponent<LineControl>().SetColor(grey);
            selectedLine.GetComponent<LineControl>().SetColor(green);
            selectedLine.GetComponent<LineControl>().MoveToLocation(51, 0);

            int num = selectedLine.GetComponent<LineControl>().v;
            part2Text[1].text = num.ToString();
            part2Text[0].text = (target - num).ToString();

            yield return Staircase(A, B_ob, target - num, 0.05f, i, num);

            if (part2finished)
            {
                yield break;
            }
            
            Destroy(selectedLine);
            B_ob[i].GetComponent<LineControl>().SetColor(Color.white);
            

            yield return new WaitForSeconds(0.1f);
        }

        // move i to left side, array 0

        // show text with the number, and Target = 2020 - number

        // run Staircase, loop
    }

    private IEnumerator Staircase(int[] A, GameObject[] B_ob, int target, float speed, int skipIndex, int product)
    {
        int i = 0; int j = B_ob.Length-1;
        if (skipIndex.Equals(i))
        {
            i++;
        }
        if (skipIndex.Equals(j))
        {
            j--;
        }
        bool finished = false;
        bool success = false;

        // select i, select j
        GameObject line1 = DuplicateLine(B_ob[i]);
        B_ob[i].GetComponent<LineControl>().SetColor(grey);
        line1.GetComponent<LineControl>().SetColor(purple);

        GameObject line2 = DuplicateLine(B_ob[j]);
        B_ob[j].GetComponent<LineControl>().SetColor(grey);
        line2.GetComponent<LineControl>().SetColor(blue);

        // move i and j to the middle
        line1.GetComponent<LineControl>().MoveToLocation(85, 0);
        line2.GetComponent<LineControl>().MoveToLocation(112, 0);

        // set text to equal i and j's value
        int num1 = line1.GetComponent<LineControl>().v;
        int num2 = line2.GetComponent<LineControl>().v;

        sumText[0].text = num1.ToString();
        sumText[1].text = num2.ToString();
        sumText[4].text = (num1 + num2).ToString();

        foreach(TextMeshProUGUI tmp in sumText)
        {
            tmp.alpha = 1;
        }

        while (!finished)
        {
            yield return new WaitForSeconds(speed);

            // red if not target, green if target
            if ((num1 + num2).Equals(target)){
                sumText[4].color = Color.green;
                sumText[4].text = (num1 + num2).ToString();
                finished = true;
                success = true;
            }
            else if(Mathf.Abs(j - i).Equals(1))
            {
                finished = true;
                break;
            }
            else
            {
                if ((num1 + num2) < target)
                {
                    i++;
                    if (skipIndex.Equals(i))
                    {
                        i++;
                    }
                    Destroy(line1);
                    line1 = DuplicateLine(B_ob[i]);
                    B_ob[i].GetComponent<LineControl>().SetColor(grey);
                    line1.GetComponent<LineControl>().SetColor(purple);
                    line1.GetComponent<LineControl>().MoveToLocation(85, 0);
                    num1 = line1.GetComponent<LineControl>().v;
                    sumText[0].text = num1.ToString();
                }
                else
                {
                    j--;
                    if (skipIndex.Equals(j))
                    {
                        j--;
                    }
                    Destroy(line2);
                    line2 = DuplicateLine(B_ob[j]);
                    B_ob[j].GetComponent<LineControl>().SetColor(grey);
                    line2.GetComponent<LineControl>().SetColor(blue);
                    line2.GetComponent<LineControl>().MoveToLocation(112, 0);
                    num2 = line2.GetComponent<LineControl>().v;
                    sumText[1].text = num2.ToString();
                }
            }

            sumText[4].text = (num1 + num2).ToString();
        }

        if (success)
        {
            productText[0].text = num1.ToString();
            productText[1].text = num2.ToString();
            productText[4].text = (num1 * num2 * product).ToString();
            foreach (TextMeshProUGUI tmp in productText)
            {
                tmp.alpha = 1;
            }
            if (product.Equals(1))
            {
                productText[5].alpha = 0;
            }
            else
            {
                part2finished = true;
                yield break;
            }

            yield return new WaitForSeconds(5.0f);
        }

        // Clean-up
        Destroy(line1);
        Destroy(line2);
        foreach (TextMeshProUGUI tmp in productText)
        {
            tmp.alpha = 0;
        }
        foreach (TextMeshProUGUI tmp in sumText)
        {
            tmp.alpha = 0;
        }
        foreach(GameObject obj in B_ob)
        {
            obj.GetComponent<LineControl>().SetColor(Color.white);
        }

        yield break;
    }

    private IEnumerator SplitSubArray(int[] A, int start, int end, int[] B, GameObject[] A_ob, GameObject[] B_ob)
    {
        if ((end - start) <= 1)
        {
            yield break;
        }
        else
        {
            int middle = (end + start) / 2;
            yield return StartCoroutine(SplitSubArray(B, start, middle, A, B_ob, A_ob));
            yield return StartCoroutine(SplitSubArray(B, middle, end, A, B_ob, A_ob));

            yield return StartCoroutine(MergeArray(A, start, middle, end, B, A_ob, B_ob));
        }
    }

    private IEnumerator MergeArray(int[] A, int start, int middle, int end, int[] B, GameObject[] A_ob, GameObject[] B_ob)
    {
        int i = start;
        int j = middle;

        for (int k = start; k < end; k++)
        {
            if (i < middle && (j >= end || A[i] <= A[j]))
            {
                B[k] = A[i];

                Destroy(B_ob[k]);
                B_ob[k] = Instantiate(bar, parentPanel.transform, false);
                LineControl.Copy(B_ob[k].GetComponent<LineControl>(), A_ob[i].GetComponent<LineControl>());
                B_ob[k].GetComponent<LineControl>().MoveToLocation(k, (A_ob[i].GetComponent<LineControl>().array + 1) % 2);

                //B_ob[k].GetComponent<LineControl>().Instantiate(k, (A_ob[i].GetComponent<LineControl>().array + 1) % 2, A_ob[i].GetComponent<LineControl>().v);
                //while (!B_ob[k].GetComponent<LineControl>().finishedMoving)
                
                yield return new WaitForSeconds(0.001f);
                
                i++;
            }
            else
            {
                B[k] = A[j];
                Destroy(B_ob[k]);
                B_ob[k] = Instantiate(bar, parentPanel.transform, false);
                LineControl.Copy(B_ob[k].GetComponent<LineControl>(), A_ob[j].GetComponent<LineControl>());
                B_ob[k].GetComponent<LineControl>().MoveToLocation(k, (A_ob[j].GetComponent<LineControl>().array + 1) % 2);

                //B_ob[k].GetComponent<LineControl>().Instantiate(k, (A_ob[j].GetComponent<LineControl>().array + 1) % 2, A_ob[j].GetComponent<LineControl>().v);
                yield return new WaitForSeconds(0.001f);
                
                j++;
            }

            while (!B_ob[end-1].GetComponent<LineControl>().finishedMoving)
            {
                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // load in the data to an array
        int[] input = System.IO.File.ReadAllLines("./Assets/Input/Day1input.txt").Select(x => int.Parse(x)).ToArray();

        /*
        int[] input1 = new int[200];
        for (int i = 0; i < 200; i++)
        {
            input1[i] = i + 1;
        }

        System.Random rnd = new System.Random();
        int[] input = input1.OrderBy(x => rnd.Next()).ToArray();
        */

        GameObject[] bars = new GameObject[input.Length];
        GameObject[] bars2 = new GameObject[input.Length];

        foreach (TextMeshProUGUI tmp in sumText)
        {
            tmp.alpha = 0;
        }
        foreach (TextMeshProUGUI tmp in productText)
        {
            tmp.alpha = 0;
        }

        // go through the array and create a new prefab line for each value, with the appropriate values and position

        for (int i = 0; i < input.Length; i++)
        {
            bars[i] = Instantiate(bar, parentPanel.transform, false);
            bars[i].GetComponent<LineControl>().Instantiate(i, 0, input[i]);
        }

        StartCoroutine(MergeSort(input, bars, bars2));

        // Now we want to delete all of A, and then "staircase" to answer part 1.
        

        /*
        for (int i = 0; i < input.Length; i++)
        {
            bars2[i] = Instantiate(bar, parentPanel.transform, false);
            bars2[i].GetComponent<LineControl>().Instantiate(GetX(i), GetY(1), input[i]);
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
    }
}
