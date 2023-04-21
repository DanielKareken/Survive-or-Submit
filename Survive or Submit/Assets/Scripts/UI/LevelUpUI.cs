using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : MonoBehaviour
{
    public Text levelUpText;
    public Text newUnlocksText;

    Animator anim;
    int level;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        level = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void levelUpAnimation(int newLevel, string newUnlocks, bool isCrate)
    {
        if(GetComponent<Animator>())
            anim = GetComponent<Animator>();

        if (isCrate)
        {
            levelUpText.text = "New weapon aquired";
            newUnlocksText.text = "";         
        }
        else
        {
            level = newLevel;
            levelUpText.text = "Progression Reached: Level " + level;
            newUnlocksText.text = "New Unlocks: " + newUnlocks;
        }
        anim.SetTrigger("leveled up");   
    }
}
