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
        transform.position = ship.transform.forward + Vector3.back * 10f;
        transform.LookAt(ship.transform);
    }
}
