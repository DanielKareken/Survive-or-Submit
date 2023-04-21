using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelBar : MonoBehaviour
{
    [SerializeField] RuntimeData runtimeData;

    public GameObject fill;
    public GameObject border;
    public Slider slider;
    public Gradient gradient;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = runtimeData.player_exp_to_lvl_up;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = runtimeData.player_experience;
        
    }
}
