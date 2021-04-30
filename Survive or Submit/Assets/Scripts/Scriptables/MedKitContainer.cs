using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Size
{
    SMALL,
    LARGE
}

[CreateAssetMenu(menuName = "New MedKitContainer")]
public class MedKitContainer : ScriptableObject
{
    public Size size;
    public int healAmount;
    public float healCooldown;
}