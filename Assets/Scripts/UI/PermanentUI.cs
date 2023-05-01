using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PermanentUI : Singleton<PermanentUI>
{
    [SerializeField] private CanvasGroup loadingGroup;
    [SerializeField] private Image overlay;

    public void FadeOut()
    {
        overlay.gameObject.SetActive(true);
        overlay.raycastTarget = false;
        
        loadingGroup.DOKill();
        loadingGroup.DOFade(0, 0.31f).OnComplete(() =>
        {
            overlay.gameObject.SetActive(false);
        });
    }

    public void FadeIn(Action callback = null)
    {
        overlay.gameObject.SetActive(true);
        overlay.raycastTarget = true;
        
        loadingGroup.DOKill();
        loadingGroup.DOFade(1f, 0.27f).OnComplete(() =>
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
}
