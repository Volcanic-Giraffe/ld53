using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelScenario : Singleton<LevelScenario>
{
    [SerializeField] private int deliveries;

    private int _delivered;

    public int DeliveriesNeed => deliveries;
    public int DeliveriesDone => _delivered;

    public event Action OnDeliveryMade;
    public event Action OnReturnedToLaunch;
    
    void Start()
    {
        Generator.Instance.OnGenerationDone += Setup;
    }

    private void Setup()
    {
        SpawnLandingPad();
        
        StatusBarUI.Instance.Show("[SPACE] to Deploy");
    }

    private void SpawnLandingPad()
    {
        var landingPad = Prefabs.Instance.Produce<LandingPad>();
        var planet = Objects.Instance.Planets.Where(p => p.LandingPad == null).PickRandom();

        planet.AttachPad(landingPad);
        planet.SetColor(Color.red);
    }

    public void DeployedFromLaunchPad()
    {
        StatusBarUI.Instance.Hide();
    }
    
    public void DeliveryMade(LandingPad lp)
    {
        _delivered += 1;

        if (_delivered == deliveries)
        {
            var pads = Objects.Instance.LaunchPads;

            pads.ForEach(p => p.SetReady(true));
            
            StatusBarUI.Instance.Show("RETURN TO LAUNCH PAD");
        }
        else
        {
            SpawnLandingPad();
        }
        
        OnDeliveryMade?.Invoke();
    }

    public void ReturnedToPad(LaunchPad pad)
    {
        OnReturnedToLaunch?.Invoke();
        
        StatusBarUI.Instance.Hide();
    }
}
