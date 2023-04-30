using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MarkerItemUI : MonoBehaviour
{
    public Vector2 ScreenSize = new (64, 64); // px
    [Space]
    [SerializeField] private RectTransform container;
    [SerializeField] private TextMeshProUGUI distanceText;

    public Transform Target { get; set; }

    public void SetDistance(float dst)
    {
        distanceText.SetText($"{dst:0.00}");

        if (dst < 20f)
        {
            container.gameObject.SetActive(false);
        }
        else
        {
            container.gameObject.SetActive(true);
        }
    }
}
