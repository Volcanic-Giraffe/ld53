using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : Singleton<LevelUI>
{
    [SerializeField] private RectTransform container;
    [SerializeField] private Slider fuelBar;
    [SerializeField] private Slider hullBar;
    [SerializeField] private TextMeshProUGUI deliveryBar;

    private Ship _ship;

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

        LevelScenario.Instance.OnDeliveryMade += RefreshDeliveries;
    }
    
    private void Update()
    {
        if (_ship == null) return;
        
        fuelBar.SetValueWithoutNotify(_ship.FuelRatio);
        hullBar.SetValueWithoutNotify(_ship.HealthRatio);
        
    }

    private void RefreshDeliveries()
    {
        var done = LevelScenario.Instance.DeliveriesDone;
        var need = LevelScenario.Instance.DeliveriesNeed;
        
        deliveryBar.SetText($"{done} / {need}");

        if (done == need)
        {
            deliveryBar.color = new Color(0.62f, 1f, 0.31f);
        }
    }
    
}
