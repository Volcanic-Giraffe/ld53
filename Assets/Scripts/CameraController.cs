using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Ship ship;

    private float rotationSpeed = 5f;
    private float tiltSpeed = 7f;
    
    void Start()
    {
        ship = FindObjectOfType<Ship>();
    }

    // Update is called once per frame
    void Update()
    {
        var target = ship.transform;
        var targetPos = target.position - (target.forward * 10f);
        
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * tiltSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Time.deltaTime * rotationSpeed);
    }
}
