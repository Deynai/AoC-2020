using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapObjectsController : MonoBehaviour
{
    public TileBase impactTile;
    public Day3TileMapController mapController;

    void OnTriggerEnter2D(Collider2D other)
    {
        /*
        Vector3 impactLocation = other.transform.localPosition;
        mapController.SetImpact(Mathf.FloorToInt(impactLocation.x/2), Mathf.FloorToInt(impactLocation.y/2));
        */
    }
}
