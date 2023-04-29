using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MainEngine : MonoBehaviour
{
    private Rigidbody rb;
    public Rigidbody RB { get => rb; }
    [SerializeField] float EngineImpulse = 300f;
    [SerializeField, Tooltip("How much time till full thrust")] float ThrustAccelerationTime = 1f;
    [SerializeField, Tooltip("How thrust will grow over acc. time")] AnimationCurve ThrustAccelerationProfile;
    private bool _thrust;
    private float _thrustTimer;
    [SerializeField] private int MouseButton;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public float CurrentThrustPower => ThrustAccelerationProfile.Evaluate(_thrustTimer / ThrustAccelerationTime);

    private void Update()
    {
        if (Input.GetMouseButton(MouseButton))
        {
            DoThrust();
        }
        else
        {
            StopThrust();
        }
        if (_thrust) _thrustTimer = Mathf.Min(_thrustTimer + Time.deltaTime, ThrustAccelerationTime);
        else { _thrustTimer = 0f; }
    }

    private void StopThrust()
    {
        _thrust = false;
    }

    private void DoThrust()
    {
        _thrust = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RB.AddForce(transform.forward * CurrentThrustPower * EngineImpulse * Time.fixedDeltaTime, ForceMode.Acceleration);
    }
}
