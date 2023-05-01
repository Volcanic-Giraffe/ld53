using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : Singleton<LevelUI>
{
    [SerializeField] private RectTransform container;
    [SerializeField] private Slider fuelBar;
    [SerializeField] private Slider hullBar;
    [SerializeField] private TextMeshProUGUI deliveryProgress;
    [SerializeField] private TextMeshProUGUI deliveryLabel;
    [SerializeField] private TextMeshProUGUI speedBar;

    private Ship _ship;

    private Color _colorGood = new Color(0.62f, 1f, 0.31f);
    
    private void Awake()
    {
        container.gameObject.SetActive(false);
    }

    private void Start()
    {
        Generator.Instance.OnGenerationDone += Setup;
    }

    private void Setup()
    {
        _ship = Objects.Instance.Ship;
        
        container.gameObject.SetActive(true);
    }
    
    private void Update()
    {
        if (_ship == null) return;
        
        fuelBar.SetValueWithoutNotify(_ship.FuelRatio);
        hullBar.SetValueWithoutNotify(_ship.HealthRatio);
        speedBar.SetText($"{_ship.Velocity:0.0} ms");

        RefreshDeliveries();
    }

    private void RefreshDeliveries()
    {
        var quest = LevelScenario.Instance.ActiveQuest;

        deliveryProgress.SetText($"{quest.Completed} / {quest.Count}");
        
        if (quest.Done && quest.MustVisitLaunchPad)
        {
            deliveryProgress.color = _colorGood;
        }
        else if (!quest.Done && quest.Type == QuestType.FloatingPackage)
        {
            deliveryLabel.SetText("LOST PARCEL");
            deliveryProgress.color = Color.white;
        
        }
        else if (!quest.Done && quest.Type == QuestType.LandingPad)
        {
            deliveryLabel.SetText("DELIVERY");
            deliveryProgress.color = Color.white;
        }
        else if (!quest.Done && quest.Type == QuestType.OrbitalPackage)
        {
            deliveryLabel.SetText("HEAVY PACKAGE");
            deliveryProgress.color = Color.white;
        }
        else
        {
            deliveryLabel.SetText("");
            deliveryProgress.SetText("");
        }
    }
    
}
