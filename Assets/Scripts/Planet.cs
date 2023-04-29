using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField] private Transform shape;

    void Start()
    {
        shape.localScale *= Random.Range(0.9f, 1.5f);
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
