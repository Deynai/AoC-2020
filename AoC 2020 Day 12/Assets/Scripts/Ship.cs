using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    private Vector2 position = new Vector2(0, 0);
    private Vector2 facing = new Vector2(1, 0);

    public void RunCommand(string command)
    {
        char key = command[0];
        int val = int.Parse(command.Substring(1));

        switch (key)
        {
            case 'N': 
                MoveInDirection(new Vector2(0, 1), val);
                break;
            case 'E':
                MoveInDirection(new Vector2(1, 0), val);
                break;
            case 'S':
                MoveInDirection(new Vector2(0, -1), val);
                break;
            case 'W':
                MoveInDirection(new Vector2(-1, 0), val);
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

        UpdatePosition();
    }

    private void MoveInDirection(Vector2 dir, int val)
    {
        position += dir * val;
    }

    private void MoveForward(int val)
    {
        position += facing * val;
    }

    private void Rotate(int wise, float degrees)
    {
        float sin = Mathf.Sin(degrees * wise * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * wise * Mathf.Deg2Rad);

        float x_dir = facing.x;
        float y_dir = facing.y;

        facing.x = (cos * x_dir) - (sin * y_dir);
        facing.y = (sin * x_dir) + (cos * y_dir);
    }

    private void UpdatePosition()
    {
        transform.position = position;
    }

    public string DisplayPosition()
    {
        return ("Facing (" + facing.x + ", " + facing.y + ") at position (" + position.x + ", " + position.y + ")");
    }

    public int GetManhattanDistance()
    {
        return Mathf.RoundToInt(Mathf.Abs(position.x) + Mathf.Abs(position.y));
    }
}
