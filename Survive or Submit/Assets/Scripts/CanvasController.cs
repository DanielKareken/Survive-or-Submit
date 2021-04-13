using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    [SerializeField] GameObject craftingUI;
    [SerializeField] GameObject weaponUI;
    [SerializeField] GameObject playerUI;

    // Start is called before the first frame update
    void Start()
    {
        craftingUI.SetActive(false);
        GameEvents.DisplayCrafting += OnCraftingDisplay;
        GameEvents.HideCrafting += OnCraftingHide;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCraftingDisplay(object sender, EventArgs args)
    {
        craftingUI.SetActive(true);
    }

    void OnCraftingHide(object sender, EventArgs args)
    {
        craftingUI.SetActive(false);
    }
}
