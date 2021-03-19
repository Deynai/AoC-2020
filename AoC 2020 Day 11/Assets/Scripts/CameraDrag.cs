using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    public float dragSpeed = 1.0f;
    private float speedDecay = 0.01f;
    private float dragDecay = 0.05f;
    private Vector3 dragOrigin;
    private Vector3 dragEnd;
    private Vector3 difference = new Vector3(0, 0, 0);

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (Input.GetMouseButtonUp(0))
        {
            dragEnd = Input.mousePosition;
        }
        //if (!Input.GetMouseButton(0)) return;

        if (Input.GetMouseButton(0))
        {
            difference = Input.mousePosition - dragOrigin;
        }
        else
        {
            difference -= difference * speedDecay;
        }

        dragOrigin += (Input.mousePosition - dragOrigin) * dragDecay;

        Vector3 pos = Camera.main.ScreenToViewportPoint(difference);
        Vector3 move = new Vector3(-pos.x * dragSpeed, -pos.y * dragSpeed, 0);

        //GetComponent<Rigidbody2D>().AddForce(move);
        transform.Translate(move, Space.World);
    }
}
                                                                                                                                                                                                                                                                                 