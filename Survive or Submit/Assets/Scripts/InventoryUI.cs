using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] Text scrapText;
    [SerializeField] Text medText;
    [SerializeField] HealthBar healthBar;

    int medAmount;
    int scrapAmount;
    int health;

    // Start is called before the first frame update
    void Start()
    {
        //temporarily initialize
        medAmount = 0;
        scrapAmount = 10;

        //subscribe to event
        GameEvents.UpdatePlayerHUD += OnUpdated;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnUpdated(object sender, PlayerHUDEventArgs args)
    {
        switch (args.type)
        {
            //update scrap count
            case 1:
                scrapAmount = args.newAmount;
                break;
            //update med count
            case 2:
                medAmount = args.newAmount;
                break;
            //update health
            case 3:
                health = args.newAmount;
                break;
            //this should never ever run
            //default:
                //print("Invalid type passed to Inventory UI for OnUpdated()");
        }

        scrapText.text = "" + scrapAmount;
        medText.text = "" + medAmount;
        healthBar.SetHealth(health);
    }
}
