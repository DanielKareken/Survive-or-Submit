using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] RuntimeData runtimeData;
    [SerializeField] GameObject player;

    public GameObject[] enemyPrefabs;
    
    Vector2 playerPos;
    float spawnTimer; //time in between spawn attempts

    // Start is called before the first frame update
    void Start()
    {
        runtimeData.currZone = 1;
        spawnTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = player.transform.position;

        //decrement spawn timer
        if (spawnTimer > 0)
        {
            spawnTimer -= Time.deltaTime;
        }
    }

    //checks what zone the player is in to determine spawn rates
    void checkZone()
    {

    }

    //spawn enemies near player
    void spawnEnemies(int max)
    {

    }
}
