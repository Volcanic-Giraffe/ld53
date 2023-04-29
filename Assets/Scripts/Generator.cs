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
        var planets = new List<Planet>();
        
        for (int i = 0; i < Count; i++)
        {
            var planet = Prefabs.Instance.Produce<Planet>();

            planet.transform.position = Random.insideUnitSphere * Radius;
            planets.Add(planet);
        }

        var start = planets.PickRandom();
        start.SetColor(Color.green);

        var ship = FindObjectOfType<Ship>();

        ship.transform.position = start.transform.position + Vector3.up * 10f;
        
        var finish = planets.PickRandom();
        finish.SetColor(Color.red);
    }
}
