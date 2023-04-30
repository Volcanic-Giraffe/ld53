using System;
using System.Collections.Generic;
using UnityEngine;

public class MarkersPanelUI : Singleton<MarkersPanelUI>
{
    private bool _ready;

    private Camera _camera;

    private List<MarkerItemUI> _markers = new ();
    
    private void Start()
    {
        Generator.Instance.OnGenerationDone += Setup;
    }

    private void Setup()
    {
        _ready = true;
        _camera = Camera.main;
    }

    public void AddMarker(Transform target)
    {
        var marker = Prefabs.Instance.Produce<MarkerItemUI>();

        marker.Target = target;
        marker.transform.SetParentZero(transform);
        
        _markers.Add(marker);
    }

    public void RemoveMarker(Transform target)
    {
        var marker = _markers.Find(m => m.Target == target);

        if (marker != null)
        {
            Destroy(marker.gameObject);
            _markers.Remove(marker);
        }
    }
    
    private void FixedUpdate()
    {
        if (!_ready) return;
        
        foreach (var marker in _markers)
        {
            marker.SetPosition(MarkerPosition(marker));
            marker.SetDistance(MarkerDistance(marker));
        }
    }

    private float MarkerDistance(MarkerItemUI item)
    {
        var source = _camera.transform;
        var target = item.Target;

        return Vector3.Distance(source.position, target.position);
    }

    private Vector2 MarkerPosition(MarkerItemUI item)
    {
        var source = _camera.transform;

        var target = item.Target;
        var targetPos = target.position + Vector3.up * 1f;

        var itemSize = item.ScreenSize;
        
        var result = _camera.WorldToScreenPoint(new Vector3(targetPos.x, targetPos.y, targetPos.z));

        if (Vector3.Dot(targetPos - source.position, source.forward) < 0)
        {
            if (result.x < Screen.width * 0.5f)
            {
                result.x = Screen.width - itemSize.x;
            }
            else
            {
                result.x = itemSize.x;
            }

            if (result.y < Screen.height * 0.5f)
            {
                result.y = Screen.height - itemSize.y;
            }
            else
            {
                result.y = itemSize.y;
            }
        }

        result.x = ScreenClamp(result.x, itemSize.x, Screen.width);
        result.y = ScreenClamp(result.y, itemSize.y, Screen.height);

        return result;
    }
    
    private float ScreenClamp(float screenPosition, float imageWidth, int screenWidth)
    {
        return Mathf.Clamp(screenPosition, imageWidth, screenWidth - imageWidth);
    }
}
