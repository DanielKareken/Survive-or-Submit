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
    [SerializeField] GameObject levelUpUI;
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject loadingUI;

    bool inMenu;

    // Start is called before the first frame update
    void Awake()
    {
        craftingUI.SetActive(false);
        gameOverUI.SetActive(false);
        //loadingUI.SetActive(false);

        //events
        GameEvents.DisplayCrafting += OnCraftingDisplay;
        GameEvents.HideCrafting += OnCraftingHide;
        GameEvents.DisplayLevelUpUI += OnLevelUp;
        GameEvents.PauseGame += OnGamePaused;
        GameEvents.EndGame += OnGameOver;

        inMenu = false;
    }

    void OnCraftingDisplay(object sender, EventArgs args)
    {
        craftingUI.SetActive(true);
        inMenu = true;
    }

    void OnCraftingHide(object sender, EventArgs args)
    {
        craftingUI.SetActive(false);
        inMenu = false;
    }

    void OnLevelUp(object sender, LevelUpEventArgs args)
    {
        levelUpUI.GetComponent<LevelUpUI>().levelUpAnimation(runtimeData.player_level, "CLASSIFIED", args.isCrate);
    }

    void OnGamePaused(object sender, EventArgs args)
    {
        if (runtimeData.game_paused)
        {
            runtimeData.game_paused = false;
            pauseMenuUI.SetActive(false);
            if(!inMenu)
            {
                Cursor.visible = false;
            }           
        }
        else
        {
            runtimeData.game_paused = true;
            pauseMenuUI.SetActive(true);
            Cursor.visible = true;
        }
    }

    void OnGameLoading()
    {
        loadingUI.SetActive(true);
    }

    public void DisplayLoadingUI()
    {
        loadingUI.SetActive(true);
    }

    void OnGameOver(object sender, GameOverEventArgs args)
    {
        Cursor.visible = true;
        gameOverUI.SetActive(true);
        gameOverUI.GetComponent<GameOverUI>().setCond(args.cond);

        GameEvents.DisplayCrafting -= OnCraftingDisplay;
        GameEvents.HideCrafting -= OnCraftingHide;
        GameEvents.DisplayLevelUpUI -= OnLevelUp;
        GameEvents.PauseGame -= OnGamePaused;
        GameEvents.EndGame -= OnGameOver;
    }
}
