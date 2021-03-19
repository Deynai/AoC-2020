using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class actionBoxController : MonoBehaviour
{
    public GameObject actionBox;
    public GameObject[] boxes;

    public void Init(string[] input)
    {
        boxes = new GameObject[input.Length];

        for(int i = 0; i < input.Length; i++)
        {
            GameObject newBox = Instantiate(actionBox, transform, false);
            newBox.GetComponent<ButtonController>().Init(input[i], i);
            newBox.GetComponent<ButtonController>().Colour(0);
            boxes[i] = newBox;
        }
    }

    public void Destroy()
    {
        foreach(GameObject box in boxes)
        {
            Destroy(box);
        }
    }

    public void Colour(int index, int val)
    {
        boxes[index].GetComponent<ButtonController>().Colour(val);
    }
}
