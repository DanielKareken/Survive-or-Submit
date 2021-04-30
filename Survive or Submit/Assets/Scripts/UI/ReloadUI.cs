using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadUI : MonoBehaviour
{
    public GameObject fill;
    public GameObject border;
    public Slider slider;
    public Gradient gradient;
    

    float reloadTimer;
    bool reloaded;
    bool showing;

    private void Start()
    {
        slider = GetComponent<Slider>();
        fill.SetActive(false);
        border.SetActive(false);
        reloaded = true;
        showing = false;

        GameEvents.ReloadWeapon += OnReloadWeapon;
        GameEvents.EndGame += OnGameOver;
    }

    // Update is called once per frame
    void Update()
    {
        //timer
        if (!reloaded)
        {
            if (reloadTimer > 0)
            {
                reloadTimer -= Time.deltaTime;
                slider.value = reloadTimer;
            }
            else
            {
                reloaded = true;
                GameEvents.InvokeWeaponRealoaded();
            }
        }
    }

    public void ToggleDisplay()
    {
        if (showing)
        {
            fill.SetActive(false);
            border.SetActive(false);
            showing = false;
        }
        else
        {
            fill.SetActive(true);
            border.SetActive(true);
            showing = true;
        }
    }

    void OnReloadWeapon(object sender, ReloadEventArgs args)
    {
        if(reloadTimer > 0)
        {
            reloaded = true;
            reloadTimer = 0;
        }
        else
        {
            reloadTimer = args.reloadTime;
            slider.maxValue = args.reloadTime;
            reloaded = false;
        }
    }

    void OnGameOver(object sender, EventArgs args)
    {
        GameEvents.ReloadWeapon -= OnReloadWeapon;
        GameEvents.EndGame -= OnGameOver;
    }
}
