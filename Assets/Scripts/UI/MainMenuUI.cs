using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : Singleton<MainMenuUI>
{
    [SerializeField] private RectTransform container;
    [SerializeField] private TextMeshProUGUI shipTitle;
    [SerializeField] private TextMeshProUGUI shipDescription;
    [SerializeField] private TextMeshProUGUI shipControls;

    private List<ShipCardUI> _cards;

    private bool _done;
    
    private void Awake()
    {
        _cards = FindObjectsOfType<ShipCardUI>().ToList();
    }

    private IEnumerator Start()
    {
        Cursor.lockState = CursorLockMode.None;
        yield return new WaitForSeconds(0.3f);
        yield return new WaitForEndOfFrame();
        
        OnCardClicked(_cards.Find(c=> c.LinkedShip.Code == "Gyro"));
    }

    public void OnDeliverClicked()
    {
        if (_done) return;
        _done = true;
        
        Permanent.Sounds.PlayRandom("notify_a");
        
        PermanentUI.Instance.FadeIn(() =>
        {
            SceneManager.LoadScene("LevelScene");
        });
    }

    public void Update()
    {
        var padX = Input.GetAxis("Pad Horizontal");
        var mainX = Input.GetAxis("Horizontal");

        if (padX > 0.9f || mainX > 0.9f)
        {
            OnCardClicked(_cards[0]);
        }

        if (padX < -0.9f || mainX < -0.9f)
        {
            OnCardClicked(_cards[1]);
        }
        
        if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Accept"))
        {
            OnDeliverClicked();
        }
    }

    public void OnCardClicked(ShipCardUI selectedCard)
    {
        var code = selectedCard.LinkedShip.Code;
        
        GameState.PickedShip = code;

        foreach (var card in _cards)
        {
            card.SetSelected(card == selectedCard);
        }
        
        shipTitle.SetText(selectedCard.LinkedShip.Title);
        shipDescription.SetText(selectedCard.LinkedShip.Description);
        shipControls.SetText(selectedCard.LinkedShip.Controls);

        MainMenuController.Instance.SpinShip(code);
    }
}
