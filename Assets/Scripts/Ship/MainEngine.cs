using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MainEngine : MonoBehaviour
{
    public Ship Ship { get; private set; }
    public Rigidbody RB { get; private set; }

    [SerializeField] float EngineImpulse = 300f;
    [SerializeField, Tooltip("How much time till full thrust")] float ThrustAccelerationTime = 1f;
    [SerializeField, Tooltip("How thrust will grow over acc. time")] AnimationCurve ThrustAccelerationProfile;
    private bool _thrust;
    private float _thrustTimer;
    [SerializeField] private int MouseButton;
    [SerializeField] private KeyCode Button;

    [SerializeField] private EngineFx[] engineFx;

    // Start is called before the first frame update
    void Awake()
    {
        Ship = GetComponentInParent<Ship>();
        RB = GetComponentInParent<Rigidbody>();
    }

    public float CurrentThrustPower => ThrustAccelerationProfile.Evaluate(_thrustTimer / ThrustAccelerationTime);

    private void Update()
    {
        if (Ship.Died)
        {
            if (_thrust) StopThrust();
            return;
        }

        if (LevelScenario.IsCompleted)
        {
            if (_thrust) StopThrust();
            return;
        }

        if (Input.GetMouseButtonDown(MouseButton) && Ship.Fuel <= 0f)
        {
            Ship.ShipSounds.PlayRandom("chirp_a");
        }
        
        if ((Input.GetMouseButton(MouseButton) || Input.GetKey(Button)) && Ship.Fuel > 0f && !Ship.Standby)
        {
            DoThrust();
        }
        else
        {
            if (Ship.Fuel <= 0 && _thrust)
            {
                Ship.ShipSounds.PlayRandom("chirp_a");
            }
            
            StopThrust();
        }
        
        
        foreach(var ef in engineFx) ef.SetRatio(Ship.FuelRatio);
        
        if (_thrust) _thrustTimer = Mathf.Min(_thrustTimer + Time.deltaTime, ThrustAccelerationTime);
        else { _thrustTimer = 0f; }
    }

    private void StopThrust()
    {
        _thrust = false;

        foreach(var ef in engineFx) ef.Stop();
    }

    private void DoThrust()
    {
        _thrust = true;

        foreach (var ef in engineFx) ef.Start();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Ship.Fuel -= CurrentThrustPower * 2f * (1f + _thrustTimer) * Time.deltaTime;
        RB.AddForce(transform.forward * CurrentThrustPower * EngineImpulse * Time.fixedDeltaTime, ForceMode.Acceleration);
    }
}
