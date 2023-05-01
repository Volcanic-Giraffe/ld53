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
    public int Spawned;
    public int Completed;
    public bool MustVisitLaunchPad;
    public QuestType Type;

    public bool Done => Count == Completed;
}

public class LevelScenario : Singleton<LevelScenario>
{
    private readonly List<DeliveryQuest> Quests = new()
    {
        new()
        {
            Count = 3,
            Completed = 0,
            MustVisitLaunchPad = true,
            Type = QuestType.FloatingPackage
        },
        new()
        {
            Count = 3,
            Completed = 0,
            MustVisitLaunchPad = true,
            Type = QuestType.LandingPad
        },
        new()
        {
            Count = 3,
            Completed = 0,
            MustVisitLaunchPad = true,
            Type = QuestType.OrbitalPackage
        }
    };

    public DeliveryQuest ActiveQuest { get; private set; }

    public int DeliveriesNeed => ActiveQuest.Count;
    public int DeliveriesDone => ActiveQuest.Completed;

    public event Action OnDeliveryMade;
    public event Action OnReturnedToLaunch;
    public event Action OnCompletedAllQuests;

    private bool _started;
    private bool _completed;


    private Ship _ship;

    private float _shipDiedTimer;
    private float _shipLowFuelTimer;

    public float PlayTime;

    public string PlayTimeFormatted
    {
        get
        {
            var ts = TimeSpan.FromSeconds(PlayTime);
            var mm = $"{ts.Minutes:D2}";
            var ss = $"{ts.Seconds:D2}";
            var ms = $"{ts.Milliseconds:D3}";

            return $"{mm}:{ss}.{ms}";
        }
    }

    private List<Vector3> _occupiedPoints = new ();
    
    void Start()
    {
        Generator.Instance.OnGenerationDone += Setup;
    }

    private void Setup()
    {
        _started = true;

        _ship = Objects.Instance.Ship;
        
        StatusBarUI.Instance.Show("[SPACE] to Deploy");

        foreach (var planet in Objects.Instance.Planets)
        {
            _occupiedPoints.Add(planet.transform.position);
        }

        foreach (var launchPad in Objects.Instance.LaunchPads)
        {
            _occupiedPoints.Add(launchPad.transform.position);
        }
        
        StartNextQuest();
    }

    private void StartNextQuest()
    {
        if (Quests.Count == 0)
        {
            OnCompletedAllQuests?.Invoke();
            _completed = true;
            return;
        }

        ActiveQuest = Quests[0];
        Quests.RemoveAt(0);

        ProceedActiveQuest();
    }

    private void ProceedActiveQuest()
    {
        if (ActiveQuest.Done)
        {
            if (ActiveQuest.MustVisitLaunchPad)
            {
                var pads = Objects.Instance.LaunchPads;

                foreach (var p in pads)
                {
                    MarkersPanelUI.Instance.AddMarker(p.transform, MarkerIcon.Crosshair);
                    p.SetReady(true);
                }
                
                StatusBarUI.Instance.Show("RETURN TO BASE STATION");
                
            }
            else
            {
                StartNextQuest();
                return;
            }
            
        }

        if (ActiveQuest.Spawned == ActiveQuest.Count)
        {
            // do nothing, player should collect all
            return;
        }

        switch (ActiveQuest.Type)
        {
            case QuestType.FloatingPackage:
                SpawnFloatingPackagesAll();
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
        
        if (!LevelMenuUI.Instance.Visible)
        {
            PlayTime += Time.deltaTime;
        }
        
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

    private void SpawnFloatingPackagesAll()
    {
        for (int i = 0; i < ActiveQuest.Count; i++)
        {
            SpawnFloatingPackage();
        }
    }

    private void SpawnFloatingPackage()
    {
        var package = Prefabs.Instance.Produce<PackagePickup>();

        var attempts = 20;
        
        while(--attempts > 0)
        {
            package.transform.position = Objects.Instance.LaunchPads[0].transform.position + Rnd.InRadius(20f);

            if (_occupiedPoints.All(p => Vector3.Distance(p, package.transform.position) > 20f))
            {
                break;
            }
        }

        MarkersPanelUI.Instance.AddMarker(package.transform, MarkerIcon.Envelope);
        ActiveQuest.Spawned += 1;
        
        _occupiedPoints.Add(package.transform.position );
    }
    
    private void SpawnLandingPad()
    {
        var landingPad = Prefabs.Instance.Produce<LandingPad>();
        var planet = Objects.Instance.Planets.Where(p => p.LandingPad == null).PickRandom();

        planet.AttachPad(landingPad);
        
        MarkersPanelUI.Instance.AddMarker(landingPad.transform, MarkerIcon.MailTarget);
        ActiveQuest.Spawned += 1;
    }
    
    private void SpawnOrbitalPackage()
    {
        var package = Prefabs.Instance.Produce<PackagePickup>("PackagePickupHeavy");
        var planet = Objects.Instance.Planets.Where(p => p.LandingPad == null).PickRandom();
        
        package.transform.position = planet.transform.position + planet.transform.up * (planet.Diameter * 0.6f);
        
        MarkersPanelUI.Instance.AddMarker(package.transform, MarkerIcon.Envelope);
        ActiveQuest.Spawned += 1;
    }
    
    public void DeployedFromLaunchPad()
    {
        StatusBarUI.Instance.Hide();

        if (ActiveQuest.Type == QuestType.FloatingPackage && ActiveQuest.Completed == 0)
        {
            StatusBarUI.Instance.Show($"Collect {ActiveQuest.Count} Lost Parcels", 9f);
        }
        
        if (ActiveQuest.Type == QuestType.LandingPad && ActiveQuest.Completed == 0)
        {
            StatusBarUI.Instance.Show($"Deliver {ActiveQuest.Count} Mails", 9f);
        }
        
        if (ActiveQuest.Type == QuestType.OrbitalPackage && ActiveQuest.Completed == 0)
        {
            StatusBarUI.Instance.Show($"Collect {ActiveQuest.Count} Packages", 9f);
        }
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

        StatusBarUI.Instance.Hide();
        
        ActiveQuest.Completed += 1;
        ProceedActiveQuest();
        
        OnDeliveryMade?.Invoke();
    }

    public void ReturnedToPad(LaunchPad pad)
    {
        MarkersPanelUI.Instance.RemoveMarker(pad.transform);
        
        OnReturnedToLaunch?.Invoke();

        StatusBarUI.Instance.Hide();
        
        ActiveQuest.MustVisitLaunchPad = false;
        ProceedActiveQuest();
    }

    public void ShipDied()
    {
        _completed = true;
        StatusBarUI.Instance.Hide();
        
        GameOverUI.Instance.ShowFail("You brocket your rocket");
    }
    
    public void ShipNoFuel()
    {
        _completed = true;
        StatusBarUI.Instance.Hide();
        
        GameOverUI.Instance.ShowFail("Your fuel is nuel");
    }

    public static bool IsCompleted => Instance != null && Instance._completed;
}
