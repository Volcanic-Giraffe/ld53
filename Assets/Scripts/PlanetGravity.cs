using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGravity : MonoBehaviour
{
    [SerializeField] float Gravity = 5f;

    private Rigidbody ship;

    private void Awake()
    {
        ship = FindObjectOfType<Ship>().RB;
    }

    void Start()
    {

    }

    void FixedUpdate()
    {
        Vector3 dist = transform.position - ship.position;
        Vector3 dir = dist.normalized;
        ship.AddForce((Time.fixedDeltaTime * dir * Gravity) / dist.magnitude);
    }
}
