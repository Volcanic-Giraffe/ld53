using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GyroControl : MonoBehaviour
{
    private Rigidbody rb;
    private Ship _ship;

    public Rigidbody RB { get => rb; }
    [SerializeField] float GyroPower = 5f;
    private Vector2 _rotation;

    private CameraController _cam;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _ship = GetComponent<Ship>();
        _cam = FindObjectOfType<CameraController>();
    }

    private void Update()
    {
        if (_ship.Standby) return;
        _rotation = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RB.AddTorque(_cam.transform.up * _rotation.x * GyroPower * Time.fixedDeltaTime, ForceMode.Acceleration);
        RB.AddTorque(-_cam.transform.right * _rotation.y * GyroPower * Time.fixedDeltaTime, ForceMode.Acceleration);
    }
}
