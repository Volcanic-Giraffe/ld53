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
        
        PermanentUI.Instance.FadeOut();
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
        foreach (var c in _shipsContainers)
        {
            c.SetSpinning(c.Ship.Code == code);
        }
    }
}
