using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New RuntimeData")]

public class RuntimeData : ScriptableObject
{
    //player specific
    public int player_scrap;
    public int player_meds;
    public int player_level;
    public int player_experience;
    public int player_max_health;
    public bool playerInSpawn;
    public bool allowWeaponFire;
    public int currZone;

    //global variables
    public int mapWidth;
    public int mapHeight;
    public bool gameOver; //keeps player from triggering OnExit when disabling player

    //scalers for difficulty
}
