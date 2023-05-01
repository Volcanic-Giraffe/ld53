using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipContainer : MonoBehaviour
{
    private bool _spinning;
    public Ship Ship { get; private set; }

    public float _currentSpeedMod;
    public float _targetSpeedMod;
    
    private void Awake()
    {
        Ship = GetComponentInChildren<Ship>();
    }

    private void Update()
    {
        _currentSpeedMod = Mathf.Lerp(_currentSpeedMod, _targetSpeedMod, Time.deltaTime * 5f);
        
        if (_currentSpeedMod > 0f)
        {
            transform.Rotate(Vector3.up * (100f * Time.deltaTime * _currentSpeedMod));
        }
    }

    public void SetSpinning(bool spinning)
    {
        if (spinning)
        {
            _targetSpeedMod = 1.0f;
        }
        else
        {
            _targetSpeedMod = 0f;
        }
    }
}
