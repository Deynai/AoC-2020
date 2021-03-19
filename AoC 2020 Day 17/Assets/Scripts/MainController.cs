using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;

public class MainController : MonoBehaviour
{
    public GameObject cube;
    public GameObject cubeParent;
    private List<GameObject> cubes = new List<GameObject>();
    private List<Position>[] blocksPart1;
    private List<Position4D>[] blocksPart2;

    public TextMeshPro text;

    private bool started = false;
    private bool paused = false;

    struct Position
    {
        public int x;
        public int y;
        public int z;
        public Position(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

    }

    struct Position4D
    {
        public int x; public int y; public int z; public int w;
        public Position4D(int x, int y, int z, int w)
        {
            this.x = x; this.y = y; this.z = z;  this.w = w;
        }
    }

    private void Main()
    {
        int[][] input = System.IO.File.ReadLines("./Assets/Input/day17input.txt").Select(a => Array.ConvertAll(a.ToCharArray(), k => (k.Equals('#') ? 1 : 0)).ToArray()).ToArray();

        //Part1(input);
        //yield return new WaitForSeconds(5.0f);
        Part2(input);
    }

    private void Part1(int[][] input)
    {
        int steps = 6;
        blocksPart1 = new List<Position>[steps + 1];
        blocksPart1[0] = InitialiseBlocks(input);

        for (int i = 0; i < steps; i++)
        {
            //DrawInWorld(blocksPart1[i]);

            Dictionary<Position, int> neighbours = ConstructNeighbours(blocksPart1[i]);
            blocksPart1[i+1] = ConstructBlockList(neighbours, blocksPart1[i]);
        }
    }

    private void Part2(int[][] input)
    {
        int steps = 6;
        blocksPart2 = new List<Position4D>[steps + 1];
        blocksPart2[0] = InitialiseBlocks4D(input);

        for (int i = 0; i < steps; i++)
        {
            //DrawInWorld(blocksPart2[i]);

            Dictionary<Position4D, int> neighbours = ConstructNeighbours(blocksPart2[i]);
            blocksPart2[i+1] = ConstructBlockList(neighbours, blocksPart2[i]);

        }
    }

    private IEnumerator DrawBlocks()
    {
        int step = -1;

        /*
        foreach(List<Position> blocks in blocksPart1)
        {
            step++;
            text.text = "Part 1\nStep " + step;
            yield return DrawInWorld(blocks);
            text.text += "\nCount " + cubes.Count;
            yield return new WaitForSeconds(3.0f);
            while (paused)
            {
                yield return new WaitForEndOfFrame();
            }
        }
        
        yield return new WaitForSeconds(5.0f);
        step = -1;
        */
        foreach (List<Position4D> blocks in blocksPart2)
        {
            step++;
            text.text = "Part 2\nStep " + step;
            yield return DrawInWorldTest(blocks);
            text.text += "\nCount " + cubes.Count;
            yield return new WaitForSeconds(3.0f);
            while (paused)
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }

    // initialising a fixed array for all tiles - but this is very sparse and will spend a lot of unnecessary time calculating empty 0's
    private void BoardInit(int[][] input, int[,,] board, int[] dim)
    {
        for (int i = 0; i < dim[0]; i++)
        {
            for (int j = 0; j < dim[1]; j++)
            {
                for (int k = 0; k < dim[2]; k++)
                {
                    board[i, j, k] = 0;
                }
            }
        }

        int x_offset = (dim[0] - input.Length) / 2;
        int y_offset = (dim[1] - input[0].Length) / 2;

        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[0].Length; j++)
            {
                board[i + x_offset, j + y_offset, 0] = input[i][j];
            }
        }
    }

    private List<Position> InitialiseBlocks(int[][] input)
    {
        List<Position> blocks = new List<Position>();

        for(int i = 0; i < input.Length; i++)
        {
            for(int j = 0; j < input[0].Length; j++)
            {
                if (input[i][j].Equals(1))
                {
                    blocks.Add(new Position(j, -i, 0));
                }
            }
        }

        return blocks;
    }
    private List<Position4D> InitialiseBlocks4D(int[][] input)
    {
        List<Position4D> blocks = new List<Position4D>();

        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[0].Length; j++)
            {
                if (input[i][j].Equals(1))
                {
                    blocks.Add(new Position4D(j, -i, 0, 0));
                }
            }
        }

        return blocks;
    }

    private Dictionary<Position, int> ConstructNeighbours(List<Position> currBlocks)
    {
        Dictionary<Position, int> neighbours = new Dictionary<Position, int>();
        int[] delta = new int[3] { -1, 0, 1 };

        foreach(Position pos in currBlocks)
        {
            foreach (int i in delta)
            {
                foreach (int j in delta)
                {
                    foreach (int k in delta)
                    {
                        Position deltapos = new Position(pos.x + i, pos.y + j, pos.z + k);

                        if ((deltapos).Equals(pos) || deltapos.z < 0)
                        {
                            continue;
                        }

                        if (!neighbours.ContainsKey(deltapos))
                        {
                            neighbours.Add(deltapos, 1);

                        }
                        else
                        {
                            neighbours[deltapos]++;
                        }

                        if (deltapos.z.Equals(0) && !pos.z.Equals(0))
                        {
                            neighbours[deltapos]++;
                        }
                    }
                }
            }
        }

        return neighbours;
    }
    private Dictionary<Position4D, int> ConstructNeighbours(List<Position4D> currBlocks)
    {
        Dictionary<Position4D, int> neighbours = new Dictionary<Position4D, int>();
        int[] delta = new int[3] { -1, 0, 1 };

        foreach (Position4D pos in currBlocks)
        {
            foreach (int i in delta)
            {
                foreach (int j in delta)
                {
                    foreach (int k in delta)
                    {
                        foreach(int l in delta)
                        {
                            Position4D deltapos = new Position4D(pos.x + i, pos.y + j, pos.z + k, pos.w + l);

                            if ((deltapos).Equals(pos) || deltapos.z < 0 || deltapos.w < 0)
                            {
                                continue;
                            }

                            if (!neighbours.ContainsKey(deltapos))
                            {
                                neighbours.Add(deltapos, 1);

                            }
                            else
                            {
                                neighbours[deltapos]++;
                            }

                            if ((deltapos.z.Equals(0) && !pos.z.Equals(0)) || (deltapos.w.Equals(0) && !pos.w.Equals(0)))
                            {
                                neighbours[deltapos]++;
                            }
                        }
                    }
                }
            }
        }

        return neighbours;
    }

    private List<Position> ConstructBlockList(Dictionary<Position,int> neighbours, List<Position> currBlocks)
    {
        List<Position> newBlocks = new List<Position>();

        foreach(Position pos in neighbours.Keys)
        {
            if (currBlocks.Contains(pos) && (neighbours[pos].Equals(2) || neighbours[pos].Equals(3)))
            {
                newBlocks.Add(pos);
            }
            else if(!currBlocks.Contains(pos) && neighbours[pos].Equals(3))
            {
                newBlocks.Add(pos);
            }
        }

        return newBlocks;
    }
    private List<Position4D> ConstructBlockList(Dictionary<Position4D, int> neighbours, List<Position4D> currBlocks)
    {
        List<Position4D> newBlocks = new List<Position4D>();

        foreach (Position4D pos in neighbours.Keys)
        {
            if (currBlocks.Contains(pos) && (neighbours[pos].Equals(2) || neighbours[pos].Equals(3)))
            {
                newBlocks.Add(pos);
            }
            else if (!currBlocks.Contains(pos) && neighbours[pos].Equals(3))
            {
                newBlocks.Add(pos);
            }
        }

        return newBlocks;
    }

    private IEnumerator DrawInWorld(List<Position> blocks)
    {
        foreach(GameObject cube in cubes)
        {
            Destroy(cube);
        }
        cubes.Clear();

        float[] scale = new float[3] { 1.0f, 1.0f, 1.0f };

        foreach(Position pos in blocks)
        {
            GameObject newCube = Instantiate(cube, new Vector3(pos.x * scale[0], pos.y * scale[1], pos.z * scale[2]), Quaternion.identity);
            cubes.Add(newCube);

            if (!pos.z.Equals(0))
            {
                GameObject newCubeZ = Instantiate(cube, new Vector3(pos.x * scale[0], pos.y * scale[1], -pos.z * scale[2]), Quaternion.identity);
                cubes.Add(newCubeZ);
            }
            yield return new WaitForSeconds(0.001f);
        }
    }
    private IEnumerator DrawInWorld(List<Position4D> blocks)
    {
        foreach (GameObject cube in cubes)
        {
            Destroy(cube);
        }
        cubes.Clear();

        float[] scale = new float[4] { 1.0f, 1.0f, 1.0f, 20.0f};

        foreach (Position4D pos in blocks)
        {
            GameObject newCube = Instantiate(cube, new Vector3(pos.x * scale[0] + pos.w*scale[3], pos.y * scale[1], pos.z * scale[2]), Quaternion.identity);
            cubes.Add(newCube);

            if (!pos.z.Equals(0))
            {
                newCube = Instantiate(cube, new Vector3(pos.x * scale[0] + pos.w * scale[3], pos.y * scale[1], -pos.z * scale[2]), Quaternion.identity);
                cubes.Add(newCube);
            }

            if (!pos.w.Equals(0))
            {
                newCube = Instantiate(cube, new Vector3(pos.x * scale[0] - pos.w * scale[3], pos.y * scale[1], pos.z * scale[2]), Quaternion.identity);
                cubes.Add(newCube);

                if (!pos.z.Equals(0))
                {
                    newCube = Instantiate(cube, new Vector3(pos.x * scale[0] - pos.w * scale[3], pos.y * scale[1], -pos.z * scale[2]), Quaternion.identity);
                    cubes.Add(newCube);
                }
            }

            yield return new WaitForEndOfFrame();
        }

        yield break;
    }

    private IEnumerator DrawInWorldTest(List<Position4D> blocks)
    {
        foreach (GameObject cube in cubes)
        {
            Destroy(cube);
        }
        cubes.Clear();

        float[] scale = new float[4] { 1.0f, 1.0f, 1.0f, 0.0f };

        foreach (Position4D pos in blocks)
        {
            GameObject newCube = Instantiate(cube, new Vector3(pos.x * scale[0] + pos.w * scale[3], pos.y * scale[1] + pos.w * scale[3], pos.z * scale[2] + pos.w * scale[3]), Quaternion.identity);
            Color col = newCube.GetComponent<Renderer>().material.color;
            newCube.transform.localScale = new Vector3(0.5f + pos.w*0.09f, 0.5f + pos.w * 0.09f, 0.5f + pos.w * 0.09f);
            col.r += pos.w * 0.2f;
            col.g -= pos.w * 0.2f;
            float wsize = 0.8f - pos.w * 0.1f;
            Vector3 wsizev = new Vector3(wsize, wsize, wsize);
            newCube.GetComponent<Renderer>().material.color = col;
            newCube.transform.localScale = wsizev;
            cubes.Add(newCube);

            if (!pos.z.Equals(0))
            {
                newCube = Instantiate(cube, new Vector3(pos.x * scale[0] + pos.w * scale[3], pos.y * scale[1] + pos.w * scale[3], -pos.z * scale[2] + pos.w * scale[3]), Quaternion.identity);
                newCube.GetComponent<Renderer>().material.color = col;
                newCube.transform.localScale = wsizev;
                cubes.Add(newCube);
            }
            
            if (!pos.w.Equals(0))
            {
                /*
                newCube = Instantiate(cube, new Vector3(pos.x * scale[0] - pos.w * scale[3], pos.y * scale[1] - pos.w * scale[3], pos.z * scale[2] - pos.w * scale[3] ), Quaternion.identity);
                newCube.GetComponent<Renderer>().material.color = col;
                newCube.transform.localScale = wsizev;
                */
                cubes.Add(newCube);

                if (!pos.z.Equals(0))
                {
                    /*
                    newCube = Instantiate(cube, new Vector3(pos.x * scale[0] - pos.w * scale[3], pos.y * scale[1] - pos.w * scale[3], -pos.z * scale[2] - pos.w * scale[3]), Quaternion.identity);
                    newCube.GetComponent<Renderer>().material.color = col;
                    newCube.transform.localScale = wsizev;
                    */
                    cubes.Add(newCube);
                }
            
            }
            
            //yield return new WaitForEndOfFrame();
        }

        yield break;
    }

    // Start is called before the first frame update
    void Start()
    {
        text.text = "Press Q to start";
        Main();
    }

    private void Update()
    {
        if (!started && Input.GetKeyDown("q"))
        {
            started = true;
            StartCoroutine("DrawBlocks");
        }

        if(Input.GetKeyDown("space"))
        {
            paused = paused ? false : true;
        }
    }
}
