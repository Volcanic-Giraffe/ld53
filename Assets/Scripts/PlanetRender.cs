using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

[ExecuteAlways]
public class PlanetRender : MonoBehaviour
{
    public int size = 1024;

    public float scale = 2.0f;
    public int seed = 0;

    public Texture2D texture;
    public Texture2D colors;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Generate()
    {
        texture = new Texture2D(size, size, TextureFormat.RGB24, true)
        {
            wrapMode = TextureWrapMode.Clamp,
            filterMode = FilterMode.Trilinear,
            anisoLevel = 9
        };
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                var fx = ((float)x + seed)/scale;
                var fy =((float)y)/scale;
                var w1 = 1.0f;
                var w2 = 0.0f;
                var w3 = 0.0f;
                var value = w1 * noise.psrnoise(new float2(fx, fy), new float2(size, size));
                //value = w1 * noise.pnoise(new float2(fx/size, fy/size), new float2(1, 1));
                //value += w2 * noise.psrnoise(new float2(fx+0.3f, fy-0.2f), new float2(size*2f, size*2f));
                //value += w3 * noise.psrnoise(new float2(fx-0.1f, fy+0.6f), new float2(size*4f, size*4f));
                //value += noise.psrnoise(new float2(fx, fy), new float2(1, 1));
                //value = (0.5f + 0.5f * value);
                texture.SetPixel(x, y, new Color(value, value, value));
            }
        }
        texture.Apply();

        var renderer = GetComponent<MeshRenderer>();

        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(mpb);
        mpb.SetTexture("_HeightMapTexture", texture);
        renderer.SetPropertyBlock(mpb);
        
        colors = new Texture2D(128, 1);
        var random = new Random();
        random.InitState((uint)seed);

        var colorsList = new List<PosColor>();
        //GetComponent<MeshRenderer>().material.mainTexture = texture;

    }

    private void OnEnable()
    {
        Generate();
    }

    struct PosColor
    {
        float pos;
        Color color;
    }
}
