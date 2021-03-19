using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainController : MonoBehaviour
{
    public GameObject seatingPanel;
    public GameObject passenger;
    public GameObject passengerPanel;

    public TextMeshProUGUI text;

    private void Main()
    {
        string[] input = System.IO.File.ReadAllLines("./Assets/Input/day5input.txt");

        StartCoroutine("CreatePassengers", input);

        /*
        string test = "BFFBFBFLRL ";
        GameObject person = Instantiate(passenger, passengerPanel.transform, false);
        person.GetComponent<Passenger>().Init(test);
        */
    }

    private IEnumerator CreatePassengers(string[] input)
    {
        yield return new WaitForSeconds(1.0f);
        foreach(string str in input)
        {
            GameObject person = Instantiate(passenger, passengerPanel.transform, false);
            Debug.Log(str);
            person.GetComponent<Passenger>().Init(str);
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void ButtonClickPosition(int row, int seat_no)
    {
        text.text = "Row: " + row.ToString() + "\nSeat: " + seat_no.ToString() + "\nSeat ID: " + (row*8 + seat_no).ToString(); 
    }

    // Start is called before the first frame update
    void Start()
    {
        Main();
        text.text = "";
    }

    void Update()
    {
    }
}
