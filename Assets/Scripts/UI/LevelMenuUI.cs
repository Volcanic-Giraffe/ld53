using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenuUI : Singleton<LevelMenuUI>
{
    [SerializeField] private RectTransform container;

    [SerializeField] private Button continueButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button exitButton;

    public bool Visible => container.gameObject.activeSelf;
    
    private void Awake()
    {
        container.gameObject.SetActive(false);
    }

    private void Start()
    {
        continueButton.onClick.AddListener(OnContinueClicked);
        restartButton.onClick.AddListener(OnRestartClicked);
        exitButton.onClick.AddListener(OnExitClicked);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Visible)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            // GameInput.cs handles this
        }

        if (Input.GetKeyDown(KeyCode.Q) && Visible)
        {
            OnExitClicked();
        }
    }

    public void Show()
    {
        if (PermanentUI.Instance.Loading) return;
        
        container.gameObject.SetActive(true);
    }
    
    public void Hide()
    {
        container.gameObject.SetActive(false);
    }

    private void OnContinueClicked()
    {
        Hide();
    }
    
    private void OnRestartClicked()
    {
        if (PermanentUI.Instance.Loading) return;
        
        DOTween.KillAll();
        PermanentUI.Instance.FadeIn(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
    }
    private void OnExitClicked()
    {
        if (PermanentUI.Instance.Loading) return;
        
        DOTween.KillAll();
        PermanentUI.Instance.FadeIn(() =>
        {
            SceneManager.LoadScene("MainMenu");
        });
    }
}