using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] RuntimeData runtimeData;
    [SerializeField] GameObject healthBar;
    [SerializeField] GameObject compass;
    [SerializeField] GameObject currentZone;
    [SerializeField] GameObject reloadBar;

    public Transform playerPos;

    HealthBar bar;
    bool reloading;

    // Start is called before the first frame update
    void Start()
    {
        bar = healthBar.GetComponent<HealthBar>();
        bar.SetMaxHealth(runtimeData.player_max_health);
        reloading = false;

        //game events
        GameEvents.UpdateHealthUI += OnHealthUpdated;
        GameEvents.DisplayCrafting += OnCraftingDisplay;
        GameEvents.HideCrafting += OnCraftingHide;
        GameEvents.ReloadWeapon += OnReloadWeapon;
        GameEvents.WeaponReloaded += OnWeaponReloaded;
        GameEvents.EndGame += OnGameOver;
    }

    // Update is called once per frame
    void Update()
    {
        //reloadBar.transform.position = new Vector3(playerPos.position.x, playerPos.position.y + 10, 0);
    }

    void OnHealthUpdated(object sender, HealthEventArgs args)
    {
        bar.SetHealth(args.playerHealth);  
    }

    void OnCraftingDisplay(object sender, EventArgs args)
    {
        healthBar.SetActive(false);
        compass.SetActive(false);
        currentZone.SetActive(false);
    }

    void OnCraftingHide(object sender, EventArgs args)
    {
        healthBar.SetActive(true);
        compass.SetActive(true);
        currentZone.SetActive(true);
    }

    void OnReloadWeapon(object sender, ReloadEventArgs args)
    {
        //cancel reload if already reloading
        if (reloading)
        {
            reloadBar.GetComponent<ReloadUI>().ToggleDisplay();
            reloading = false;
            runtimeData.allowWeaponFire = true;
        }
        else
        {
            reloadBar.GetComponent<ReloadUI>().ToggleDisplay();
            reloading = true;
            runtimeData.allowWeaponFire = false;
        }
    }

    void OnWeaponReloaded(object sender, EventArgs args)
    {
        reloadBar.GetComponent<ReloadUI>().ToggleDisplay();
        reloading = false;
        runtimeData.allowWeaponFire = true;
    }

    void OnGameOver(object sender, GameOverEventArgs args)
    {
        GameEvents.ActivateHeal -= OnHealthUpdated;
        GameEvents.DisplayCrafting -= OnCraftingDisplay;
        GameEvents.HideCrafting -= OnCraftingHide;
        GameEvents.EndGame -= OnGameOver;
    }
}
