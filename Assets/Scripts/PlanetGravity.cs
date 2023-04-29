using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGravity : MonoBehaviour
{
    [SerializeField] float Gravity = 5f;
    [SerializeField] private Transform shape;
    
    private Rigidbody ship;

    private void Awake()
    {
        ship = FindObjectOfType<Ship>().RB;

        shape.localScale *= Random.Range(0.9f, 2.5f);
        shape.rotation = Random.rotation;
    }

    void Start()
    {

    }

    void FixedUpdate()
    {
        Vector3 dist = transform.position - ship.position;
        Vector3 dir = dist.normalized;
        ship.AddForce((Time.fixedDeltaTime * dir * Gravity) / dist.sqrMagnitude);
    }
}
