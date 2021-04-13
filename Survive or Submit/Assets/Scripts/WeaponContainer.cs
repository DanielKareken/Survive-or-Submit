using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New WeaponContainer")]

public class WeaponContainer : ScriptableObject
{
    [SerializeField] GameObject[] weapons;
}
