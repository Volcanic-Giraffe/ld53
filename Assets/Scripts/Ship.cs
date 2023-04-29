using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ship : MonoBehaviour
{
    private Rigidbody rb;
    public Rigidbody RB { get => rb; }
    [SerializeField] float VelocityLimit = 20f;
    [SerializeField] float Impulse = 10f;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RB.AddForce(Random.onUnitSphere * Impulse, ForceMode.VelocityChange);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (RB.velocity.magnitude > VelocityLimit) RB.velocity = RB.velocity.normalized * VelocityLimit;
    }
}
