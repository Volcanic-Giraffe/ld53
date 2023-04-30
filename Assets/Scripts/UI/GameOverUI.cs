using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : Singleton<GameOverUI>
{
    [Header("Win Screen:")]
    [SerializeField] private RectTransform winContainer;

    [Header("Fail Screen:")]
    [SerializeField] private RectTransform failContainer;
    [SerializeField] private TextMeshProUGUI failReasonLabel;
    
    private void Awake()
    {
        winContainer.gameObject.SetActive(false);
        failContainer.gameObject.SetActive(false);
    }
    
    private void Start()
    {
        Generator.Instance.OnGenerationDone += Setup;
    }

    private void Setup()
    {
        LevelScenario.Instance.OnReturnedToLaunch += ShowWin;
    }

    public void ShowWin()
    {
        winContainer.gameObject.SetActive(true);
        failContainer.gameObject.SetActive(false);
    }
    
    public void ShowFail(string reason)
    {
        failContainer.gameObject.SetActive(true);
        winContainer.gameObject.SetActive(false);
        
        failReasonLabel.SetText(reason);
    }
}