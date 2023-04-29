using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ship : MonoBehaviour
{
    private Rigidbody rb;
    public Rigidbody RB { get => rb; }
    [SerializeField] float VelocityLimit = 20f;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (RB.velocity.magnitude > VelocityLimit) RB.velocity = RB.velocity.normalized * VelocityLimit;
    }
}
