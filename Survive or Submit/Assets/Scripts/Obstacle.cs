using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Obstacle : MonoBehaviour
{
    [SerializeField] Tile tile;

    public int width;
    public int height;

    private void Start()
    {
        width = 10;
        height = 10;
    }

    public Tile getTile()
    {
        return tile;
    }
}
