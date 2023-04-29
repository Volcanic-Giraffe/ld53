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
        StartCoroutine(GenerateCR());
    }

    private IEnumerator GenerateCR()
    {
        GeneratePlanets();

        var start = _planets.Where(p => p.LandingPad == null).PickRandom();

        var launchPad = Prefabs.Instance.Produce<LaunchPad>();
        launchPad.transform.position = start.transform.position;
        launchPad.transform.rotation = Random.rotation;
        Destroy(start.gameObject);

        var ship = Objects.Instance.Ship;
        launchPad.Attach(ship);
        
        
        yield return new WaitForEndOfFrame();

        OnGenerationDone?.Invoke();
    }

    private void GeneratePlanets()
    {
        for (int i = 0; i < Count; i++)
        {
            var planet = Prefabs.Instance.Produce<Planet>();
            planet.Setup();

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
