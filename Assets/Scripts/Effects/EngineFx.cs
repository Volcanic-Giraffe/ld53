using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EngineFx : MonoBehaviour
{
    [SerializeField] private ParticleSystem mainPs;
    [SerializeField] private AudioSource asThrust;

    private void Awake()
    {
        asThrust.Play();
    }

    public void Stop()
    {
        mainPs.Stop();
        
        asThrust.DOKill();
        asThrust.DOFade(0.0f, 0.13f);
    }

    public void Start()
    {
        mainPs.Play();

        asThrust.DOKill();
        asThrust.DOFade(0.5f, 0.23f);
    }

    private void OnDestroy()
    {
        asThrust.DOKill();
    }
}
