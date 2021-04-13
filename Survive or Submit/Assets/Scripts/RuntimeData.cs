using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New RuntimeData")]

public class RuntimeData : ScriptableObject
{
    public GameObject player_weapon;
    public int player_scrap;

    public int mapWidth;
    public int mapHeight;

    public int currZone;
}
