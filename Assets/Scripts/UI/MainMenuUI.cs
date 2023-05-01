using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : Singleton<MainMenuUI>
{
    [SerializeField] private RectTransform container;

    private List<ShipCardUI> _cards;

    private void Awake()
    {
        _cards = FindObjectsOfType<ShipCardUI>().ToList();

        OnCardClicked(_cards.Find(c=> c.LinkedShip.Code == "Hopper"));
    }

    public void OnDeliverClicked()
    {
        SceneManager.LoadScene("LevelScene");
    }

    public void OnCardClicked(ShipCardUI selectedCard)
    {
        var code = selectedCard.LinkedShip.Code;
        
        GameState.PickedShip = code;

        foreach (var card in _cards)
        {
            card.SetSelected(card == selectedCard);
        }
    }
}
