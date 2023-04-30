using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ship : MonoBehaviour
{
    [SerializeField] public float FuelMax;
    [SerializeField] public float Fuel;

    [SerializeField] public float HealthMax;
    [SerializeField] public float Health;
    
    public bool Died { get; set; }
    
    public float FuelRatio => FuelMax > 0 ? Fuel / FuelMax : 0f;
    public float HealthRatio => HealthMax > 0 ? Health / HealthMax : 0f;
    
    private Rigidbody rb;

    public Rigidbody RB { get => rb; }
    public Planet ClosestPlanet { get => _closestPlanet; }
    private bool _standby = true;
    public bool Standby => _standby;

    [SerializeField] float VelocityLimit = 20f;
    private Planet _closestPlanet;

    private float _invulnerabilityTimer;

    public float Velocity => rb.velocity.magnitude;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Generator.Instance.OnGenerationDone += Instance_OnGenerationDone;

        Objects.Instance.Ship = this;
    }

    public void Launch()
    {
        _standby = false;
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
        if (other.impulse.magnitude > 3f)
        {
            var dmgFormula = 10f + other.impulse.magnitude * 0.2f;

            DoDamage(dmgFormula);
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
