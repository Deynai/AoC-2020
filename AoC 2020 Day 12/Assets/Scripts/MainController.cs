using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class MainController : MonoBehaviour
{
    public GameObject shipObject;
    private Ship ship;
    public GameObject ship2Object;
    private Ship2 ship2;

    private IEnumerator Main()
    {
        string[] input = System.IO.File.ReadAllLines("./Assets/Input/day12input.txt");

        //yield return Part1(input);

        yield return Part2(input);
    }

    private IEnumerator Part1(string[] input)
    {
        foreach(string line in input)
        {
            ship.RunCommand(line);
            Debug.Log(ship.DisplayPosition());
            yield return new WaitForSeconds(0.1f);
        }

        Debug.Log(ship.GetManhattanDistance());
    }

    private IEnumerator Part2(string[] input)
    {
        foreach (string line in input)
        {
            ship2.RunCommand(line);
            Debug.Log(ship2.DisplayPosition());
            yield return new WaitForSeconds(0.1f);
        }

        Debug.Log(ship2.GetManhattanDistance());
    }

    private void Awake()
    {
        ship = shipObject.GetComponent<Ship>();
        ship2 = ship2Object.GetComponent<Ship2>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Main");
    }
}
