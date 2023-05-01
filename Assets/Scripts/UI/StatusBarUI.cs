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

    private Color _frameColorInitial;
    private Color _frameColorWarning = new Color(1, 0.3098039f, 0, 0.6f);

    private void Awake()
    {
        frame.gameObject.SetActive(false);

        _frameColorInitial = frame.color;
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

    public void Show(string message, float autohide = 0f, bool warnning = false)
    {
        frame.gameObject.SetActive(true);
        statusLabel.SetText(message);

        _autoHideTimer = autohide;

        if (warnning)
        {
            frame.color = _frameColorWarning;
        }
        else
        {
            frame.color = _frameColorInitial;
        }
    }

    public void Hide()
    {
        frame.gameObject.SetActive(false);
        
        _autoHideTimer = 0f;
    }
}
