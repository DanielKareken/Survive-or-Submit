using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] RuntimeData runtimeData;
    [SerializeField] private GameObject[] _itemCrates;
    [SerializeField] GameManager gameManager;
    public Tilemap obstacleMap; //obstacle Tilemap
    public TileContainer[] tiles; //obstacles
    public int tileSpawnLimit;
    public int itemSpawnLimit;

    Dictionary<Vector2, int> objectDict; //grid position -> int (value does not matter)
    Vector2 mapSize;
    int map_width;
    int map_height;
    int spawn_bound_X;
    int spawn_bound_Y;
    Logger logger;
    int countSpawned;

    // Start is called before the first frame update
    void Start()
    {
        logger = new Logger(@"C:C:\Users\Daniel Kareken\Desktop\MyLog.log");       
    }

    public void StartSpawningObstacles()
    {
        mapSize.x = runtimeData.mapWidth;
        mapSize.y = runtimeData.mapHeight;
        map_width = (int)mapSize.x;
        map_height = (int)mapSize.y;
        spawn_bound_X = -40; //stores offset for x of safehouse area
        spawn_bound_Y = -40; //stores offset for y of safehouse area
        objectDict = new Dictionary<Vector2, int>();
        StartCoroutine(generateObstacles());
        //displaySpawnZone();
    }

    IEnumerator generateObstacles()
    {
        bool stillToSpawn;
        countSpawned = 0;
        int spawnType;

        for (int i = 0; i < tileSpawnLimit; i++)
        {
            stillToSpawn = true;
            Vector2 spawnPos;
            int attempts = 0;
            spawnType = 0; //Random.Range(0, 1);

            //try to spawn obstacle until successful or after certain num of attempts
            while (stillToSpawn && attempts < 5)
            {
                attempts++;
                spawnPos = getRandPos();
                //logger.WriteLine("Trying to spawn for position: " + spawnPos);

                switch (spawnType)
                {
                    case 0:
                        stillToSpawn = SpawnTile(spawnPos);
                        break;
                    case 1:
                        //stillToSpawn = SpawnCrate(spawnPos);
                        break;
                    default:
                        Debug.Log("Error: Invalid case for 'spawnType' recieved");
                        break;
                }               
                //logger.WriteLine("Trying to spawn for object number " + i);
            }
            //logger.WriteLine("Just spawned object number " + i);
            //Debug.Log("Object succefully spawned at " + spawnPos);
            if(i % 5 == 0)
                yield return new WaitForEndOfFrame();
        }
        //logger.WriteLine("Num successfully spawned objects: " + countSpawned);
        gameManager.OnObstacleSpawnerFinished();
        //End of generateObstacles
    }

    bool SpawnTile(Vector2 spawnPos)
    {
        TileContainer spawnTile = tiles[Random.Range(0, tiles.Length)];
        int tileW = spawnTile.width;
        int tileH = spawnTile.height;
        //on successful position found

        if (isValidSpawnPos(tileW, tileH, spawnPos))
        {
            //Debug.Log("Spawning obstacle...");
            Vector3Int spawnPos3 = new Vector3Int((int)spawnPos.x, (int)spawnPos.y, 0);
            obstacleMap.SetTile(spawnPos3, spawnTile.getTile());
            updateObjectDict(tileW, tileH, spawnPos);
            countSpawned++;
            return false;
        }
        else
            return true;
    }

    /*
    bool SpawnCrate(Vector2 spawnPos)
    {
        GameObject itemCrateToSpawn = _itemCrates[Random.Range(0, _itemCrates.Length)];

        //on successful position found
        if (isValidSpawnPos(5, 5, spawnPos))
        {
            Debug.Log("Spawning item crate...");
            Vector3Int spawnPos3 = new Vector3Int((int)spawnPos.x, (int)spawnPos.y, 0);
            Instantiate(itemCrateToSpawn);
            itemCrateToSpawn.transform.position = spawnPos3;
            updateObjectDict(5, 5, spawnPos);
            countSpawned++;
            return false;
        }
        else
            return true;
    }*/

    //returns a random 2D position within the width and height given in the class
    Vector2 getRandPos()
    {
        Vector2 randPos = new Vector2(Random.Range(-map_width / 2, map_width / 2), Random.Range(-map_height / 2, map_height / 2)); //realigned to place the center of the map to (0,0)
        //print(randPos);
        return randPos;
    }
    
    //determines whether a given position will not overlap antoher object
    bool isValidSpawnPos(int width, int height, Vector2 pos)
    {
        bool isValid = true;
        int tileTopLeft = (int)pos.x - (width / 2);
        int tileXOffset = tileTopLeft + width;
        int tileYOffset = tileTopLeft + height;

        //check matrix to see if it overlaps with any other obstacles or the safehouse
        for (int i = tileTopLeft; i < tileXOffset; i++) //x axis
        {
            for (int j = tileTopLeft; j < tileYOffset; j++) //y axis
            {
                Vector2 checkPos = new Vector2(i, j);
                //check bounds of safehouse
                if((checkPos.x >= spawn_bound_X && checkPos.x <= spawn_bound_X * -2) && (checkPos.y >= spawn_bound_Y && checkPos.y <= spawn_bound_Y * -2)) 
                {
                    return false;
                } 
                //check if obstacle already exists there
                else if (objectDict.ContainsKey(checkPos)) {
                    return false;
                }
            }
        }

        return isValid;
    }

    //save new obstacle to dictionary at approriate positions
    void updateObjectDict(int width, int height, Vector2 pos)
    {
        int tileTopLeft = (int)pos.x - (width / 2);

        for (int i = tileTopLeft; i < tileTopLeft + width; i++)
        {
            for (int j = tileTopLeft; j < tileTopLeft + height; j++)
            { 
                Vector2 newPos = new Vector2(i, j);
                objectDict.Add(newPos, 1);
                //log.WriteLine("new pos: " + newPos);
            }
        }
    }

    //for testing area of spawn zone
    void displaySpawnZone()
    {
        for (int i = spawn_bound_X; i < spawn_bound_X + 80; i++)
        {
            for (int j = spawn_bound_Y; j < spawn_bound_Y + 80; j++)
            {
                obstacleMap.SetTile(new Vector3Int(i, j, 0), tiles[5].getTile());
            }
        }
    }

}
