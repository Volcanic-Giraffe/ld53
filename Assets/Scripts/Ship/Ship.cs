using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ship : MonoBehaviour
{
    [SerializeField] public string Code;
    
    [SerializeField] public float FuelMax;
    [SerializeField] public float Fuel;

    [SerializeField] public float HealthMax;
    [SerializeField] public float Health;

    [Space]
    [SerializeField] public string Title;
    [TextArea]
    [SerializeField] public string Description;
    [TextArea]
    [SerializeField] public string Controls;

    public bool Died { get; set; }
    
    public float FuelRatio => FuelMax > 0 ? Fuel / FuelMax : 0f;
    public float HealthRatio => HealthMax > 0 ? Health / HealthMax : 0f;
    
    private Rigidbody rb;

    public Rigidbody RB { get => rb; }
    public Planet ClosestPlanet { get => _closestPlanet; }
    private bool _standby = true;
    public bool Standby => _standby;
    private bool _cameraInAss = true;
    public bool CameraInAss => _cameraInAss;

    [SerializeField] float VelocityLimit = 20f;
    private Planet _closestPlanet;

    private float _invulnerabilityTimer;

    public float Velocity => rb.velocity.magnitude;

    public float VelocityRatio => Velocity > 0 ? Velocity / 20f : 0f;
    
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (Generator.Instance != null)
        {
            Generator.Instance.OnGenerationDone += Instance_OnGenerationDone;
        }

        Objects.Instance.Ship = this;
    }

    public void Launch()
    {
        _standby = false;
        _cameraInAss = false;
    }
    
    public void Dock(bool resetCamera = true)
    {
        _standby = true;
        _cameraInAss = resetCamera;
    }

    private void Instance_OnGenerationDone()
    {
        _closestPlanet = Objects.Instance.Planets[0];
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (ClosestPlanet != null)
        {
            var size = Consts.PlanetDiameter * 1.2f;
            Gizmos.DrawSphere(ClosestPlanet.transform.position + Vector3.up * size, 1f);
            Gizmos.DrawSphere(ClosestPlanet.transform.position - Vector3.up * size, 1f);
            Gizmos.DrawSphere(ClosestPlanet.transform.position + Vector3.right * size, 1f);
            Gizmos.DrawSphere(ClosestPlanet.transform.position - Vector3.right * size, 1f);
            Gizmos.DrawSphere(ClosestPlanet.transform.position + Vector3.forward * size, 1f);
            Gizmos.DrawSphere(ClosestPlanet.transform.position - Vector3.forward * size, 1f);
        }
    }
#endif 

    private void Update()
    {
        FindClosestPlanet();

        if (_invulnerabilityTimer > 0) _invulnerabilityTimer -= Time.deltaTime;
    }

    private void FindClosestPlanet()
    {
        if (_standby) return;
        var minDist = 8000f;
        foreach (var planet in Objects.Instance.Planets)
        {
            var pDist = Vector3.Distance(transform.position, planet.transform.position);
            if (pDist < minDist)
            {
                minDist = pDist;
                _closestPlanet = planet;
            }
        }
    }

    void FixedUpdate()
    {
        if (RB.velocity.magnitude > VelocityLimit) RB.velocity = RB.velocity.normalized * VelocityLimit;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.impulse.magnitude > 1f)
        {
            var dmgFormula = 10f + other.impulse.magnitude * 1.5f;

            DoDamage(dmgFormula);

            LevelVolume.Instance.HitEffect(dmgFormula * 0.05f);
        }
    }

    private void DoDamage(float amount)
    {
        if (_invulnerabilityTimer > 0) return;
        _invulnerabilityTimer = 0.1f;
        
        Health -= amount;
        Health = Mathf.Clamp(Health, 0f, HealthMax);

        if (Health <= 0)
        {
            Died = true;
        }
    }

    public void Heal(float amount)
    {
        Health += amount;
        Health = Mathf.Clamp(Health, 0f, HealthMax);
    }
    
    public void Refuel(float amount)
    {
        Fuel += amount;
        Fuel = Mathf.Clamp(Fuel, 0f, FuelMax);
    }
}
