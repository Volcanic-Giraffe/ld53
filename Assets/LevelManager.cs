using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] Ship ship;
    [SerializeField] float PullToCenter = 0.1f;
    [SerializeField] float StartImpulse = 10f;

    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ship.RB.AddForce(PullToCenter * Time.fixedDeltaTime * -ship.transform.position);
    }
}
