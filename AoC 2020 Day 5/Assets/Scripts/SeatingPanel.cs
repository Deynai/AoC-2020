using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeatingPanel : MonoBehaviour
{
    public GameObject seat;
    public GameObject seatPanel;

    private GameObject[,] seating;

    public float[] GetPosition(int row, int w)
    {
        return new float[] {seating[row, w].transform.localPosition.x, seating[row, w].transform.localPosition.y};
    }

    private void InitialiseSeatingPlan()
    {
        int rows = 128; // how long the plane is 
        int width = 8; // how many seats on each row
        int divisions = 2; // how many times to divide the length of the plane into, for formatting
        int division_width = 40;
        int aisle_width = 20; // space in the aisle
        int spacing = 1; // space between each seat

        int seat_dimx = 12;
        int seat_dimy = 12;

        int offset_x = -360;
        int offset_y = 8 * seat_dimy + aisle_width + division_width / 2 + 1;

        seating = new GameObject[rows, width];
        
        for(int div = 0; div < divisions; div++)
        {
            for(int w = 0; w < width; w++)
            {
                for(int r = 0; r < rows/divisions; r++)
                {
                    seating[r + (div * rows / divisions), w] = Instantiate(seat, seatPanel.transform, false);

                    int xpos = r * (seat_dimx + spacing) + spacing + offset_x;
                    int ypos = -div * ((seat_dimy + spacing) * width + aisle_width + division_width) - w * (seat_dimy + spacing) + (w >= (width / 2) ? -aisle_width : 0) + offset_y;

                    seating[r + (div * rows / divisions), w].GetComponent<Seat>().Init(xpos, ypos);
                    seating[r + (div * rows / divisions), w].GetComponent<Seat>().SetText((r + (div * rows / divisions)).ToString() + "," + w.ToString());
                }
            }
        }
    }

    void Start()
    {
        InitialiseSeatingPlan();
    }
}
