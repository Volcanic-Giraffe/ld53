using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EngineFx : MonoBehaviour
{
    [SerializeField] private ParticleSystem mainPs;
    [SerializeField] private AudioSource asThrust;

    private float _rateOriginal;
    private float _pitchInitial;
    
    private void Awake()
    {
        _pitchInitial = asThrust.pitch;
        _rateOriginal = mainPs.main.startLifetime.constant;
        
        if (asThrust.enabled)
        {
            asThrust.Play();
        }
    }

    public void Stop()
    {
        mainPs.Stop();

        if (asThrust.enabled)
        {
            asThrust.DOKill();
            asThrust.DOFade(0.0f, 0.13f);
        }
    }

    public void Start()
    {
        mainPs.Play();

        if (asThrust.enabled)
        {
            asThrust.DOKill();
            asThrust.DOFade(0.5f, 0.23f);
        }
    }

    private void OnDestroy()
    {
        asThrust.DOKill();
    }

    public void SetRatio(float ratio)
    {
        if (ratio < 0.25)
        {
            asThrust.pitch = _pitchInitial * 1.5f;
            
            var emission =  mainPs.main;
            emission.startLifetime = _rateOriginal * 0.5f;
        }
        else
        {
            asThrust.pitch = _pitchInitial;
            
            var emission =  mainPs.main;
            emission.startLifetime = _rateOriginal;

        }
    }
}
