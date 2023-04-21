using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New WeaponContainer")]

public class WeaponContainer : ScriptableObject
{
    public Sprite weaponSideView;
    public Sprite ammoImage;
    public string nickname;
    public string special;
    public string description;
    public int levelRequired;
    public string weaponName;
}
