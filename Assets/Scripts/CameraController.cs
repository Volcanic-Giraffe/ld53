using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Ship ship;

    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float tiltSpeed = 7f;
    [SerializeField] private float followDistance = 10f;
    [SerializeField] private float mouseControlSpeed = 5f;
    private float timer = 0f;
    void Start()
    {
        ship = Objects.Instance.Ship;
    }

    // Update is called once per frame
    void Update()
    {
        OrientToVelocity();
        //OrientByMouse();
        transform.LookAt(ship.transform);
    }

    private void OrientByMouse()
    {
        var mouseInput = new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y")) * mouseControlSpeed;
        var currentDirection = (transform.position - ship.transform.position).normalized;
        var newDirection = Quaternion.AngleAxis(mouseInput.y, transform.right) * Quaternion.AngleAxis(mouseInput.x, transform.up) * currentDirection;
        var targetPos = ship.transform.position + (newDirection * followDistance);
        transform.position = targetPos;
    }

    private void OrientToVelocity()
    {
        //if (ship.RB.velocity.magnitude > 1f) timer += Time.deltaTime;
        //else timer -= Time.deltaTime;
        //timer = Mathf.Clamp01(timer);
        if (ship.RB.velocity.magnitude > 1f)
        {
            var direction = ship.RB.velocity.normalized;
            if (direction == Vector3.zero) direction = transform.forward;
            var targetPos = ship.transform.position - (direction * followDistance);
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * tiltSpeed);
        }
        //transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Time.deltaTime * rotationSpeed);
    }
}
