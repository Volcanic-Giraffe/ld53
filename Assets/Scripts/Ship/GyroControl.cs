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

    private float _stuckTimer;
    private float _initialGyroPower;

    // Start is called before the first frame update
    void Awake()
    {
        _initialGyroPower = GyroPower;
        rb = GetComponent<Rigidbody>();
        _ship = GetComponent<Ship>();
        _cam = FindObjectOfType<CameraController>();
    }

    private void Update()
    {
        if (_ship.Standby) return;
        if (LevelScenario.IsCompleted) return;
        
        _rotation = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 0.5f, LayerMask.GetMask("Planet")))
        {
            _stuckTimer += Time.deltaTime;
            if (_stuckTimer > 3f) GyroPower = _initialGyroPower * 10f;
        }
        else
        {
            _stuckTimer = 0;
            GyroPower = _initialGyroPower;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RB.AddTorque(_cam.transform.up * _rotation.x * GyroPower * Time.fixedDeltaTime, ForceMode.Acceleration);
        RB.AddTorque(-_cam.transform.right * _rotation.y * GyroPower * Time.fixedDeltaTime, ForceMode.Acceleration);
    }
}
