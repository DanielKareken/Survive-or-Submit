using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public Collider2D hitbox;
    public int aoeDamage;
    public float aoeRange;
    public LayerMask targetLayers;

    // Start is called before the first frame update
    void Start()
    {
        hitbox = GetComponent<Collider2D>();

        //get targets in range
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, aoeRange);

        //damage all hit targets
        foreach (var collider in hitColliders)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                damageTarget(collider);
            }
        }
    }

    public void Deactivate()
    {
        Destroy(gameObject);
    }
    
    //apply damage to target health
    void damageTarget(Collider2D collision)
    {
        collision.gameObject.GetComponent<Enemy>().takeDamage(aoeDamage);
    }
}
