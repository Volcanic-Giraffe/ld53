using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FuelPickup : MonoBehaviour
{
    [SerializeField] private float fuelAmount;
    [SerializeField] private Transform artGroup;
    
    private bool _collected;

    private void Update()
    {
        artGroup.Rotate(Vector3.up * (30f * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_collected) return;
        
        if (other.CompareTag("Ship"))
        {
            _collected = true;
            
            var ship = other.GetComponentInParent<Ship>();

            transform.DOMove(ship.transform.position, 0.23f);
            transform.DOScale(0.1f, 0.23f).OnComplete(() =>
            {
                ship.Refuel(fuelAmount);
                Destroy(gameObject,0.01f);
            });
        }
    }
}
