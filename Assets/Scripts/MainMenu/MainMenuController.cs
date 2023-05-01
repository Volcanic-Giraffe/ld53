using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainMenuController : Singleton<MainMenuController>
{
    private List<Planet> _planets = new ();
    private List<ShipContainer> _shipsContainers = new();
    
    void Start()
    {
        _planets = FindObjectsOfType<Planet>().ToList();
        _shipsContainers = FindObjectsOfType<ShipContainer>().ToList();
        
        foreach (var planet in _planets)
        {
            // planet.Setup();
            planet.GenerateSurface();
        }
    }

    private void Update()
    {
        
        foreach (var planet in _planets)
        {
            planet.transform.Rotate(Vector3.up * (planet.GetInstanceID() % 30 * Time.deltaTime));
        }
    }

    public void SpinShip(string code)
    {
        var cont = _shipsContainers.Find(c => c.Ship.Code == code);

        if (cont != null)
        {
            cont.SetSpinning(true);
        }
    }
    
    public void StopSpin(string code)
    {
        var cont = _shipsContainers.Find(c => c.Ship.Code == code);

        if (cont != null)
        {
            cont.SetSpinning(false);
        }
    }
}
