using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ship : MonoBehaviour
{
    private Rigidbody rb;
    private Planet[] _planets;

    public Rigidbody RB { get => rb; }
    public Planet ClosestPlanet { get => _closestPlanet;  }
    private bool _standby = true;
    public bool Standby => _standby;

    [SerializeField] float VelocityLimit = 20f;
    private Planet _closestPlanet;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Generator.Instance.OnGenerationDone += Instance_OnGenerationDone;
    }

    private void Instance_OnGenerationDone()
    {
        _planets = FindObjectsOfType<Planet>();
        _closestPlanet = _planets[0];
        _standby = false;
    }

    private void Update()
    {
        FindClosestPlanet();
    }

    private void FindClosestPlanet()
    {
        if (_standby) return;
        var minDist = 8000f;
        foreach (var planet in _planets)
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
}
