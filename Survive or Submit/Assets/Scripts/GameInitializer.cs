using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] RuntimeData runtimeData;
    [SerializeField] AudioSource ambientEnvironment;

    public int startingScrap;
    public int startingMeds;

    // Start is called before the first frame update
    void Start()
    {
        Logger logger = new Logger(@"C:C:\Users\Daniel Kareken\Desktop\MyLog.log");
        logger.WriteLine("Hello World");

        //initialize RuntimeData vars
        runtimeData.player_scrap = startingScrap;
        runtimeData.player_meds = startingMeds;
        runtimeData.playerInSpawn = true;
        runtimeData.currZone = 1;
        runtimeData.gameOver = false;

        //play sounds
        ambientEnvironment.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
