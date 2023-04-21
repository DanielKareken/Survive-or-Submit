using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] RuntimeData runtimeData;
    [SerializeField] WeaponContainer[] weaponContainer;
    [SerializeField] GameObject ammo; //references GameObject storing text
    [SerializeField] GameObject ammoImg; //references GameObject storing ammo image
    [SerializeField] GameObject scrapAmount;
    [SerializeField] GameObject medAmount;
    [SerializeField] GameObject medCooldown;

    Weapon weapon;
    Text ammoText;
    Image img;
    float healTimer;
    int curr = 0;
    int reserve = 0;
    bool healing;

    // Start is called before the first frame update
    void Start()
    {
        ammoText = ammo.GetComponent<Text>();
        ammoText.text = curr + "/" + reserve;
        img = ammoImg.GetComponent<Image>();

        healing = false;
        medCooldown.SetActive(false);

        //events
        GameEvents.UpdateWeaponUI += OnWeaponUpdated;
        GameEvents.UpdateInventory += OnInventoryUpdated;
        GameEvents.ActivateHeal += OnHealUsed;
        GameEvents.EndGame += OnGameOver;
    }

    void Update()
    {
        //heal cooldown timer
        if (healing) {
            if (healTimer > 0)
            {
                medCooldown.GetComponent<Slider>().value = healTimer;
                healTimer -= Time.deltaTime;
            }
            else
            {
                healing = false;
                medCooldown.SetActive(false);
                GameEvents.InvokeHealEnded();
            }
        }
    }

    void OnWeaponUpdated(object sender, WeaponEventArgs args)
    {
        //print("Recieved signal from player for weapon: " + args.weapon + ". Updating UI...");
        ammoImg.GetComponent<Image>().sprite = weaponContainer[args.index].ammoImage;
        UpdateWeaponUI(args.weapon);
    }

    void UpdateWeaponUI(GameObject weapon)
    {
        Weapon w = weapon.GetComponent<Weapon>();
        curr = w.getCurrAmmo();
        reserve = w.reserveAmmo;
        ammoText.text = curr + "/" + reserve;
    }

    void OnInventoryUpdated(object sender, EventArgs args)
    {
        scrapAmount.GetComponent<Text>().text = "" + runtimeData.player_scrap;
        medAmount.GetComponent<Text>().text = "" + runtimeData.player_meds;
    }

    void OnHealUsed(object sender, HealthEventArgs args)
    {
        medAmount.GetComponent<Text>().text = "" + runtimeData.player_meds; //update ui

        healTimer = args.healCooldown;
        healing = true;
        medCooldown.SetActive(true);
        medCooldown.GetComponent<Slider>().maxValue = healTimer;
    }

    void OnGameOver(object sender, GameOverEventArgs args)
    {
        GameEvents.UpdateWeaponUI -= OnWeaponUpdated;
        GameEvents.UpdateInventory -= OnInventoryUpdated;
        GameEvents.ActivateHeal -= OnHealUsed;
        GameEvents.EndGame -= OnGameOver;
    }
}
