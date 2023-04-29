using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Generator : Singleton<Generator>
{
    public float Radius;
    public int Count;

    public event Action OnGenerationDone;

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
        
        var launchPad = Prefabs.Instance.Produce<LaunchPad>();
        launchPad.transform.position = start.transform.position;
        launchPad.transform.rotation = Random.rotation;
        Destroy(start.gameObject);

        var ship = FindObjectOfType<Ship>();
        ship.transform.position = launchPad.transform.position;
        ship.transform.rotation = launchPad.transform.rotation;
        
        var finish = planets.PickRandom();
        finish.SetColor(Color.red);
        
        OnGenerationDone?.Invoke();
    }
}
