using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LaunchPad : MonoBehaviour
{
    [SerializeField] private Transform mountPoint;
    [SerializeField] float LaunchForce = 200f;
    private Ship _ship;

    private bool _ready;

    private void Awake()
    {
        Objects.Instance.AddLaunchPad(this);
    }

    public void Attach(Ship ship)
    {
        _ship = ship;

        _ship.transform.position = transform.position;
        _ship.transform.rotation = transform.rotation;

        _ship.RB.isKinematic = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Deploy();
        }
    }

    public void Deploy()
    {
        if (_ship == null) return;

        _ship.RB.isKinematic = false;
        _ship.RB.AddForce(_ship.transform.forward * LaunchForce, ForceMode.VelocityChange);
        _ship.Launch();
        _ship = null;
        
        LevelScenario.Instance.DeployedFromLaunchPad();
    }

    public void SetReady(bool ready)
    {
        _ready = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_ready) return;
        
        if (other.CompareTag("Ship"))
        {
            var ship = other.GetComponentInParent<Ship>();

            ship.RB.isKinematic = true;

            _ship = ship;
            _ready = false;
            
            ship.transform.DOMove(mountPoint.position, Consts.LaunchPadLandingTime).OnComplete(() =>
            {
                ship.Refuel(100f);
                LevelScenario.Instance.ReturnedToPad(this);
            });

            ship.transform.DORotateQuaternion(mountPoint.rotation, Consts.LaunchPadLandingTime * 0.5f);
        }
    }
    
    private void OnDestroy()
    {
        if (Objects.Instance != null)
        {
            Objects.Instance.RemoveLaunchPad(this);
        }
    }
}
