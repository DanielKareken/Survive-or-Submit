using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftEventArgs : EventArgs
{
    //public string weapon;
    public int ammoAmount;
    public int medAmount;
}

public class PlayerHUDEventArgs : EventArgs
{
    public int type;
    public int newAmount;
}

public class WeaponEventArgs : EventArgs
{
    public GameObject weapon;
}

public static class GameEvents
{
    //UI Events
    public static event EventHandler DisplayCrafting;
    public static event EventHandler HideCrafting;
    public static event EventHandler<CraftEventArgs> CraftThis;
    public static event EventHandler<PlayerHUDEventArgs> UpdatePlayerHUD;
    public static event EventHandler<WeaponEventArgs> UpdateWeaponUI;

    //functions
    //crafting
    public static void InvokeCraftingDisplay()
    {
        DisplayCrafting(null, EventArgs.Empty);
    }
    public static void InvokeCraftingHide()
    {
        HideCrafting(null, EventArgs.Empty);
    }

    public static void InvokeCraftThis(/*string gun*/ int ammo, int meds)
    {
        CraftThis(null, new CraftEventArgs { ammoAmount = ammo, medAmount = meds });
    }

    public static void InvokeUpdatePlayerHUD(int t, int amount)
    {
        UpdatePlayerHUD(null, new PlayerHUDEventArgs { type = t, newAmount = amount });
    }

    public static void InvokeUpdateWeaponUI(GameObject w)
    {
        UpdateWeaponUI(null, new WeaponEventArgs { weapon = w });
    }
}
