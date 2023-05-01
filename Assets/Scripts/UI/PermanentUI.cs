using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PermanentUI : Singleton<PermanentUI>
{
    [SerializeField] private CanvasGroup loadingGroup;
    [SerializeField] private Image loadingOverlay;
    [SerializeField] private TextMeshProUGUI loadingMessage;
    [SerializeField] private Slider loadingBar;
    
    public bool Loading { get; private set; }
    
    private List<string> LoadingMessages = new()
    {
        "42",
        "Generating life, the universe, and everything",
        "Take it slow!",
        "Yellow barrels restore Fuel",
    };

    private void Awake()
    {
        loadingMessage.SetText("");
        loadingBar.gameObject.SetActive(false);
    }

    public void FadeOut()
    {
        loadingOverlay.gameObject.SetActive(true);
        loadingOverlay.raycastTarget = false;
        Loading = false;
        
        loadingGroup.DOKill();
        loadingGroup.DOFade(0, 0.17f).OnComplete(() =>
        {
            loadingOverlay.gameObject.SetActive(false);
            loadingBar.gameObject.SetActive(false);

        });
    }

    public void FadeIn(Action callback = null)
    {
        loadingMessage.SetText(LoadingMessages.PickRandom());
        
        loadingOverlay.gameObject.SetActive(true);
        loadingOverlay.raycastTarget = true;
        Loading = true;
        
        loadingGroup.DOKill();
        loadingGroup.DOFade(1f, 0.17f).OnComplete(() =>
        {
            callback?.Invoke();
        });
    }
    
    public void FadeTransition(Action callback)
    {
        FadeIn(() =>
        {
            callback?.Invoke();
            FadeOut();
        });
    }

    public void UpdateLoadingProgress(float ratio)
    {
        loadingBar.gameObject.SetActive(true);
        loadingBar.SetValueWithoutNotify(ratio);
    }
}
