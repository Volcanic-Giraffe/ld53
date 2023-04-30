using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Planet : MonoBehaviour
{
    [SerializeField] private Transform shape;

    public LandingPad LandingPad { get; private set; }

    public float Diameter { get; private set; }

    private void Awake()
    {
        Objects.Instance.AddPlanet(this);
    }

    void Start()
    {
    }

    void Update()
    {
        
    }
    
    public void Setup()
    {
        Diameter = (Consts.PlanetDiameter * Random.Range(0.8f, 1.2f));
        shape.localScale = Vector3.one * Diameter;
        
        transform.rotation = Random.rotation;
    }

    public void GenerateSurface()
    {
        GetComponentInChildren<PlanetGenerator>().GenerateRandom();
    }

    public void AttachPad(LandingPad lp)
    {
        LandingPad = lp;
        lp.Planet = this;

        lp.transform.SetParent(transform);
        
        lp.transform.position = transform.position + Vector3.up * (Diameter * 0.5f);
        lp.transform.rotation = Quaternion.LookRotation(lp.transform.position - transform.position);
        
        transform.rotation = Random.rotation;
    }

    private void OnDestroy()
    {
        if (Objects.Instance != null)
        {
            Objects.Instance.RemovePlanet(this);
        }
    }
}
