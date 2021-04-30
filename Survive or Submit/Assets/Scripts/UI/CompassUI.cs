using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassUI : MonoBehaviour
{
    [SerializeField] Transform playerPos;
    [SerializeField] Transform spawnPos;
    [SerializeField] GameObject needle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PointAtSpawn();
    }

    void PointAtSpawn()
    {
        Vector2 lookDir = spawnPos.position - playerPos.position;
        float lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        needle.transform.rotation = Quaternion.Euler(0, 0, lookAngle);
    }
}
