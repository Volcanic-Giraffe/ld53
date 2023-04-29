using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Ship ship;
    
    void Start()
    {
        ship = FindObjectOfType<Ship>();
    }

    // Update is called once per frame
    void Update()
    {
        var target = ship.transform;
        
        transform.position =  target.position - (target.forward * 10f);
        transform.transform.LookAt(target);
    }
}
