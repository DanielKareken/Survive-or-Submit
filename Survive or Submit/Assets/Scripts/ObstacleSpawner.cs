using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] RuntimeData runtimeData;
    public Tilemap obstacleMap; //obstacle Tilemap
    public TileContainer[] tiles; //obstacles
    public int spawnLimit;

    Dictionary<Vector2, int> tileDict; //grid position -> int (value does not matter)
    Vector2 mapSize;
    int width;
    int height;

    // Start is called before the first frame update
    void Start()
    {
        mapSize.x = runtimeData.mapWidth;
        mapSize.y = runtimeData.mapHeight;
        width = (int)mapSize.x;
        height = (int)mapSize.y;
        tileDict = new Dictionary<Vector2, int>();
        generateObstacles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void generateObstacles()
    {
        bool stillToSpawn;
        for (int i = 0; i < spawnLimit; i++)
        {
            stillToSpawn = true;
            Vector2 spawnPos;
            TileContainer spawnTile;

            while (stillToSpawn)
            {
                spawnPos = getRandPos();
                spawnTile = tiles[Random.Range(0, tiles.Length)];

                if (isValidSpawnPos(spawnTile, spawnPos))
                {
                    //print(tileDict.Keys);
                    Vector3Int spawnPos3 = new Vector3Int((int)spawnPos.x, (int)spawnPos.y, 0);
                    obstacleMap.SetTile(spawnPos3, spawnTile.getTile());
                    updateTileDict(spawnTile, spawnPos);
                    stillToSpawn = false;
                }
            }
        }
    }

    //returns a random 2D position within the width and height given in the class
    Vector2 getRandPos()
    {
        Vector2 randPos = new Vector2(Random.Range(-width / 2, width / 2), Random.Range(-height / 2, height / 2)); //realigned to place the center of the map to (0,0)
        //print(randPos);
        return randPos;
    }
    
    //determines whether a given position will not overlap antoher object
    bool isValidSpawnPos(TileContainer tile, Vector2 pos)
    {
        bool isValid = true;
        int tileW = tile.width;
        int tileH = tile.height;
        int tileTopLeft = (int)pos.x - (tileW / 2);
        int tileXOffset = tileTopLeft + tileW;
        int tileYOffset = tileTopLeft + tileH;
        int spawnX = (width / 2) - 40; //stores top left of safehouse area for x
        int spawnY = (height / 2) - 40; //stores top left of safehouse area for y

        //check matrix to see if it overlaps with any other obstacles or the safehouse
        for (int i = tileTopLeft; i < tileXOffset; i++) //x axis
        {
            for (int j = tileTopLeft; j < tileYOffset; j++) //y axis
            {
                Vector2 checkPos = new Vector2(i, j);
                if((checkPos.x >= spawnX && checkPos.x <= spawnX + 80) || (checkPos.y >= spawnY && checkPos.y <= spawnY + 80)) //check bounds of safehouse
                {
                    isValid = false;
                } 
                else if (tileDict.ContainsKey(checkPos)) {
                    isValid = false;
                }
            }
        }

        return isValid;
    }

    //save new obstacle to dictionary at approriate positions
    void updateTileDict(TileContainer tile, Vector2 pos)
    {
        int tileW = tile.width;
        int tileH = tile.height;
        int tileTopLeft = (int)pos.x - (tileW / 2);

        for (int i = tileTopLeft; i < tileTopLeft + tileW; i++)
        {
            for (int j = tileTopLeft; j < tileTopLeft + tileH; j++)
            { 
                Vector2 newPos = new Vector2(i, j);
                tileDict.Add(newPos, 1); 
            }
        }
    }
}
