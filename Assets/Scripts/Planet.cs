using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField] private Transform shape;

    void Start()
    {
        shape.localScale = Vector3.one * (Consts.PlanetDiameter * Random.Range(0.8f, 1.2f));
    }

    void Update()
    {
        
    }

    public void SetColor(Color color)
    {
        var rends = GetComponentsInChildren<MeshRenderer>();

        foreach (var rend in rends)
        {
            rend.material.DOColor(color, 0.1f);
        }
    }
}
