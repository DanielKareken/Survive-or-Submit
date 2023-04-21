using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _loadingText;
    [SerializeField] Slider _loadingSlider;

    // Start is called before the first frame update
    void Start()
    {
        //game events
        GameEvents.UpdateLoadingUI += OnUpdateLoadingUI;

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnUpdateLoadingUI(object sender, UpdateLoadingUIArgs args)
    {
        _loadingSlider.maxValue = args.targetVal;
        _loadingSlider.value = args.curValue;
        _loadingText.text = args.message + "...";
    }
}
