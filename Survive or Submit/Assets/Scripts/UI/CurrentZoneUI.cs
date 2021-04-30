using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentZoneUI : MonoBehaviour
{
    public Text zoneText;

    // Start is called before the first frame update
    void Start()
    {
        zoneText.text = "Safehouse";

        GameEvents.UpdateZoneUI += OnZoneUpdated;
        GameEvents.EndGame += OnGameOver;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnZoneUpdated(object sender, ZoneEventArgs args)
    {
        zoneText.text = args.zone;
    }

    void OnGameOver(object sender, GameOverEventArgs args)
    {
        GameEvents.UpdateZoneUI -= OnZoneUpdated;
        GameEvents.EndGame -= OnGameOver;
    }
}