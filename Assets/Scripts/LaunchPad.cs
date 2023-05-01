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
        _ship.Dock();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Deploy();
        }
        
        if (_ship != null)
        {
            _ship.Refuel(5f * Time.deltaTime);
            _ship.Heal(10f * Time.deltaTime);
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
            _ship = ship;

            _ship.RB.isKinematic = true;
            _ship.Dock(true);

            _ready = false;
            
            _ship.transform.DOMove(mountPoint.position, Consts.LaunchPadLandingTime).OnComplete(() =>
            {
                LevelScenario.Instance.ReturnedToPad(this);
            });

            _ship.transform.DORotateQuaternion(mountPoint.rotation, Consts.LaunchPadLandingTime * 0.5f);
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
