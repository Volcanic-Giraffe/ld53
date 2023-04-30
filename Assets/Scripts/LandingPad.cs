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

            ship.RB.isKinematic = true;

            _ship = ship;
            _visited = true;

            ship.transform.DOMove(mountPoint.position, Consts.LandingPadLandingTime).OnComplete(() =>
            {
                _shipLanded = true;
                Planet.SetColor(Color.green);
                
                LevelScenario.Instance.DeliveryMade(this);
                
            });

            ship.transform.DORotateQuaternion(mountPoint.rotation, Consts.LandingPadLandingTime * 0.5f);
        }
    }
    
    public void Deploy()
    {
        if (_ship == null) return;
        if (!_shipLanded) return;

        _ship.RB.isKinematic = false;
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
