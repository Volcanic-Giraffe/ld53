using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ship : MonoBehaviour
{
    private Rigidbody rb;

    public Rigidbody RB { get => rb; }
    public Planet ClosestPlanet { get => _closestPlanet; }
    private bool _standby = true;
    public bool Standby => _standby;

    [SerializeField] float VelocityLimit = 20f;
    private Planet _closestPlanet;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Generator.Instance.OnGenerationDone += Instance_OnGenerationDone;

        Objects.Instance.Ship = this;
    }

    private void Instance_OnGenerationDone()
    {
        _closestPlanet = Objects.Instance.Planets[0];
        _standby = false;
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

}
