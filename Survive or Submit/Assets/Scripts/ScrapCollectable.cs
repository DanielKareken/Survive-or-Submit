using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapCollectable : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprite;

    public Rigidbody2D rb;

    int quantity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //gets the value to be assigned to drop from specific zombie
    public void setQuantity(int amount)
    {
        quantity = amount;
    }

    //for player collect
    public int getQuantity()
    {
        return quantity;
    }

    public void OnCollected()
    {
        //play any animations or sounds
        Destroy(gameObject);
    }
}
