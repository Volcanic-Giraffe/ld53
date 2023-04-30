using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum QuestType
{
    FloatingPackage,
    LandingPad,
    OrbitalPackage
}

public class DeliveryQuest
{
    public int Count;
    public int Completed;
    public QuestType Type;

    public bool Done => Count == Completed;
}

public class LevelScenario : Singleton<LevelScenario>
{
    private readonly List<DeliveryQuest> Quests = new ()
    {
        new()
        {
            Count = 3,
            Completed = 0,
            Type = QuestType.FloatingPackage
        },
        new ()
        {
            Count = 3,
            Completed = 0,
            Type = QuestType.LandingPad
        },
        new ()
        {
            Count = 3,
            Completed = 0,
            Type = QuestType.OrbitalPackage
        }
    };

    public DeliveryQuest ActiveQuest { get; private set; }

    public int DeliveriesNeed => ActiveQuest.Count;
    public int DeliveriesDone => ActiveQuest.Completed;

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
        
        StatusBarUI.Instance.Show("[SPACE] to Deploy");

        StartNextQuest();
    }

    private void StartNextQuest()
    {
        if (Quests.Count == 0)
        {
            // victory
            
            var pads = Objects.Instance.LaunchPads;
            
            pads.ForEach(p => p.SetReady(true));
            
            StatusBarUI.Instance.Show("RETURN TO LAUNCH PAD");
        }

        ActiveQuest = Quests[0];
        Quests.RemoveAt(0);

        ProceedActiveQuest();
    }

    private void ProceedActiveQuest()
    {
        if (ActiveQuest.Done)
        {
            StartNextQuest();
            return;
        }

        switch (ActiveQuest.Type)
        {
            case QuestType.FloatingPackage:
                SpawnFloatingPackage();
                break;
            case QuestType.LandingPad:
                SpawnLandingPad();
                break;
            case QuestType.OrbitalPackage:
                SpawnOrbitalPackage();
                break;
        }
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

    private void SpawnFloatingPackage()
    {
        var package = Prefabs.Instance.Produce<PackagePickup>();

        package.transform.position = Objects.Instance.LaunchPads[0].transform.position + Rnd.InRadius(10f);
        
        MarkersPanelUI.Instance.AddMarker(package.transform);
    }
    
    private void SpawnLandingPad()
    {
        var landingPad = Prefabs.Instance.Produce<LandingPad>();
        var planet = Objects.Instance.Planets.Where(p => p.LandingPad == null).PickRandom();

        planet.AttachPad(landingPad);
        
        MarkersPanelUI.Instance.AddMarker(landingPad.transform);
    }
    
    private void SpawnOrbitalPackage()
    {
        var package = Prefabs.Instance.Produce<PackagePickup>();
        var planet = Objects.Instance.Planets.Where(p => p.LandingPad == null).PickRandom();
        
        package.transform.position = planet.transform.position + planet.transform.up * (planet.Diameter * 0.6f);
        
        MarkersPanelUI.Instance.AddMarker(package.transform);
    }
    
    public void DeployedFromLaunchPad()
    {
        StatusBarUI.Instance.Hide();
    }
    
    public void DeliveryMade(LandingPad lp)
    {
        ActiveQuest.Completed += 1;
        ProceedActiveQuest();

        OnDeliveryMade?.Invoke();
    }
    
    public void PackagePicked(PackagePickup package)
    {
        MarkersPanelUI.Instance.RemoveMarker(package.transform);
        
        ActiveQuest.Completed += 1;
        ProceedActiveQuest();
        
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
