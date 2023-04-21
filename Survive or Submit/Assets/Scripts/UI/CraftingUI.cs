using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    [Header("Object Refs")]
    [SerializeField] RuntimeData runtimeData;
    [SerializeField] WeaponContainer[] weaponContainer;
    [SerializeField] private WeaponHolder _weaponHolder;
    public GameObject weaponTabButton;
    public GameObject gearTabButton;
    public GameObject weaponImage;

    [Header("Weapon Crafting Tab")]
    public GameObject[] weaponSlots;

    [Header("User Options UI")]
    public GameObject craftButton;
    public GameObject ammoCostText;
    public GameObject medCostText;
    public GameObject multiplierText;
    public GameObject ammoImage;

    [Header("Item Stats UI")]
    public GameObject nicknameText;
    public GameObject damageBar;
    public GameObject fireRateBar;
    public GameObject magCapacityBar;
    public GameObject costPerShotBar;
    public GameObject accuracyBar;
    public GameObject reloadSpeedBar;
    public GameObject specialText;
    public GameObject descriptionText;

    [Header("Gear Crafting Tab")]
    public GameObject[] gearSlots;
    
    
    public int medCost;
    public int ammoCost; //does not need to be initialized

    bool[] isUnlocked;
    Text mCostText;
    Text aCostText;
    Text multiText;
    int currEquipedWeaponIndex; //stores index of currently selected weapon
    int lastSelectedButtonIndex; //stores index of last selected button
    int currSelectedButtonIndex; //stores index of just clicked button
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
        currSelectedButtonIndex = 0;
        currEquipedWeaponIndex = 0;
        lastSelectedButtonIndex = currSelectedButtonIndex;

        //events
        GameEvents.UpdateWeaponUI += OnWeaponUpdated;
        GameEvents.EndGame += OnGameOver;

        //initialize weapons
        foreach(GameObject weaponSlot in weaponSlots)
        {
            weaponSlot.GetComponent<Image>().color = new Color(0.1f, 0.1f, 0.1f, 0.5f); //transparent black
            weaponSlot.GetComponent<WeaponButton>().SetName("CLASSIFIED");
        }
        weaponSlots[0].GetComponent<Image>().color = new Color(0, 1, 0, 0.5f); //transparent green
        weaponSlots[0].GetComponent<WeaponButton>().SetName(weaponContainer[0].weaponName);
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
        if (isUnlocked[currEquipedWeaponIndex])
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
                _weaponHolder.AddAmmo(currEquipedWeaponIndex, multiplier[multiIndex]);
                //GameEvents.InvokeUpdateWeapon(currEquipedWeaponIndex);
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

    //on weapon clicked, update UI and select it
    public void SelectWeapon(int index)
    {
        if (currSelectedButtonIndex != index)
        {
            lastSelectedButtonIndex = currSelectedButtonIndex;

            //new weapon select + weapon unlocked
            if (isUnlocked[index])
            {
                //update UI elements
                UpdateUI(index);
                weaponSlots[index].GetComponent<Image>().color = new Color(0, 1, 0, 0.5f); //transparent green
                if (index != currEquipedWeaponIndex)
                {
                    weaponSlots[currEquipedWeaponIndex].GetComponent<Image>().color = new Color(0.1f, 0.1f, 0.1f, 0.5f); //transparent black
                }
                
                //GameEvents.InvokeUpdateWeapon(index); //will send to Player first to get weapon GameObject, since this class wont have access to it
                currEquipedWeaponIndex = index;
            }
            //new weapon selected + weapon locked
            else
            {
                //weapon is unlockable
                if (index <= runtimeData.weapons_unlocked - 1)
                {
                    //update UI elements
                    UpdateUI(index);
                    weaponSlots[index].GetComponent<WeaponButton>().SetName(weaponContainer[index].weaponName); 
                    weaponSlots[index].GetComponent<Image>().color = new Color(0, 1, 0, 0.5f); //transparent green
                    weaponSlots[currEquipedWeaponIndex].GetComponent<Image>().color = new Color(0.1f, 0.1f, 0.1f, 0.5f); //transparent black            

                    //GameEvents.InvokeUpdateWeapon(index); //will send to Player first to get weapon GameObject, since this class wont have access to it
                    GameEvents.InvokeUpdateInventory();
                    isUnlocked[index] = true;
                    currEquipedWeaponIndex = index;
                }
                //weapon is not unlockable
                else
                {
                    weaponSlots[index].GetComponent<WeaponButton>().SetName("Error: Weapon Not found");
                    int max = weaponContainer.Length - 1;

                    //update UI elements
                    UpdateUI(max);
                    //GameEvents.InvokeUpdateWeapon(index); //will send to Player first to get weapon GameObject, since this class wont have access to it
                }
            }

            //if last button was locked, replace name with "CLASSIFIED"
            if (!isUnlocked[lastSelectedButtonIndex])
            {
                weaponSlots[lastSelectedButtonIndex].GetComponent<WeaponButton>().SetName("CLASSIFIED");
            }

            currSelectedButtonIndex = index;
        }
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

        updateItemStatsUI(weapon);          
    }

    void OnGameOver(object sender, GameOverEventArgs args)
    {
        GameEvents.UpdateWeaponUI -= OnWeaponUpdated;
        GameEvents.EndGame -= OnGameOver;
    }

    //update display of stats for item
    void updateItemStatsUI(Weapon weapon)
    {
        damageBar.GetComponent<Slider>().value = weapon.damagePerShot;
        reloadSpeedBar.GetComponent<Slider>().value = 1 / weapon.reloadSpeed;
        costPerShotBar.GetComponent<Slider>().value = ammoCost;
        accuracyBar.GetComponent<Slider>().value = 100 - weapon.accuracy;
        magCapacityBar.GetComponent<Slider>().value = weapon.magSize;
        fireRateBar.GetComponent<Slider>().value = weapon.fireRate;   
    }
}
