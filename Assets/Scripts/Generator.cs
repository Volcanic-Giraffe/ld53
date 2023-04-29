using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Generator : MonoBehaviour
{
    public float Radius;
    public int Count;

    private void Start()
    {
        for (int i = 0; i < Count; i++)
        {
            var planet = Prefabs.Instance.Produce<Planet>();

            planet.transform.position = Random.insideUnitSphere * Radius;
        }
    }
}
