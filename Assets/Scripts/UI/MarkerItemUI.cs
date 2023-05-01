using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum MarkerIcon
{
    Envelope,
    MailTarget,
    Pin,
    Dot,
    Crosshair
}

public class MarkerItemUI : MonoBehaviour
{
    public Vector2 ScreenSize = new (64, 64); // px
    [Space]
    [SerializeField] private RectTransform container;

    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI distanceText;

    [SerializeField] private List<Sprite> _spriteIcons;
    
    public Transform Target { get; set; }

    private Vector2 _targetPos;

    private Color _hoverColor = new Color(1, 1, 1, 0.3f);
    
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

    public void SetIcon(MarkerIcon iconType)
    {
        var idx = Math.Clamp((int)iconType, 0, _spriteIcons.Count - 1);

        icon.sprite = _spriteIcons[idx];
    }

    private void Update()
    {
        // Feels drunk
        transform.position = _targetPos; // Vector2.Lerp(transform.position, _targetPos, 5f * Time.deltaTime);
    }

    public void SetPosition(Vector2 screenPos)
    {
        _targetPos = screenPos;

        var mid = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        
        var dst = Vector2.Distance(screenPos, mid);

        if (dst < 100f)
        {
            icon.color = _hoverColor;
        }
        else
        {
            icon.color = Color.white;
        }
    }
}
