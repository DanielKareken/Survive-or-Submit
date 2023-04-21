using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public RuntimeData runtimeData;
    public GameObject healthBar;
    public GameObject levelBar;
    public GameObject compass;
    public GameObject currentZone;
    public GameObject reloadBar;
    public GameObject playerLevelText;
    public GameObject playerLevelBar;
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
        GameEvents.WeaponReloaded += OnWeaponReloaded;
        GameEvents.EndGame += OnGameOver;
    }

    // Update is called once per frame
    void Update()
    {
        //reloadBar.transform.position = new Vector3(playerPos.position.x, playerPos.position.y + 10, 0);
        playerLevelText.GetComponent<Text>().text = "Lv " + runtimeData.player_level;
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
        levelBar.SetActive(false);
    }

    void OnCraftingHide(object sender, EventArgs args)
    {
        healthBar.SetActive(true);
        compass.SetActive(true);
        currentZone.SetActive(true);
        levelBar.SetActive(true);
    }

    public void CallReloadWeaponUI(float reloadTime)
    {
        //cancel reload if already reloading
        if (reloading)
        {
            reloading = false;
            runtimeData.allowWeaponFire = true;      
        }
        else
        {
            reloading = true;
            runtimeData.allowWeaponFire = false;
        }

        reloadBar.GetComponent<ReloadUI>().UpdateReloadWeaponUI(reloadTime);
    }

    void OnWeaponReloaded(object sender, EventArgs args)
    {
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
