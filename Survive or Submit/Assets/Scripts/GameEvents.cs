using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoEventArgs : EventArgs
{ 
    public int amount;
}

public class WeaponEventArgs : EventArgs
{
    public GameObject weapon;
    public int index;
}

public class HealthEventArgs : EventArgs
{
    public int playerHealth;
    public float healCooldown;
}

public class GameOverEventArgs : EventArgs
{
    public string cond;
}

public class ZoneEventArgs: EventArgs
{
    public string zone;
}

public class ReloadEventArgs : EventArgs
{
    public float reloadTime;
}

public static class GameEvents
{
    //UI Events
    public static event EventHandler DisplayCrafting;
    public static event EventHandler HideCrafting;
    public static event EventHandler<AmmoEventArgs> CraftAmmo;
    public static event EventHandler<WeaponEventArgs> UpdateWeapon;
    public static event EventHandler<WeaponEventArgs> UpdateWeaponUI;
    public static event EventHandler<HealthEventArgs> UpdateHealthUI;
    public static event EventHandler<HealthEventArgs> ActivateHeal;
    public static event EventHandler HealEnded;
    public static event EventHandler<GameOverEventArgs> EndGame;
    public static event EventHandler UpdateInventory;
    public static event EventHandler<ZoneEventArgs> UpdateZoneUI;
    public static event EventHandler<ReloadEventArgs> ReloadWeapon;
    public static event EventHandler WeaponReloaded;
    

    //functions

    //activate/deactivate crafting feature
    public static void InvokeCraftingDisplay()
    {
        DisplayCrafting(null, EventArgs.Empty);
    }
    public static void InvokeCraftingHide()
    {
        HideCrafting(null, EventArgs.Empty);
    }

    //simulate crafting the given input
    public static void InvokeCraftAmmo(int x)
    {
        CraftAmmo(null, new AmmoEventArgs { amount = x });
    }

    //handles updating WeaponHUD when crafting/swapping weapons
    public static void InvokeUpdateWeapon(int i)
    {
        UpdateWeapon(null, new WeaponEventArgs { index = i });
    }

    //handles updating WeaponHUD on firing/reloading
    public static void InvokeUpdateWeaponUI(GameObject w, int i)
    {
        UpdateWeaponUI(null, new WeaponEventArgs { weapon = w, index = i });
    }

    //handles changing healthbar on player health change
    public static void InvokeUpdateHealthUI(int h)
    {
        UpdateHealthUI(null, new HealthEventArgs { playerHealth = h});
    }

    //handles updating player meds UI and displaying heal cooldown
    public static void InvokeActivateHeal(float cd)
    {
        ActivateHeal(null, new HealthEventArgs { healCooldown = cd });
    }
    
    //hide the med coldown in UI and allow player to use heal again
    public static void InvokeHealEnded()
    {
        HealEnded(null, EventArgs.Empty);
    }

    //handles ending the game (win or lose)
    public static void InvokeEndGame(string c)
    {
        EndGame(null, new GameOverEventArgs { cond = c });
    }

    //handles updating player inventory
    public static void InvokeUpdateInventory()
    {
        UpdateInventory(null, EventArgs.Empty);
    }

    //handles updating current zone display
    public static void InvokeUpdateZone(string z)
    {
        UpdateZoneUI(null, new ZoneEventArgs { zone = z });
    }

    //display reload UI and send appropriate reload duration
    public static void InvokeReloadWeapon(float time)
    {
        ReloadWeapon(null, new ReloadEventArgs { reloadTime = time });
    }

    //hide the reload UI
    public static void InvokeWeaponRealoaded()
    {
        WeaponReloaded(null, EventArgs.Empty);
    }
}
