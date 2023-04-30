using System;
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

    private Vector2 _targetPos;
    
    public void SetDistance(float dst)
    {
        distanceText.SetText($"{dst:0.00}");

        if (dst < 10f)
        {
            container.gameObject.SetActive(false);
        }
        else
        {
            container.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        transform.position = Vector2.Lerp(transform.position, _targetPos, 5f * Time.deltaTime);
    }

    public void SetPosition(Vector2 screenPos)
    {
        _targetPos = screenPos;
    }
}
