using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] RuntimeData runtimeData;
    [SerializeField] GameObject player;

    public GameObject[] enemyPrefabs;
    public int totalThreshold;
    public float spawnMinDist;
    public float spawnMaxDist;

    Vector2 playerPos;
    float spawnTimer; //time in between spawn attempts
    int numToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = 10f;
        runtimeData.numActiveEnemies = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerPos = player.transform.position;

        //only run timer when player is outside of spawn area
        if(!runtimeData.playerInSpawn)
        {
            //decrement spawn timer
            if (spawnTimer > 0)
            {
                spawnTimer -= Time.deltaTime;
            }
            else
            {
                spawnEnemies();
            }
        }    
    }

    //spawn enemies near player
    void spawnEnemies()
    {
        int zombieTypeMax = checkZone();

        for (int i = 0; i < numToSpawn; i++)
        {
            Vector3 spawnPos = player.transform.position;
            //offset from player position
            Vector2 spawnOffest = Random.insideUnitCircle * spawnMinDist;
            spawnOffest.x += Random.Range(0, spawnMaxDist - spawnMinDist);
            spawnOffest.y += Random.Range(0, spawnMaxDist - spawnMinDist);
            spawnPos = new Vector3(spawnPos.x + spawnOffest.x, spawnPos.y + spawnOffest.y, 0);
            Instantiate(enemyPrefabs[Random.Range(0, zombieTypeMax)], spawnPos, transform.rotation);
            runtimeData.numActiveEnemies++;
        }

        spawnTimer = 15f;
    }
    
    //checks what zone the player is in to determine spawn rates and what types, return typesMax
    int checkZone()
    {
        int zone = runtimeData.currZone;

        switch(zone)
        {
            case 1:
                numToSpawn = 3;
                break;
            case 2:
                numToSpawn = 5;
                break;
            case 3:
                numToSpawn = 6;
                break;
            case 4:
                numToSpawn = 8;
                break;
            case 5:
                numToSpawn = 10;
                break;
            case 6:
                numToSpawn = 10;
                break;
        }

        //reduce spawn numbers after a certain number of zombies are present
        if(runtimeData.numActiveEnemies >= 15)
        {
            numToSpawn /= 2;
        }
        else if (runtimeData.numActiveEnemies >= 30)
        {
            numToSpawn = 1;
        }

        return zone;
    }
}
