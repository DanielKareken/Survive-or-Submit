using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    [SerializeField] RuntimeData runtimeData;

    public GameObject[] weapons;
    public GameObject craftButton;
    public GameObject ammoSlider;
    public GameObject medSlider;
    public GameObject ammoText;
    public GameObject medText;
    public GameObject totalCostText;
    public Camera cam;

    int ammoToCraft;
    int medsToCraft;
    Slider aSlider;
    Slider mSlider;
    Text aText;
    Text mText;
    Text costText;
    int currCost;

    // Start is called before the first frame update
    void Start()
    {
        //assign Camera to weapon slots for handling mouse interactions
        foreach (GameObject weapon in weapons)
        {
            weapon.GetComponent<CraftWeaponButton>().setCam(cam);
        }

        aSlider = ammoSlider.GetComponent<Slider>();
        mSlider = medSlider.GetComponent<Slider>();
        aText = ammoText.GetComponent<Text>();
        mText = medText.GetComponent<Text>();
        costText = totalCostText.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        ammoToCraft = (int)aSlider.value;
        medsToCraft = (int)mSlider.value;
        aText.text = "" + ammoToCraft;
        mText.text = "" + medsToCraft;
        currCost = computeCost(ammoToCraft, medsToCraft);
        costText.text = "Scrap cost: " + currCost;
    }

    //function to calcualte total scrap cost of current craft based on weapon type
    int computeCost(int aVal, int mVal)
    {
        int cost = aVal + mVal;
        return cost;
    }

    //On craft button pressed, craft the amount specified by slider for given weapon
    //iff valid recources are available
    public void Craft()
    {
        if(currCost > runtimeData.player_scrap)
        {
            print("Not enough scrap");
        }
        else
        {
            runtimeData.player_scrap -= currCost;
        }
    }
}
