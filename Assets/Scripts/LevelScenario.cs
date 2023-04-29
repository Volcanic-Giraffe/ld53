using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelScenario : Singleton<LevelScenario>
{
    [SerializeField] private int deliveries;

    private int _delivered;
    
    void Start()
    {
        Generator.Instance.OnGenerationDone += Setup;
    }

    private void Setup()
    {
        SpawnLandingPad();
    }

    private void SpawnLandingPad()
    {
        var landingPad = Prefabs.Instance.Produce<LandingPad>();
        var planet = Objects.Instance.Planets.Where(p => p.LandingPad == null).PickRandom();

        planet.AttachPad(landingPad);
        planet.SetColor(Color.red);
    }

    public void OnDeliveryMade(LandingPad lp)
    {
        _delivered += 1;

        if (_delivered == deliveries)
        {
            // victory!
            
            // ask return to base?
        }
        else
        {
            SpawnLandingPad();
        }
    }
}
