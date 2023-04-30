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

    private bool _started;
    private bool _completed;
    
    
    private Ship _ship;

    private float _shipDiedTimer;
    private float _shipLowFuelTimer;
    
    void Start()
    {
        Generator.Instance.OnGenerationDone += Setup;
    }

    private void Setup()
    {
        _started = true;

        _ship = Objects.Instance.Ship;
        
        SpawnLandingPad();
        StatusBarUI.Instance.Show("[SPACE] to Deploy");
    }

    private void Update()
    {
        if (!_started) return;
        if (_completed) return;
        
        if (_ship.Health <= 0)
        {
            _shipDiedTimer -= Time.deltaTime;

            if (_shipDiedTimer <= 0)
            {
                ShipDied();
            }
        }
        else
        {
            _shipDiedTimer = Consts.GameOverDieTime;
        }
        
        if (_ship.Fuel <= 0)
        {
            _shipLowFuelTimer -= Time.deltaTime;

            if (_shipLowFuelTimer <= 0)
            {
                ShipNoFuel();
            }
        }
        else
        {
            _shipLowFuelTimer = Consts.GameOverNoFuelTime;
        }
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

    public void ShipDied()
    {
        _completed = true;
        StatusBarUI.Instance.Hide();
        
        GameOverUI.Instance.ShowFail("Hull Damaged");
    }
    
    public void ShipNoFuel()
    {
        _completed = true;
        StatusBarUI.Instance.Hide();
        
        GameOverUI.Instance.ShowFail("Low Fuel");
    }
}
