using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;

public class MainController : MonoBehaviour
{
    public TileController tileController;

    private IEnumerator Main()
    {
        Regex reg_dir = new Regex(@"ne|nw|se|sw|e|w");
        List<List<(int x, int y)>> input = System.IO.File.ReadLines("./Assets/Input/day24input.txt").Select(line => (from Match m in reg_dir.Matches(line) select ConvertDirection(m.Value)).ToList()).ToList();

        Dictionary<(int x, int y), int> active_tiles = new Dictionary<(int x, int y), int>();
        active_tiles = Part1(input);

        yield return Part2(active_tiles);
    }

    private Dictionary<(int x, int y), int> Part1(List<List<(int x, int y)>> input)
    {
        List<(int x, int y)> tiles = input.Select(list => SumVectors(list)).ToList();
        Dictionary<(int x, int y), int> directions_count = new Dictionary<(int x, int y), int>();
        
        foreach((int x, int y) tuple in tiles)
        {
            if (directions_count.ContainsKey(tuple))
            {
                directions_count[tuple] = (directions_count[tuple] + 1) % 2;
            }
            else
            {
                directions_count.Add(tuple, 1);
            }
        }

        int sum = 0;
        foreach(int count in directions_count.Values)
        {
            sum += count;
        }

        Debug.Log($"Number of Black Tiles: " + sum);

        Dictionary<(int x, int y), int> active_tiles = new Dictionary<(int x, int y), int>();
        foreach((int x, int y) pos in directions_count.Keys)
        {
            if (directions_count[pos].Equals(1))
            {
                active_tiles.Add(pos, 1);
            }
        }

        return active_tiles;
    }

    private IEnumerator Part2(Dictionary<(int x, int y), int> active_tiles)
    {
        (int x, int y)[] deltas = { (1, 0), (-1, 0), (0, 1), (0, -1), (-1, 1), (1, -1) };
        int STEPS = 1000;

        Dictionary<(int x, int y), int> active_neighbours_count = new Dictionary<(int x, int y), int>();

        //tileController.RunTest();
        //yield return new WaitForSeconds(20.0f);

        for(int i = 1; i < STEPS+1; i++)
        {
            tileController.DrawActiveTiles(active_tiles);

            active_neighbours_count = AddNeighbours(active_tiles, deltas);

            yield return new WaitForSeconds(0.5f);
            //yield return new WaitForEndOfFrame();

            tileController.DrawNeighbourTiles(active_neighbours_count);

            active_tiles = UpdateActiveTiles(active_tiles, active_neighbours_count);

            //yield return new WaitForSeconds(0.1f);
            yield return new WaitForEndOfFrame();

            if ((i % 5) == 0)
            {
                CountActiveTiles(active_tiles, i);
            }
        }
    }

    private Dictionary<(int x, int y), int> AddNeighbours(Dictionary<(int x, int y), int> active_tiles, (int x, int y)[] deltas)
    {
        Dictionary<(int x, int y), int> active_neighbours_count = new Dictionary<(int x, int y), int>();

        foreach((int x, int y) pos in active_tiles.Keys)
        {
            foreach ((int x, int y) delta in deltas)
            {
                (int x, int y) deltaPos = (pos.x + delta.x, pos.y + delta.y);
                if (active_neighbours_count.ContainsKey(deltaPos))
                {
                    active_neighbours_count[deltaPos]++;
                }
                else
                {
                    active_neighbours_count.Add(deltaPos, 1);
                }
            }
        }

        return active_neighbours_count;
    }

    private Dictionary<(int x, int y), int> UpdateActiveTiles(Dictionary<(int x, int y), int> active_tiles, Dictionary<(int x, int y), int> active_neighbours_count)
    {
        Dictionary<(int x, int y), int> new_active_tiles = new Dictionary<(int x, int y), int>();

        foreach((int x, int y) pos in active_neighbours_count.Keys)
        {
            if (active_tiles.ContainsKey(pos) && (active_neighbours_count[pos].Equals(1) || active_neighbours_count[pos].Equals(2)))
            {
                new_active_tiles.Add(pos, 1);
            }
            else if(!active_tiles.ContainsKey(pos) && active_neighbours_count[pos].Equals(2))
            {
                new_active_tiles.Add(pos, 1);
            }
        }

        return new_active_tiles;
    }

    private void CountActiveTiles(Dictionary<(int x, int y), int> active_tiles, int i )
    {
        Debug.Log($"Number of active tiles on step {i}: {active_tiles.Count()}");
    }

    private (int x, int y) SumVectors(List<(int x, int y)> list)
    {
        return list.Aggregate((p, q) => (p.x + q.x, p.y + q.y));
    }

    private (int x, int y) ConvertDirection(string dir)
    {
        switch (dir)
        {
            case "e": 
                return (1, 0);
            case "w": 
                return (-1, 0);
            case "ne":
                return (0, 1);
            case "sw":
                return (0, -1);
            case "nw": // in fact: z = y - x, we don't need the third dimension at all.
                return (-1, 1);
            case "se":
                return (1, -1);
            default: 
                Debug.Log($"Error reading dir: {dir}");
                return (0, 0);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Main");
    }
}
