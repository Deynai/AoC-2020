using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextControl : MonoBehaviour
{
    // unity vars
    public GameObject checkmark;
    public TextMeshProUGUI tmptext;
    public TextMeshProUGUI countText;

    // public vars


    // local private vars
    private char[] text;
    private string prePasswordText;
    private string password;
    private int[] colourFlags;
    private char letter;
    private int lower;
    private int upper;
    private static string[] delims = { "-", " ", ": " };

    // colors
    private Color green = Color.green;
    private Color red = Color.red;
    private Color defaultColor = Color.white;
    private Color hidden = new Color(0, 0, 0, 0);

    // flags
    private bool requestUpdate = false;
    public bool finishedParsing = false;
    public bool isValid = false;
    

    public IEnumerator CheckPassword(int speed)
    {
        int letterCount = 0;

        for(int i = 0; i < password.Length; i++)
        {
            if (text[i].Equals(letter))
            {
                letterCount++;
                // set char to green colour
                colourFlags[i] = 1;
                countText.text = letterCount.ToString();
            }
            else
            {
                // set char to red colour
                colourFlags[i] = 2;
            }

            requestUpdate = true;
            if (speed.Equals(0))
            {
                yield return new WaitForSeconds(0.1f);
            }
        }

        if(letterCount >= lower && letterCount <= upper)
        {
            // set checkmark to green
            checkmark.GetComponent<RawImage>().color = green;
            isValid = true;
        }
        else
        {
            // set tick to red
            checkmark.GetComponent<RawImage>().color = red;
        }

        finishedParsing = true;

        yield break;
    }

    public IEnumerator CheckPasswordPart2(int speed)
    {
        int letterCount = 0;

        if (text[lower-1].Equals(letter))
        {
            letterCount++;
            colourFlags[lower-1] = 1;
            countText.text = letterCount.ToString();
        }
        else
        {
            colourFlags[lower-1] = 2;
        }
        requestUpdate = true;
        if (speed.Equals(0))
        {
            yield return new WaitForSeconds(0.7f);
        }

        if (text[upper-1].Equals(letter))
        {
            letterCount++;
            colourFlags[upper-1] = 1;
            countText.text = letterCount.ToString();
        }
        else
        {
            colourFlags[upper-1] = 2;
        }
        requestUpdate = true;
        if (speed.Equals(0))
        {
            yield return new WaitForSeconds(0.7f);
        }

        if (letterCount.Equals(1))
        {
            checkmark.GetComponent<RawImage>().color = green;
            isValid = true;
        }
        else
        {
            // set tick to red
            checkmark.GetComponent<RawImage>().color = red;
        }

        finishedParsing = true;

        yield break;
    }

    private string StringOutput()
    {
        string output = "";

        for(int i = 0; i < colourFlags.Length; i++)
        {
            if (colourFlags[i].Equals(0))
            {
                output += text[i];
            }
            if (colourFlags[i].Equals(1))
            {
                output += "<color=green>" + text[i] + "</color>";
            }
            if(colourFlags[i].Equals(2))
            {
                output += "<color=red>" + text[i] + "</color>";
            }
        }

        return output;
    }

    public void Init(string str)
    {
        checkmark.GetComponent<RawImage>().color = hidden;
        tmptext.richText = true;
        string[] splitstr = str.Split(delims, System.StringSplitOptions.RemoveEmptyEntries);
        lower = int.Parse(splitstr[0]);
        upper = int.Parse(splitstr[1]);
        letter = char.Parse(splitstr[2]);
        prePasswordText = lower.ToString() + "-" + upper.ToString() + " " + letter + ": ";
        password = splitstr[3];
        text = password.ToCharArray();
        Debug.Log(splitstr[3]);
        colourFlags = new int[password.Length];
        
        for(int i = 0; i < colourFlags.Length; i++)
        {
            colourFlags[i] = 0;
        }

        requestUpdate = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (requestUpdate)
        {
            tmptext.text = prePasswordText + StringOutput();
            requestUpdate = false;
        }
    }
}
