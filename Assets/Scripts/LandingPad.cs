using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LandingPad : MonoBehaviour
{
    [SerializeField] private Transform mountPoint;

    [SerializeField] private float refuelRate; // per second
    
    private Ship _ship;
    private bool _shipLanded;
    private bool _visited;

    public Planet Planet { get; set; }
    
    private void Awake()
    {
        Objects.Instance.AddLandingPad(this);
    }
    
    private void Update()
    {
        if (LevelScenario.IsCompleted) return;
        
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Deploy();
        }

        if (_ship != null)
        {
            _ship.Refuel(refuelRate * Time.deltaTime);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_visited) return;
        
        if (other.CompareTag("Ship"))
        {
            var ship = other.GetComponentInParent<Ship>();
            _ship = ship;

            _ship.RB.isKinematic = true;
            _ship.Dock(false);

            _visited = true;

            _ship.ShipSounds.PlayRandom("land_b");
            
            _ship.transform.DOMove(mountPoint.position, Consts.LandingPadLandingTime).OnComplete(() =>
            {
                _shipLanded = true;
                
                LevelScenario.Instance.DeliveryMade(this);
                
                MarkersPanelUI.Instance.RemoveMarker(transform);
                
            });

            _ship.transform.DORotateQuaternion(mountPoint.rotation, Consts.LandingPadLandingTime * 0.5f);
        }
    }
    
    public void Deploy()
    {
        if (_ship == null) return;
        if (!_shipLanded) return;

        
        _ship.ShipSounds.PlayRandom("launch_b");
        
        _ship.RB.isKinematic = false;
        _ship.Launch();
        _ship.RB.AddForce(_ship.transform.forward * Consts.LandingPadLaunchForce, ForceMode.VelocityChange);
        _ship = null;
        _shipLanded = false;
    }

    private void OnDestroy()
    {
        if (Objects.Instance != null)
        {
            Objects.Instance.RemoveLandingPad(this);
        }
    }
}
