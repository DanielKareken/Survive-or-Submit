using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public Camera cam;
    public Player player;
    public Rigidbody2D rb;

    Vector2 mousePos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 offset = new Vector2(0.5f, 0);
        //get location of mouse on screen
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePos;

        //look at player
        Vector2 playerPos = player.transform.position;
        Vector2 lookDir = playerPos - rb.position;
        float lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 270f;
        rb.rotation = lookAngle;
    }
}
