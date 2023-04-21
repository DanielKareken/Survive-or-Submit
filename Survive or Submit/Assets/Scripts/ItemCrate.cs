using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CrateType { WeaponCache, MedBag }

public class ItemCrate : MonoBehaviour
{
    [SerializeField] private GameObject _visuals;
    [SerializeField] private float _minDrop;
    [SerializeField] private float _maxDrop;

    public CrateType crateType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //return random number for drop amount
    public int GetItemAmount()
    {
        return (int)Random.Range(_minDrop, _maxDrop);
    }
}
