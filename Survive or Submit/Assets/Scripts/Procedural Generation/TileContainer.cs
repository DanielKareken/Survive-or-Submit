using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileContainer : MonoBehaviour
{
    [SerializeField] Tile tile;
    public int width;
    public int height;

    public Tile getTile()
    {
        return tile;
    }
}
