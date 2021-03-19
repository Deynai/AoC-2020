using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SledController : MonoBehaviour
{
    public Camera cam;
    public Day3TileMapController mapController;

    private int x_dir;
    private int y_dir;
    private float[] sledXY;
    public bool finished = false;
    private float speed = 0.8f;
    private float timer = 0.0f;

    private RaycastHit2D ray;

    public void Init(int dx, int dy)
    {
        x_dir = dx;
        y_dir = dy;
        transform.position = new Vector3(-x_dir + 0.5f, y_dir + 0.2f, 0);
        sledXY = new float[2] { -x_dir, y_dir };
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mapController = GameObject.Find("Tilemap Controller").GetComponent<Day3TileMapController>();
    }

    private void DrawPosition()
    {
        /*
        GetComponent<Rigidbody2D>().MovePosition(new Vector3(sledXY[0] + 0.5f, sledXY[1] + 0.2f, 0));

        //this.transform.position = new Vector3(sledXY[0]+0.5f, sledXY[1]+0.2f, 0);
        cam.transform.position = new Vector3(sledXY[0]+0.5f, sledXY[1]+0.2f, -10);
        */
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.deltaTime;

        /*
        if(timer >= 5 && speed <= 3.0f)
        {
            speed = speed * 1.005f;
        }

        if (!finished)
        {
            sledXY[0] += (x_dir/20f)*speed;
            sledXY[1] -= (y_dir/20f)*speed;
            DrawPosition();
        }
        */
        cam.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        
        ray = Physics2D.Raycast(transform.position, new Vector2(-x_dir, y_dir), 100.0f);
        
        if (ray.collider)
        {
            Vector3 impactLocation = ray.point;
            mapController.SetImpact(Mathf.FloorToInt(impactLocation.x / 2), Mathf.FloorToInt(impactLocation.y / 2));
            Debug.Log(Mathf.FloorToInt(impactLocation.x / 2) + "," + Mathf.FloorToInt(impactLocation.y / 2));
        }

        if (GetComponent<Rigidbody2D>().velocity.magnitude <= 200)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector3(x_dir*3, -y_dir*3, 0));
        }

        if(Mathf.Abs(transform.position.y/2) > Mathf.Abs(mapController.GetSize(0))+2)
        {
            finished = true;
            DrawPosition();
        }
    }
}
