using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New RuntimeData")]

public class RuntimeData : ScriptableObject
{
    [Header("Player Stats")]
    public int player_scrap;
    public int player_meds;
    public int player_level;
    public int player_experience;
    public int player_exp_to_lvl_up;
    public int player_max_health;
    public bool playerInSpawn;
    public bool allowWeaponFire;
    public int currZone;
    public int weapons_unlocked;

    [Header("Gear modifiers")]
    public double player_damage_mutli = 1;
    public double player_speed_multi = 1;
    public double player_defense_multi = 1;
    public double player_heal_multi = 1;

    [Header("Global variables")]
    public int mapWidth;
    public int mapHeight;
    public bool gameOver; //keeps player from triggering OnExit when disabling player
    public int numActiveEnemies;
    public bool game_paused;

    //scalers for difficulty
}
