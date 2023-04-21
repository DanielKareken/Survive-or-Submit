using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponButton : MonoBehaviour
{
    public Text WeaponName;

    public void SetName(string name)
    {
        WeaponName.text = name;
    }
}
