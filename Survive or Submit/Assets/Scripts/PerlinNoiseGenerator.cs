using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PerlinNoiseGenerator : MonoBehaviour
{
    [SerializeField] RuntimeData runtimeData;
    //tilemaps
    public Tilemap waterMap;
    public Tilemap groundMap;
    public Tilemap obstacleMap;
    public AnimatedTile[] waterTile; //animated water tiles
    public RuleTile[] groundTile; //ground terrain
    public Tile[] fillTile; //normal tile to fill gaps
    public Tile[] obstacleTile; //obstacle terrain (rock formations, etc)

    // Width and height of the texture in pixels.
    int mapWidth;
    int mapHeight;

    //store center of map as seperate variable
    private int xOrg;
    private int yOrg;

    void Start()
    {
        mapWidth = runtimeData.mapWidth;
        mapHeight = runtimeData.mapHeight;
        xOrg = mapWidth / 2;
        yOrg = mapHeight / 2;
        Generate();
    }
 
    private void Generate(){
        for (float  i = 0; i < mapWidth; i++)
        {
            for (float j = 0; j < mapHeight; j++)
            {
                var perlin = Mathf.PerlinNoise(i / 10, j / 10);
                int x = (int)i;
                int y = (int)j;

                if (perlin < .1f)
                {
                    //water tile
                    waterMap.SetTile(new Vector3Int(-x + mapWidth / 2, -y + mapHeight / 2, 0), waterTile[0]);
                }
                else if (perlin <.55f)
                {
                    //ground tile
                    groundMap.SetTile(new Vector3Int(-x + mapWidth / 2, -y + mapHeight / 2, 0), groundTile[0]);
                }
                else if (perlin < .65f)
                {
                    //fill tile standard
                    groundMap.SetTile(new Vector3Int(-x + mapWidth / 2, -y + mapHeight / 2, 0), fillTile[0]);
                }
                else if (perlin < .75f)
                {
                    //fill tile special
                    groundMap.SetTile(new Vector3Int(-x + mapWidth / 2, -y + mapHeight / 2, 0), fillTile[Random.Range(0,4)]);
                }
                else
                {
                    //architecture
                    groundMap.SetTile(new Vector3Int(-x + mapWidth / 2, -y + mapHeight / 2, 0), fillTile[0]);
                }
            }
        }
    }
}