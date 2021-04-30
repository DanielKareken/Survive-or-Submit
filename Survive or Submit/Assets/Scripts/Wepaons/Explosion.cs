using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Animator anim;
    public Collider2D hitbox;
    public int aoeDamage;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        hitbox = GetComponent<Collider2D>();
    }

    public void Deactivate()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().takeDamage(aoeDamage);
        }
    }
}
