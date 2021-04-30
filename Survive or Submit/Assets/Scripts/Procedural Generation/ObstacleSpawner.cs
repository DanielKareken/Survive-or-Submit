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
    int spawnX;
    int spawnY;
    Logger logger;

    // Start is called before the first frame update
    void Start()
    {
        logger = new Logger(@"C:C:\Users\Daniel Kareken\Desktop\MyLog.log");

        mapSize.x = runtimeData.mapWidth;
        mapSize.y = runtimeData.mapHeight;
        width = (int)mapSize.x;
        height = (int)mapSize.y;
        spawnX = -40; //stores offset for x of safehouse area
        spawnY = -40; //stores offset for y of safehouse area
        tileDict = new Dictionary<Vector2, int>();
        generateObstacles();
        //displaySpawnZone();
    }

    void generateObstacles()
    {
        bool stillToSpawn;
        int countSpawned = 0;

        for (int i = 0; i < spawnLimit; i++)
        {
            stillToSpawn = true;
            Vector2 spawnPos;
            TileContainer spawnTile;
            int attempts = 0;

            //try to spawn obstacle until successful or after certain num of attempts
            while (stillToSpawn && attempts < 5)
            {
                attempts++;
                spawnPos = getRandPos();
                //logger.WriteLine("Trying to spawn for position: " + spawnPos);
                spawnTile = tiles[Random.Range(0, tiles.Length)];

                //on successful position found
                if (isValidSpawnPos(spawnTile, spawnPos))
                {
                    Vector3Int spawnPos3 = new Vector3Int((int)spawnPos.x, (int)spawnPos.y, 0);
                    obstacleMap.SetTile(spawnPos3, spawnTile.getTile());
                    updateTileDict(spawnTile, spawnPos);
                    stillToSpawn = false;
                    countSpawned++;
                }
                
                //logger.WriteLine("Trying to spawn for object number " + i);
            }
            //logger.WriteLine("Just spawned object number " + i);
        }
        logger.WriteLine("Num successfully spawned objects: " + countSpawned);
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

        //check matrix to see if it overlaps with any other obstacles or the safehouse
        for (int i = tileTopLeft; i < tileXOffset; i++) //x axis
        {
            for (int j = tileTopLeft; j < tileYOffset; j++) //y axis
            {
                Vector2 checkPos = new Vector2(i, j);
                if((checkPos.x >= spawnX && checkPos.x <= spawnX + 80) && (checkPos.y >= spawnY && checkPos.y <= spawnY + 80)) //check bounds of safehouse
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
                //log.WriteLine("new pos: " + newPos);
            }
        }
    }

    //for testing area of spawn zone
    void displaySpawnZone()
    {
        for (int i = spawnX; i < spawnX + 80; i++)
        {
            for (int j = spawnY; j < spawnY + 80; j++)
            {
                obstacleMap.SetTile(new Vector3Int(i, j, 0), tiles[5].getTile());
            }
        }
    }

}
