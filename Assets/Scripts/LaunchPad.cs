using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPad : MonoBehaviour
{
    private Ship _ship;
    
    public void Attach(Ship ship)
    {
        _ship = ship;

        _ship.transform.position = transform.position;
        _ship.transform.rotation = transform.rotation;

        _ship.RB.isKinematic = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Deploy();
        }
    }

    public void Deploy()
    {
        if (_ship == null) return;

        _ship.RB.isKinematic = false;
        _ship = null;
    }
}
