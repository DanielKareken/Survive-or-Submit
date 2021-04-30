using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VCamHelper : MonoBehaviour
{
    [SerializeField] Transform cursor;
    [SerializeField] Transform player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3((player.position.x + cursor.position.x) / 2, (player.position.y + cursor.position.y) / 2, player.position.z);
    }
}
