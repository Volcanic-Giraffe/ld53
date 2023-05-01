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
    [SerializeField] private GameObject ringPrefab;

    public int FuelPickupCount;

    public event Action OnGenerationDone;

    private List<Planet> _planets = new();

    private void Start()
    {
        StartCoroutine(GenerateCR());
    }

    private IEnumerator GenerateCR()
    {
        SelectShip();
        
        GeneratePlanets();
        for (int i = 0; i < _planets.Count; i++)
        {
            _planets[i].GenerateSurface();
            yield return new WaitForEndOfFrame();
        }

        var start = _planets.Where(p => p.LandingPad == null).PickRandom();

        var launchPad = Prefabs.Instance.Produce<LaunchPad>();
        launchPad.transform.position = start.transform.position;
        launchPad.transform.rotation = Random.rotation;
        Destroy(start.gameObject);

        var ship = Objects.Instance.Ship;
        launchPad.Attach(ship);

        for (int i = 0; i < FuelPickupCount; i++)
        {
            var planet = _planets.Where(p => p.LandingPad == null).PickRandom();

            var pickup = Prefabs.Instance.Produce<FuelPickup>();
            pickup.transform.position = planet.transform.position + Vector3.up * planet.Diameter * 0.6f;
        }

        yield return new WaitForEndOfFrame();

        OnGenerationDone?.Invoke();
        
        yield return new WaitForEndOfFrame();
        
        PermanentUI.Instance.FadeOut();
    }

    private void SelectShip()
    { 
        if (string.IsNullOrEmpty(GameState.PickedShip)) return;

        var ships = FindObjectsOfType<Ship>(true);

        foreach (var ship in ships)
        {
            ship.gameObject.SetActive(ship.Code == GameState.PickedShip);

            if (ship.gameObject.activeSelf)
            {
                Objects.Instance.Ship = ship;
            }
        }
    }

    private void GeneratePlanets()
    {
        for (int i = 0; i < Count; i++)
        {
            var planet = Prefabs.Instance.Produce<Planet>();
            planet.Setup();

            planet.transform.position = Random.insideUnitSphere * Radius;

            var attempts = 10f;
            while (_planets.Any(p => Vector3.Distance(planet.transform.position, p.transform.position) < (Radius / 5f)) && attempts > 0)
            {
                attempts -= 1;

                planet.transform.position = Random.insideUnitSphere * Radius;
            }
            if (Random.value > 0.9f)
            {
                var rotation = Random.rotation;
                CreateRing(planet, Random.Range(1.2f, 1.4f), rotation);
                if(Random.value > 0.75f)
                {
                    CreateRing(planet, Random.Range(1.6f, 2f), rotation);
                }
            }
            _planets.Add(planet);
        }
    }

    private void CreateRing(Planet planet, float size, Quaternion rotation)
    {
        var ring = Instantiate(ringPrefab);
        ring.GetComponentInChildren<SpriteRenderer>().color = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.7f, 1f);
        ring.transform.SetParent(planet.transform);
        ring.transform.localPosition = Vector3.zero;
        ring.transform.localScale = Vector3.one * planet.Diameter * size;
        ring.transform.localRotation = rotation;
    }
}
