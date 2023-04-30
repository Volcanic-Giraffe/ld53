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

    private void Awake()
    {
        frame.gameObject.SetActive(false);
    }

    private void Start()
    {
        group.DOFade(0.2f, 0.63f).SetLoops(-1, LoopType.Yoyo);
    }

    public void Show(string message)
    {
        frame.gameObject.SetActive(true);
        statusLabel.SetText(message);
    }

    public void Hide()
    {
        frame.gameObject.SetActive(false);
    }
}
