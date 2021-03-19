using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineControl : MonoBehaviour
{
    private float x; // x-coordinate
    private float y; // y-coordinate
    public int v; // value
    public int index; 
    public int array; // 0 or 1 for which array it is

    private int tindex;
    public int tarray;
    private float tx; // target x
    private float ty; // target y

    private static float speed = 0.1f;
    private static int maxSize = 2400;

    public bool finishedMoving = true;

    public Color defaultColor = Color.white;

    /*
    public LineControl(float x, float y, int v)
    {
        this.x = x;
        this.y = y;
        this.v = v;
    }
    */

    public void SetColor(Color col)
    {
        defaultColor = col;
        GetComponent<Image>().color = defaultColor;
    }

    private static float GetX(int index)
    {
        return (960.0f / 200) * index + 2.5f;
    }

    private static float GetY(int array_no)
    {
        return array_no.Equals(0) ? 300.0f : 0.0f;
    }

    // move the value bar to a new location
    public void MoveToLocation(int ind, int arr)
    {
        tindex = ind;
        tarray = arr;
        tx = GetX(ind);
        ty = GetY(arr);
        finishedMoving = false;
        CheckFinishedMoving();
        //Debug.Log(".");
    }

    private void CheckFinishedMoving()
    {
        if(tx.Equals(x) && ty.Equals(y))
        {
            finishedMoving = true;
        }
    }

    // change the bar height, this will correspond to the value
    public void Resize(int v)
    {
        // not really needed since we are just moving the bars around, not resizing bars
    }

    public static void Copy(LineControl newLC, LineControl oldLC)
    {
        newLC.x = oldLC.x;
        newLC.y = oldLC.y;
        newLC.v = oldLC.v;
        newLC.index = oldLC.index;
        newLC.array = oldLC.array;
        newLC.tx = oldLC.tx;
        newLC.ty = oldLC.ty;
        newLC.tindex = oldLC.tindex;
        newLC.tarray = oldLC.tarray;
        newLC.DrawValue();
    }

    // draw the sprite based on location and value
    public void DrawValue()
    {
        var pos = GetComponent<Transform>().position;
        var size = GetComponent<RectTransform>();
        size.sizeDelta = new Vector2(3, (300.0f / maxSize) * this.v);
        pos.x = this.x;
        pos.y = this.y;
        GetComponent<Transform>().position = pos;
    }

    public void Instantiate(int index, int array, int value)
    {
        x = GetX(index);
        tx = x;
        this.array = array;
        y = GetY(array);
        ty = y;
        v = value;
        DrawValue();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get parent, copy transform etc, then place with correct x/y

        //DrawValue();
    }

    // Update is called once per frame
    void Update()
    {
        
            if (!tx.Equals(x))
            {
                this.GetComponent<Image>().color = Color.red;
                this.x += (tx - x) * speed;
                if ((tx - x) * (tx - x) <= 4)
                {
                    this.GetComponent<Image>().color = defaultColor;
                    this.x = tx;
                    this.index = tindex;
                    CheckFinishedMoving();
                }
                DrawValue();
            }

            if (!ty.Equals(y))
            {
                this.GetComponent<Image>().color = Color.red;
                this.y += (ty - y) * speed;
                if ((ty - y) * (ty - y) <= 4)
                {
                    this.GetComponent<Image>().color = defaultColor;
                    this.y = ty;
                    this.array = tarray;
                    CheckFinishedMoving();
                }
                DrawValue();
            }
        
    }
}
