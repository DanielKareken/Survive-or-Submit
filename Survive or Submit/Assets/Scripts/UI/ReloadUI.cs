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
                ToggleDisplay(); //turn off reload UI
                GameEvents.InvokeWeaponRealoaded();
            }
        }
    }

    void ToggleDisplay()
    {
        bool isActive = !showing;

        fill.SetActive(isActive);
        border.SetActive(isActive);
        showing = isActive;
    }

    public void UpdateReloadWeaponUI(float reloadTime)
    {
        if(reloadTimer > 0)
        {
            reloaded = true;
            reloadTimer = 0;
        }
        else
        {
            reloadTimer = reloadTime;
            slider.maxValue = reloadTime;
            reloaded = false;
        }

        ToggleDisplay();
    }

    void OnGameOver(object sender, EventArgs args)
    {
        GameEvents.EndGame -= OnGameOver;
    }
}
