using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineFx : MonoBehaviour
{
    [SerializeField] private ParticleSystem mainPs;

    public void Stop()
    {
        mainPs.Stop();
    }

    public void Start()
    {
        mainPs.Play();
    }
}
