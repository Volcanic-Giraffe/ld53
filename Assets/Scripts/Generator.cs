using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Generator : Singleton<Generator>
{
    public float Radius;
    public int Count;

    public event Action OnGenerationDone;

    private List<Planet> _planets = new();
    
    private void Start()
    {
        GeneratePlanets();

        var start = _planets.PickRandom();
        
        var launchPad = Prefabs.Instance.Produce<LaunchPad>();
        launchPad.transform.position = start.transform.position;
        launchPad.transform.rotation = Random.rotation;
        Destroy(start.gameObject);

        var ship = FindObjectOfType<Ship>();
        ship.transform.position = launchPad.transform.position;
        ship.transform.rotation = launchPad.transform.rotation;
        
        var finish = _planets.PickRandom();
        finish.SetColor(Color.red);
        
        OnGenerationDone?.Invoke();
    }

    private void GeneratePlanets()
    {
        for (int i = 0; i < Count; i++)
        {
            var planet = Prefabs.Instance.Produce<Planet>();

            planet.transform.position = Random.insideUnitSphere * Radius;

            var attempts = 10f;
            while (_planets.Any(p => Vector3.Distance(planet.transform.position, p.transform.position) < 10f) && attempts > 0)
            {
                attempts -= 1;
                
                planet.transform.position = Random.insideUnitSphere * Radius;
            }
            
            _planets.Add(planet);
        }
    }
}
