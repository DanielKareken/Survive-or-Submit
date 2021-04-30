using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    [SerializeField] RuntimeData runtimeData;
    [SerializeField] WeaponContainer[] weaponContainer;

    public GameObject[] weaponSlots;
    public GameObject[] weaponNames;
    public GameObject craftButton;
    public GameObject ammoCostText;
    public GameObject medCostText;
    public GameObject multiplierText;
    public GameObject ammoImage;
    public GameObject weaponImage;
    public GameObject nicknameText;
    public GameObject damageText;
    public GameObject fireRateText;
    public GameObject magCapacityText;
    public GameObject costPerShotText;
    public GameObject accuracyText;
    public GameObject reloadSpeedText;
    public GameObject specialText;
    public GameObject descriptionText;
    public int medCost;
    public int ammoCost; //does not need to be initialized

    bool[] isUnlocked;
    Text mCostText;
    Text aCostText;
    Text multiText;
    int lastSelectedIndex; //stores index of currently selected weapon
    int currIndex; //stores index of last clicked button
    int[] multiplier;
    int multiIndex; //points to an element in multiplier   

    // Start is called before the first frame update
    void Start()
    {
        //get components
        aCostText = ammoCostText.GetComponent<Text>();
        mCostText = medCostText.GetComponent<Text>();
        multiText = multiplierText.GetComponent<Text>();
       
        //init vars
        multiplier = new int[] {1, 5, 10, 25, 100};
        multiIndex = 0;
        multiText.text = "x" + multiplier[multiIndex];
        aCostText.text = "0";
        mCostText.text = "" + medCost * multiplier[multiIndex];
        isUnlocked = new bool[] { true, false, false, false, false, false, false, false, false};
        currIndex = 0;
        lastSelectedIndex = currIndex;

        foreach (GameObject text in weaponNames)
        {
            text.GetComponent<Text>().text = "CLASSIFIED";
        }
        weaponNames[0].GetComponent<Text>().text = "Axe";

        //events
        GameEvents.UpdateWeaponUI += OnWeaponUpdated;
        GameEvents.EndGame += OnGameOver;
    }

    // Update is called once per frame
    void Update()
    {
        
    }    

    //function to calcualte total scrap cost of craft
    int computeCost(int amount, int costPerItem)
    {
        int cost = (amount * costPerItem);
        return cost;
    }  

    //On button pressed, craft the amount specified by multiplier for given weapon
    //iff valid recources are available
    public void CraftAmmo()
    {
        //make sure weapon selected is unlocked
        if (isUnlocked[currIndex])
        {
            int costToCraft = computeCost(multiplier[multiIndex], ammoCost);

            //check if player has enough scrap
            if (costToCraft > runtimeData.player_scrap)
            {
                print("Not enough scrap");
            }
            else
            {
                runtimeData.player_scrap -= costToCraft;
                GameEvents.InvokeCraftAmmo(multiplier[multiIndex]);
                GameEvents.InvokeUpdateWeapon(currIndex);
                GameEvents.InvokeUpdateInventory();
            }
        }
    }

    //On craft button pressed, craft the amount specified by multiplier for meds
    //iff valid recources are available
    public void CraftMeds()
    {
        int costToCraft = computeCost(multiplier[multiIndex], medCost);

        if (costToCraft > runtimeData.player_scrap)
        {
            print("Not enough scrap");
        }
        else
        {
            runtimeData.player_scrap -= costToCraft;
            runtimeData.player_meds += multiplier[multiIndex];
            GameEvents.InvokeUpdateInventory();
        }
    }

    //toggle multiplier
    public void toggleMultiplier()
    {
        if(multiIndex == 4)
        {
            multiIndex = 0;
        }
        else
        {
            multiIndex++;
        }
        updateUI();
    }
    
    //change values displayed when multiplier changed
    void updateUI()
    {
        multiText.text = "x" + multiplier[multiIndex];
        aCostText.text = "" + ammoCost * multiplier[multiIndex];
        mCostText.text = "" + medCost * multiplier[multiIndex];
    }

    //on weapon clicked, equip weapon and update neccessary variables
    public void SelectWeapon(int index)
    {
        if (currIndex != index)
        {
            //new weapon select + weapon unlocked
            if (isUnlocked[index])
            {
                HighlightButton(index); //display weapon is selected in UI to player
                GameEvents.InvokeUpdateWeapon(index); //will send to Player first to get weapon GameObject, since this class wont have access to it

                //update UI elements
                UpdateUI(index);
            }
            //new weapon selected + weapon locked
            else
            {
                weaponNames[index].GetComponent<Text>().text = "Cost: " + weaponContainer[index].cost;
                int max = weaponContainer.Length - 1;

                //update UI elements
                UpdateUI(max);
                damageText.GetComponent<Text>().text = "Damage: ???";
                reloadSpeedText.GetComponent<Text>().text = "Reload Speed: ???";
                costPerShotText.GetComponent<Text>().text = "Ammo Cost: ???";
                accuracyText.GetComponent<Text>().text = "Accuracy: ???";
                magCapacityText.GetComponent<Text>().text = "Mag Size: ???";
                fireRateText.GetComponent<Text>().text = "Fire rate: ???";
            }

            if (!isUnlocked[currIndex])
            {
                weaponNames[currIndex].GetComponent<Text>().text = "CLASSIFIED";
            }

            currIndex = index;
        }
        else
        {
            //same weapon clicked again + weapon locked
            if (!isUnlocked[index])
            {
                //player has enough scrap: unlock weapon
                if (runtimeData.player_scrap >= weaponContainer[index].cost)
                {
                    HighlightButton(index); //display weapon is selected in UI to player
                    runtimeData.player_scrap -= weaponContainer[index].cost;
                    isUnlocked[index] = true;

                    GameEvents.InvokeUpdateWeapon(index); //will send to Player first to get weapon GameObject, since this class wont have access to it
                    GameEvents.InvokeUpdateInventory();

                    //print("Sending to weapon at index " + index);

                    //update UI elements
                    weaponNames[index].GetComponent<Text>().text = weaponContainer[index].weaponName;
                    UpdateUI(index);
                }
            }
        }
    }

    //display what weapon is currently selected in UI and active for player
    void HighlightButton(int index)
    {
        weaponSlots[index].GetComponent<Image>().color = Color.green;
        weaponSlots[lastSelectedIndex].GetComponent<Image>().color = Color.black;
        
        lastSelectedIndex = index;
    }

    //first part of updating UI elements
    void UpdateUI(int index)
    {
        ammoImage.GetComponent<Image>().sprite = weaponContainer[index].ammoImage;
        weaponImage.GetComponent<Image>().sprite = weaponContainer[index].weaponSideView;
        nicknameText.GetComponent<Text>().text = weaponContainer[index].nickname;
        specialText.GetComponent<Text>().text = "Special: " + weaponContainer[index].special;
        descriptionText.GetComponent<Text>().text = weaponContainer[index].description;
    }

    //now it can use the weapon to get the rest of the stats
    void OnWeaponUpdated(object sender, WeaponEventArgs args)
    {
        Weapon weapon = args.weapon.GetComponent<Weapon>();
        ammoCost = args.weapon.GetComponent<Weapon>().getAmmoCost();

        damageText.GetComponent<Text>().text = "Damage: " + weapon.damagePerShot;
        reloadSpeedText.GetComponent<Text>().text = "Reload Speed: " + weapon.reloadSpeed;
        costPerShotText.GetComponent<Text>().text = "Ammo Cost: " + ammoCost;
        accuracyText.GetComponent<Text>().text = "Accuracy: " + weapon.accuracy;
        magCapacityText.GetComponent<Text>().text = "Mag Size: " + weapon.magSize;
        fireRateText.GetComponent<Text>().text = "Fire rate: " + weapon.fireRate + "/s";        
    }

    void OnGameOver(object sender, GameOverEventArgs args)
    {
        GameEvents.UpdateWeaponUI -= OnWeaponUpdated;
        GameEvents.EndGame -= OnGameOver;
    }
}
