using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passenger : MonoBehaviour
{
    public GameObject seatingPanel;
    public GameObject mainController;

    private string seatid = "";
    private int seat_division;
    private int seat_row;
    private int seat_no;

    private static float[] queueEnd = new float[2] { -400.0f, 0.0f };
    private float[] div_pos;
    private float[] seat_pos;

    private float speed = 5f;
    private float[] currPos = new float[2] { -490.0f, 0.0f };

    private bool isQueueWalk = true;
    private bool isDivWalk = false;
    private bool isAisleWalk = false;
    private bool isRowWalk = false;
    private bool isFinished = false;

    public void OnButtonClick()
    {
        mainController.GetComponent<MainController>().ButtonClickPosition(seat_row, seat_no);
    }

    private void MovePassenger(float dx, float dy)
    {
        Vector2 pos = transform.localPosition;
        pos.x += dx;
        currPos[0] = pos.x;
        pos.y += dy;
        currPos[1] = pos.y;
        transform.localPosition = pos;
    }

    private void TeleportPassenger(float[] x)
    {
        currPos[0] = x[0];
        currPos[1] = x[1];
        transform.localPosition = new Vector2(x[0], x[1]);
    }

    private void TeleportPassenger(float x, float y)
    {
        currPos[0] = x;
        currPos[1] = y;
        transform.localPosition = new Vector2(x, y);
    }

    public void Init(string str)
    {
        seatingPanel = GameObject.Find("SeatingPanel");
        mainController = GameObject.Find("MainController");
        str = str.Trim();
        seatid = str;
        string strRow = str.Replace('F', '0').Replace('B', '1').Trim('L', 'R');
        string strNo = str.Trim('F', 'B').Replace('L', '0').Replace('R', '1');
        seat_division = System.Convert.ToInt32(strRow[0].ToString());
        seat_row = System.Convert.ToInt32(strRow, 2);
        seat_no = System.Convert.ToInt32(strNo, 2);

        div_pos = seat_division.Equals(0) ? new float[2] { -400.0f, 81.5f } : new float[2] { -400.0f, -82f };
        seat_pos = seatingPanel.GetComponent<SeatingPanel>().GetPosition(seat_row, seat_no);
        Debug.Log(seat_row + " " + seat_no);
        Debug.Log(seat_pos[0] + ", " + seat_pos[1]);

        TeleportPassenger(currPos);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isFinished)
        {
            return;
        }

        // we want to first animate the position in queue, this will slowly move along until the end of queue point, at which point it will get "unlocked"
        if (isQueueWalk)
        {
            MovePassenger(1 * speed, 0);
            if(Mathf.Abs(currPos[0] - queueEnd[0]) <= 3)
            {
                TeleportPassenger(queueEnd);
                isQueueWalk = false;
                isDivWalk = true;
            }
        }
        // after being unlocked, it will move into the correct division, stopping at div0_pos or div1_pos

        if (isDivWalk)
        {
            MovePassenger(0, Mathf.Pow(-1, seat_division) * speed); // +1 if 0, -1 if 1

            Debug.Log(currPos[1] + ", " + div_pos[1]);
            if (Mathf.Abs(currPos[1] - div_pos[1]) <= 3)
            {
                TeleportPassenger(div_pos);
                isDivWalk = false;
                isAisleWalk = true;
            }
        }
        // upon reaching the correct div0 or div1 position, move to the correct x position along the aisle

        if (isAisleWalk)
        {
            MovePassenger(1.5f * speed, 0);

            if (Mathf.Abs(currPos[0] - seat_pos[0]) <= 4)
            {
                TeleportPassenger(seat_pos[0],currPos[1]);
                isAisleWalk = false;
                isRowWalk = true;
            }
        }

        // upon reaching the correct x position, move up or down the row to the right seat number

        if (isRowWalk)
        {
            
            MovePassenger(0, 0.2f * speed * (seat_no < 4 ? 1 : -1));

            if (Mathf.Abs(currPos[1] - seat_pos[1]) <= 3)
            {
                TeleportPassenger(seat_pos);
                isRowWalk = false;
                isFinished = true;
            }
        }
    }
}
