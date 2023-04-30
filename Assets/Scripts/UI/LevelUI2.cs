using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI2 : Singleton<LevelUI2>
{
    [SerializeField] private RectTransform container;

    [Space]
    [SerializeField] private RectTransform fuelGaugeKey;
    [SerializeField] private float fuelGaugeAngleMin;
    [SerializeField] private float fuelGaugeAngleMax;
    [Space]
    [SerializeField] private Slider hpBar;
    
    [Space]
    [SerializeField] private RectTransform speedGaugeKey;
    [SerializeField] private float speedGaugeAngleMin;
    [SerializeField] private float speedGaugeAngleMax;
    [Space]
    [SerializeField] private TextMeshProUGUI speedText;
    
    [Space]
    [SerializeField] private TextMeshProUGUI taskText;
    [SerializeField] private TextMeshProUGUI taskProgress;
    [SerializeField] private Image taskIconMail;
    [SerializeField] private Image taskIconReturn;
    [SerializeField] private Image taskIconParcel;
    
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

        LevelScenario.Instance.OnDeliveryMade += RefreshTasks;
    }
    
    private void Update()
    {
        if (_ship == null) return;

        var fuelAngle = Mathf.Lerp(fuelGaugeAngleMin, fuelGaugeAngleMax, _ship.FuelRatio);
        fuelGaugeKey.rotation = Quaternion.Euler(0,0,fuelAngle);

        var speedAngle = Mathf.Lerp(speedGaugeAngleMin, speedGaugeAngleMax, _ship.VelocityRatio);
        speedGaugeKey.rotation = Quaternion.Euler(0,0,speedAngle);

        hpBar.SetValueWithoutNotify(_ship.HealthRatio);

        speedText.SetText($"{_ship.Velocity:0.0} ms");
    }


    private void RefreshTasks()
    {
        
    }
}
