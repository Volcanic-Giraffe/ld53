using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PackagePickup : MonoBehaviour
{
    [SerializeField] private Transform artGroup;
    [SerializeField] private float fuelAmount;
    
    private bool _collected;

    private void Start()
    {
    }

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
            
            Permanent.Sounds.PlayRandom("pick_a");
            
            var tw = transform.DOMove(ship.transform.position, duration);
            
            tw.OnUpdate(() =>
            {
                tw.ChangeEndValue(ship.transform.position, true);
            });

            transform.DOScale(0.1f, duration).OnComplete(() =>
            {
                ship.Refuel(fuelAmount);
                
                LevelScenario.Instance.PackagePicked(this);                
                Destroy(gameObject,0.01f);
            });
        }
    }
}
