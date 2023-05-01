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
        PermanentUI.Instance.FadeIn(() =>
        {
            SceneManager.LoadScene("LevelScene");
        });
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
