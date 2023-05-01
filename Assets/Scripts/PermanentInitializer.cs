using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentInitializer : MonoBehaviour
{
    [SerializeField] private GameObject PermanentGO;

    private void Awake()
    {
        if (FindObjectsOfType<Permanent>().Length == 0)
        {
            Instantiate(PermanentGO);
        }
    }

    private void Start()
    {
    }
}
