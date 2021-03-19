using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class Day3TileMapController : MonoBehaviour
{
    public GameObject tilemapBase;
    public GameObject tilemapObjects;
    public TileBase snowTile;
    public TileBase treeTile;
    public TileBase santaTile; // we may need to change this to a sprite in the gameworld that interacts with the tilemaps
    public TileBase impactTile;
    public TextMeshProUGUI impactText;

    public Collider2D objectCollider;

    public GameObject impactTextParent;
    public GameObject textPrefab;

    public GameObject sled;
    private GameObject newsled;

    private char[,] map;
    private char[,] inputmap;
    private int[] visibleXY = new int[2] { 0, 0 };
    private int drawDistance = 20;

    private int impacts = 0;
    private int[] impactTable = new int[5];
    private bool finished = true;

    public void Main()
    {
        // load array data
        string[] input = System.IO.File.ReadAllLines("./Assets/Input/Day3input.txt");

        inputmap = new char[input.Length,input[0].Length];
        map = new char[input.Length, input[0].Length];

        for (int i = 0, end = input.Length; i < end; i++)
        {
            char[] charLine = input[i].ToCharArray();

            for(int j = 0, endrow = charLine.Length; j < endrow; j++)
            {
                inputmap[i, j] = charLine[j];
                /*
                PlaceSnow(j, -i);
                PlaceTree(charLine[j], j, -i);
                */
            }
        }
        StartCoroutine(StartSledTests());
    }

    private int RunCodeTest(int dx, int dy)
    {
        int sx = dx;
        int sy = dy;
        int impactCount = 0;

        while(sy < inputmap.GetLength(0))
        {
            if (inputmap[sy, sx].Equals('#'))
            {
                impactCount++;
            }
            sx = (sx + dx) % inputmap.GetLength(1);
            sy += dy;
        }

        return impactCount;
    }

    private IEnumerator StartSledTests()
    {
        yield return RunSledTest(1, 1, 0);
        yield return RunSledTest(3, 1, 1);
        yield return RunSledTest(5, 1, 2);
        yield return RunSledTest(7, 1, 3);
        yield return RunSledTest(1, 2, 4);

        long prod = (long)impactTable[0] * impactTable[1] * impactTable[2] * impactTable[3] * impactTable[4];
        GameObject txt = Instantiate(textPrefab, impactTextParent.transform, false);
        txt.GetComponent<ImpactText>().SetText(prod.ToString());
    }

    private IEnumerator RunSledTest(int dx, int dy, int z)
    {
        for(int i = 0; i < inputmap.GetLength(0); i++)
        {
            for(int j = 0; j < inputmap.GetLength(1); j++)
            {
                map[i, j] = inputmap[i, j];
            }
        }

        // reset the drawn tiles
        tilemapBase.GetComponent<Tilemap>().ClearAllTiles();
        tilemapObjects.GetComponent<Tilemap>().ClearAllTiles();

        // create sled
        newsled = Instantiate(sled, new Vector3(0, 0, 0), Quaternion.identity);
        newsled.GetComponent<SledController>().Init(dx, dy);

        // init values
        finished = false;
        DrawVisibleArea(0, 0);

        // wait until finished
        while (newsled.GetComponent<SledController>().finished == false)
        {
            yield return new WaitForSeconds(3.0f);
        }

        finished = true;
        Destroy(newsled);
        // set text and return

        GameObject txt = Instantiate(textPrefab, impactTextParent.transform, false);
        txt.GetComponent<ImpactText>().SetText(impacts.ToString());
        impactTable[z] = impacts;
        impacts = 0;
        impactText.text = "0";

        yield break;
    }

    private void DrawVisibleArea(int x, int y)
    {
        for (int i = x-drawDistance; i < x+ drawDistance; i++)
        {
            
            for(int j = y-drawDistance; j < y+drawDistance; j++)
            {
                PlaceSnow(j, -i);

                if (i < map.GetLength(0) && i >= 0)
                {
                    PlaceObject(map[i, (j + map.GetLength(1)) % map.GetLength(1)], j, -i);
                }
            }
        }

        visibleXY[0] = x;
        visibleXY[1] = y;
    }

    private void DestroyVisibleArea()
    {
        for (int i = visibleXY[0] - drawDistance; i < visibleXY[0] + drawDistance; i++)
        {
            for (int j = visibleXY[1] - drawDistance; j < visibleXY[1] + drawDistance; j++)
            {
                PlaceVoid(j, -i);
            }
        }
    }

    private void PlaceSnow(int i, int j)
    {
        Vector3Int pos = new Vector3Int(i, j, 0);
        tilemapBase.GetComponent<Tilemap>().SetTile(pos, snowTile);
    }

    private void PlaceObject(char ch, int i, int j)
    {
        Vector3Int pos = new Vector3Int(i, j, 0);
        switch (ch)
        {
            case '#': tilemapObjects.GetComponent<Tilemap>().SetTile(pos, treeTile);
                break;
            case '%': tilemapObjects.GetComponent<Tilemap>().SetTile(pos, impactTile);
                break;
            default:
                tilemapObjects.GetComponent<Tilemap>().SetTile(pos, null);
                break;
        }
    }

    public void SetImpact(int x, int y)
    {
        int modx = x % map.GetLength(1);

        impacts++;
        impactText.text = impacts.ToString();
        map[-y, modx] = '%';
        PlaceObject('%', x, y);
        //DrawVisibleArea(-y, x);
    }

    private void PlaceVoid(int i, int j)
    {
        Vector3Int pos = new Vector3Int(i, j, 0);
        tilemapBase.GetComponent<Tilemap>().SetTile(pos, null);
        tilemapObjects.GetComponent<Tilemap>().SetTile(pos, null);
    }

    public int GetSize(int i)
    {
        return map.GetLength(i);
    }

    // Start is called before the first frame update
    void Start()
    {
        Main();
    }

    void FixedUpdate()
    {
        if (!finished)
        {
            Vector3 sledPos = newsled.transform.localPosition;
            sledPos.x -= 0.5f;
            sledPos.y -= 0.2f;
            sledPos.x /= 2;
            sledPos.y /= 2;

            if (Mathf.Abs(-sledPos.y - visibleXY[0]) + Mathf.Abs(sledPos.x - visibleXY[1]) >= 5)
            {
                //DestroyVisibleArea();
                DrawVisibleArea(Mathf.FloorToInt(-sledPos.y), Mathf.FloorToInt(sledPos.x));
            }

            if (Mathf.Abs(sledPos.x - Mathf.RoundToInt(sledPos.x)) + Mathf.Abs(-sledPos.y - Mathf.RoundToInt(-sledPos.y)) < 0.1f)
            {
                int x = Mathf.RoundToInt(sledPos.x) % map.GetLength(1);
                int y = Mathf.RoundToInt(-sledPos.y);

                if (y >= map.GetLength(0))
                {
                    finished = true;
                    //Debug.Log("Total impacts: " + impacts);
                    return;
                }
                /*
                if (y >= 0 && map[y, x].Equals('#'))
                {
                    Debug.Log("posX: " + sledPos.x);
                    Debug.Log("posY: " + sledPos.y);
                    impacts++;
                    impactText.text = impacts.ToString();
                    map[y, x] = '%';
                    DrawVisibleArea(Mathf.FloorToInt(-sledPos.y), Mathf.FloorToInt(sledPos.x));
                }
                */
            }
        }
    }
}
