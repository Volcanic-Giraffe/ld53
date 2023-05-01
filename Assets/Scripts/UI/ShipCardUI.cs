using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShipCardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI shipLabel;
    [SerializeField] private Image selector;
    [SerializeField] public Ship LinkedShip;
    
    private void Awake()
    {
        selector.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MainMenuController.Instance.SpinShip(LinkedShip.Code);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MainMenuController.Instance.StopSpin(LinkedShip.Code);
    }

    public void OnCardClicked()
    {
        MainMenuUI.Instance.OnCardClicked(this);
    }

    public void SetSelected(bool selected)
    {
        selector.gameObject.SetActive(selected);
    }
}
