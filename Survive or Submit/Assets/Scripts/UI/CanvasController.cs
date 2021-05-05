using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    [SerializeField] RuntimeData runtimeData;
    [SerializeField] GameObject craftingUI;
    [SerializeField] GameObject weaponUI;
    [SerializeField] GameObject playerUI;
    [SerializeField] GameObject gameOverUI;

    // Start is called before the first frame update
    void Start()
    {
        craftingUI.SetActive(false);
        gameOverUI.SetActive(false);

        //events
        GameEvents.DisplayCrafting += OnCraftingDisplay;
        GameEvents.HideCrafting += OnCraftingHide;
        GameEvents.EndGame += OnGameOver;
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

    void OnGameOver(object sender, GameOverEventArgs args)
    {
        Cursor.visible = true;
        gameOverUI.SetActive(true);
        gameOverUI.GetComponent<GameOverUI>().setCond(args.cond);

        GameEvents.DisplayCrafting -= OnCraftingDisplay;
        GameEvents.HideCrafting -= OnCraftingHide;
        GameEvents.EndGame -= OnGameOver;
    }
}
