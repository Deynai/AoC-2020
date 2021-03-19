using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Seat : MonoBehaviour
{
    public void SetText(string str)
    {
        //GetComponentInChildren<TextMeshProUGUI>().text = str;
        GetComponentInChildren<TextMeshProUGUI>().text = "";
    }

    public float x
    {
        get { return transform.localPosition.x; }
    }

    public float y
    {
        get { return transform.localPosition.y; }
    }

    public void Init(float x, float y)
    {
        transform.localPosition = new Vector2(x, y);
    }

    public void Init(int x, int y)
    {
        transform.localPosition = new Vector2(x, y);
    }
}
