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
    }
    
    private void Update()
    {
        if (_ship == null) return;
        
        fuelBar.SetValueWithoutNotify(_ship.FuelRatio);
        hullBar.SetValueWithoutNotify(_ship.HealthRatio);
    }
}
