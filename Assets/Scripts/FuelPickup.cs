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

            
            var duration = 0.27f;
            Permanent.Sounds.PlayRandom("bonus_a");
            
            var tw  = transform.DOMove(ship.transform.position, duration);
            tw.OnUpdate(() =>
            {
                tw.ChangeEndValue(ship.transform.position, true);
            });
            
            transform.DOScale(0.1f, duration).OnComplete(() =>
            {
                ship.Refuel(fuelAmount);
                Destroy(gameObject,0.01f);
            });
        }
    }
}
