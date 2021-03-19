using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship2 : MonoBehaviour
{
    private Vector2 position = new Vector2(0, 0);
    private Vector2 waypoint = new Vector2(10, 1);

    List<Vector3> positions = new List<Vector3>();
    LineRenderer line;
    int index = 0;

    public void RunCommand(string command)
    {
        char key = command[0];
        int val = int.Parse(command.Substring(1));

        switch (key)
        {
            case 'N':
                MoveWaypoint(new Vector2(0, 1), val);
                break;
            case 'E':
                MoveWaypoint(new Vector2(1, 0), val);
                break;
            case 'S':
                MoveWaypoint(new Vector2(0, -1), val);
                break;
            case 'W':
                MoveWaypoint(new Vector2(-1, 0), val);
                break;
            case 'F':
                MoveForward(val);
                break;
            case 'R':
                Rotate(-1, val);
                break;
            case 'L':
                Rotate(1, val);
                break;
            default:
                break;
        }
    }

    private void MoveWaypoint(Vector2 dir, int val)
    {
        waypoint += dir * val;
    }

    private void MoveForward(int val)
    {
        position += waypoint * val;

        UpdatePosition();
    }

    private void Rotate(int wise, float degrees)
    {
        float sin = Mathf.Sin(degrees * wise * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * wise * Mathf.Deg2Rad);

        float x_dir = waypoint.x;
        float y_dir = waypoint.y;

        waypoint.x = (cos * x_dir) - (sin * y_dir);
        waypoint.y = (sin * x_dir) + (cos * y_dir);
    }

    private void UpdatePosition()
    {
        transform.position = position/100;
        positions.Add(transform.position);
        line.positionCount = index+1;
        line.SetPosition(index, transform.position);
        index++;
    }

    public string DisplayPosition()
    {
        return ("Waypoint (" + waypoint.x + ", " + waypoint.y + ") at position (" + position.x + ", " + position.y + ")");
    }

    public int GetManhattanDistance()
    {
        return Mathf.RoundToInt(Mathf.Abs(position.x) + Mathf.Abs(position.y));
    }

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        positions.Add(new Vector2(0, 0));
        line.SetPosition(0, new Vector2(0, 0));

        line.loop = false;
        line.startWidth = 3.0f;
        line.endWidth = 3.0f;
        line.startColor = Color.white;
        line.enabled = true;
    }
}
