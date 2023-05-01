using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusBarUI : Singleton<StatusBarUI>
{
    [SerializeField] private CanvasGroup group;
    [SerializeField] private Image frame;

    [SerializeField] private TextMeshProUGUI statusLabel;

    private float _autoHideTimer;
    
    private void Awake()
    {
        frame.gameObject.SetActive(false);
    }

    private void Start()
    {
        group.DOFade(0.2f, 0.63f).SetLoops(-1, LoopType.Yoyo);
    }

    private void Update()
    {
        if (_autoHideTimer > 0)
        {
            _autoHideTimer -= Time.deltaTime;

            if (_autoHideTimer <= 0)
            {
                Hide();
            }
        }
    }

    public void Show(string message, float autohide = 0f)
    {
        frame.gameObject.SetActive(true);
        statusLabel.SetText(message);

        _autoHideTimer = autohide;
    }

    public void Hide()
    {
        frame.gameObject.SetActive(false);
        
        _autoHideTimer = 0f;
    }
}
