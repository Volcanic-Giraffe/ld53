using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] float PullToCenter = 0.1f;
    [SerializeField] float StartImpulse = 10f;

    private Ship ship;

    void Start()
    {
        ship = Objects.Instance.Ship;
    }

    void FixedUpdate()
    {
        ship.RB.AddForce(PullToCenter * Time.fixedDeltaTime * -ship.transform.position);
    }
}
