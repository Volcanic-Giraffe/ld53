using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : Singleton<Objects>
{
    [Header("DO NOT LINK THESE OBJECTS IN EDITOR")]
    // linked in code only
    public Ship Ship;
    public List<Planet> Planets = new();
    public List<LandingPad> LandingPads = new();
    public List<LaunchPad> LaunchPads = new();
    
    public void AddPlanet(Planet p)
    {
        Planets.Add(p);
    }

    public void RemovePlanet(Planet p)
    {
        Planets.Remove(p);
    }

    public void AddLandingPad(LandingPad lp)
    {
        LandingPads.Add(lp);
    }

    public void RemoveLandingPad(LandingPad lp)
    {
        LandingPads.Remove(lp);
    }

    public void AddLaunchPad(LaunchPad lp)
    {
        LaunchPads.Add(lp);
    }

    public void RemoveLaunchPad(LaunchPad lp)
    {
        LaunchPads.Remove(lp);
    }
}
