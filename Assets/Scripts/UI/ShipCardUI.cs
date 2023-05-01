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
    [SerializeField] private Image preSelector;
    [SerializeField] private Image selector;
    [SerializeField] public Ship LinkedShip;
    
    private void Awake()
    {
        selector.gameObject.SetActive(false);
        preSelector.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        preSelector.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        preSelector.gameObject.SetActive(false);
    }

    public void OnCardClicked()
    {
        Permanent.Sounds.PlayRandom("click_c");
        
        MainMenuUI.Instance.OnCardClicked(this);
    }

    public void SetSelected(bool selected)
    {
        preSelector.gameObject.SetActive(false);
        selector.gameObject.SetActive(selected);
    }
}
