using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingPad : MonoBehaviour
{
    private void Awake()
    {
        Objects.Instance.AddLandingPad(this);
    }

    private void OnDestroy()
    {
        if (Objects.Instance != null)
        {
            Objects.Instance.RemoveLandingPad(this);
        }
    }
}
