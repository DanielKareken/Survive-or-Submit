using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PerlinNoiseGenerator : MonoBehaviour
{
    [SerializeField] RuntimeData runtimeData;
    [SerializeField] GameManager gameManager;
    //tilemaps
    public Tilemap waterMap;
    public Tilemap groundMap;
    public Tilemap obstacleMap;
    public AnimatedTile waterTile; //animated water tiles
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
      
    }

    public void StartPerlinNoise()
    {
        mapWidth = runtimeData.mapWidth;
        mapHeight = runtimeData.mapHeight;
        xOrg = mapWidth / 2;
        yOrg = mapHeight / 2;
        StartCoroutine(Generate());
    }
 
    private IEnumerator Generate() {
        float timeElapsed = 0f;
        int i, j;
        for (i = 0; i < mapWidth; i++) { //vertical
            for (j = 0; j < mapHeight; j++) { //horizontal
                var perlin = Mathf.PerlinNoise((float)i / 10, (float)j / 10);
                int x = i;
                int y = j;
                x = -x + xOrg;
                y = -y + yOrg;

                if (perlin < .25f)
                {
                    if(x > -40 && x < 40 && y > -40 && y < 40)
                    {
                        groundMap.SetTile(new Vector3Int(x, y, 0), groundTile[0]); //ground tile
                    }
                    else
                    {                        
                        waterMap.SetTile(new Vector3Int(x, y, 0), waterTile); //water tile
                    }
                    
                }
                else if (perlin <.55f)
                {                    
                    groundMap.SetTile(new Vector3Int(x, y, 0), groundTile[0]); //ground tile
                }
                else if (perlin < .65f)
                {                    
                    groundMap.SetTile(new Vector3Int(x, y, 0), fillTile[0]); //fill tile standard
                }
                else if (perlin < .75f)
                {                    
                    groundMap.SetTile(new Vector3Int(x, y, 0), fillTile[Random.Range(0,4)]); //fill tile special
                }
                else
                {                    
                    groundMap.SetTile(new Vector3Int(x, y, 0), fillTile[0]); //architecture
                }
                //Debug.Log("Tile geterated at " + x + ", " + y);         
            }

            timeElapsed += Time.deltaTime;
            //Debug.Log("Time elapsed: " + timeElapsed);
            GameEvents.InvokeUpdateLoadingUI(i + (j / mapHeight), mapWidth, timeElapsed, "Generating tiles");
            yield return new WaitForEndOfFrame();                  
        }
        gameManager.OnPerlinNoiseFinished();
        //End of Generate
    }

    //similar to generate but starts from the center of the map an does outwards
    private IEnumerator Generate2()
    {
        for (int i = (mapWidth / 2) + 25; i < mapWidth; i++) {
            for (int j = 0; j < mapHeight; j++) {
                var perlin = Mathf.PerlinNoise((float)i / 10, (float)j / 10);
                int x = i;
                int y = j;
                x = -x + xOrg;
                y = -y + yOrg;

                if (perlin < .25f)
                {
                    if (x > -40 && x < 40 && y > -40 && y < 40)
                    {
                        groundMap.SetTile(new Vector3Int(x, y, 0), groundTile[0]); //ground tile
                    }
                    else
                    {
                        waterMap.SetTile(new Vector3Int(x, y, 0), waterTile); //water tile
                    }

                }
                else if (perlin < .55f)
                {
                    groundMap.SetTile(new Vector3Int(x, y, 0), groundTile[0]); //ground tile
                }
                else if (perlin < .65f)
                {
                    groundMap.SetTile(new Vector3Int(x, y, 0), fillTile[0]); //fill tile standard
                }
                else if (perlin < .75f)
                {
                    groundMap.SetTile(new Vector3Int(x, y, 0), fillTile[Random.Range(0, 4)]); //fill tile special
                }
                else
                {
                    groundMap.SetTile(new Vector3Int(x, y, 0), fillTile[0]); //architecture
                }
                //Debug.Log("Tile geterated at " + x + ", " + y);         
            }
            yield return new WaitForEndOfFrame();
        }
        //End of Generate2
    }
}