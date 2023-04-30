using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Ship ship;

    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float tiltSpeed = 7f;
    [SerializeField] private float followDistance = 10f;
    private float timer = 0f;
    void Start()
    {
        ship = FindObjectOfType<Ship>();
    }

    // Update is called once per frame
    void Update()
    {
        var target = ship.transform;

        //if (ship.RB.velocity.magnitude > 1f) timer += Time.deltaTime;
        //else timer -= Time.deltaTime;
        //timer = Mathf.Clamp01(timer);
        if (ship.RB.velocity.magnitude > 1f)
        {
            var direction = ship.RB.velocity.normalized;
            if (direction == Vector3.zero) direction = transform.forward;
            var targetPos = target.position - (direction * followDistance);
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * tiltSpeed);
        }
        //transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Time.deltaTime * rotationSpeed);

        transform.LookAt(target.transform);
    }
}
