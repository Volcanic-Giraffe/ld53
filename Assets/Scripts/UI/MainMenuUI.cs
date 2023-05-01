using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : Singleton<MainMenuUI>
{
    [SerializeField] private RectTransform container;

    public void OnDeliverClicked()
    {
        SceneManager.LoadScene("LevelScene");
    }

    public void OnCardClicked(ShipCardUI shipCard)
    {
        var code = shipCard.LinkedShip.Code;
        
        GameState.PickedShip = code;
    }
}
