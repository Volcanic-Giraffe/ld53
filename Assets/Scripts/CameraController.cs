using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Ship ship;
    private Vector3 currentDirection;
    [SerializeField] private bool orientToVelocity = true;
    [SerializeField] private bool orientByMouse = true;
    [SerializeField] private float followDistance = 10f;
    [SerializeField] private float mouseControlSpeed = 5f;
    private float timer = 0f;
    void Start()
    {
        ship = Objects.Instance.Ship;
        currentDirection = (transform.position - ship.transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        if (ship.Standby)
        {
            transform.position = ship.transform.position - (ship.transform.forward * followDistance * 2f);
            transform.LookAt(ship.transform, transform.up);
            return;
        }
        if (orientToVelocity) DoOrientToVelocity();
        if(orientByMouse) DoOrientByMouse();
        var targetPos = ship.transform.position + (currentDirection * followDistance);
        transform.position = targetPos;
        transform.LookAt(ship.transform, transform.up);
    }

    private void DoOrientByMouse()
    {
        var mouseInput = new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y")) * mouseControlSpeed;
        currentDirection = Quaternion.AngleAxis(mouseInput.x, transform.up) * Quaternion.AngleAxis(mouseInput.y, transform.right) * currentDirection;
    }

    private void DoOrientToVelocity()
    {
        currentDirection = (transform.position - ship.transform.position).normalized;
    }
}
