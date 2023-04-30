using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = Unity.Mathematics.Random;

[ExecuteAlways]
public class PlanetGenerator : MonoBehaviour
{
    public Texture noiseTex;
    public Texture colorTex;
    public Material drawMaterial;

    [NotNull] public List<Texture2D> whiteBrushes = new List<Texture2D>();
    [NotNull] public List<Texture2D> blackBrushes = new List<Texture2D>();
    public List<Texture2D> seamlessNoises = new List<Texture2D>();
    public List<Texture2D> colorSchemes = new List<Texture2D>();
    
    public int size = 1024;
    public float hpow = 0.2f;
    public float heightStrength = 0.1f;
    public float cpow = 1f;
    public uint seed = 1;

    [Range(0, 1)]
    public float weight1 = 0.8f;
    [Range(0, 1)]
    public float weight2 = 0.2f;
    [Range(0, 1)]
    public float weight3 = 0.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Mesh originalMesh = null;

    public void Generate()
    {
        var rnd = new Random();
        rnd.InitState(seed);
        var time1 = System.DateTime.UtcNow;

        var weights = new float[] { weight1, weight2, weight3 };

        //no size
        const int nsize = 512;
        var offsets = new int2[]
        {
            rnd.NextInt2(nsize),
            rnd.NextInt2(nsize),
            rnd.NextInt2(nsize),
        };
        var minH = 111f;
        var maxH = -111f;
        var outputTexture = new Texture2D(nsize, nsize);
        var ti1 = rnd.NextInt(seamlessNoises.Count);
        var tp1 = GetCachedColors(ti1, seamlessNoises[ti1]);
        //yield return new WaitForEndOfFrame();
        var ti2 = rnd.NextInt(seamlessNoises.Count);
        var tp2 = GetCachedColors(ti2, seamlessNoises[ti2]);
        //yield return new WaitForEndOfFrame();
        var ti3 = rnd.NextInt(seamlessNoises.Count);
        var tp3 = weight3 > 0 ? GetCachedColors(ti3, seamlessNoises[ti3]) : tp2;
        //yield return new WaitForEndOfFrame();
        var time2 = System.DateTime.UtcNow;
        var tpout = GenerateNoiseTexture(nsize, tp1, offsets, tp2, tp3, ref minH, ref maxH);
        //yield return new WaitForEndOfFrame();
        var time3 = System.DateTime.UtcNow;

        this.noiseTex = outputTexture;

        var colorScheme1 = colorSchemes[rnd.NextInt(colorSchemes.Count)]; 
        
        var mesh = originalMesh;

        var verticesList = new List<Vector3>();
        var normalsList = new List<Vector3>(); 
        var uvsList = new List<Vector2>();
        var colorList = new List<Color>();
        ApplyHeightAndColor(mesh, verticesList, normalsList, uvsList, nsize, tpout, minH, maxH, colorScheme1, colorList);
        var time4 = System.DateTime.UtcNow;
        //yield return new WaitForEndOfFrame();
        //Debug.Log($"Min is {minH} max is {maxH}");

        StartCoroutine(SetNewMesh(verticesList, mesh, uvsList, colorList));
        var time5 = System.DateTime.UtcNow;

        SetColorSchemeToMaterial(colorScheme1);
        var time6 = System.DateTime.UtcNow;
        /*
        Debug.Log($"Read pixels = {(time2-time1).TotalMilliseconds}, \n" +
                  $"Generate noise tex= {(time3-time2).TotalMilliseconds}, \n" +
                  $"Apply h&c={(time4-time3).TotalMilliseconds}, \n" +
                  $"New mesh={(time5-time4).TotalMilliseconds}, \n" +
                  $"Set color to material={(time6-time5).TotalMilliseconds}");
        Debug.Log($"Total = {(time6-time1).TotalMilliseconds}");
        */
        //yield return null;
    }

    private float[] GenerateNoiseTexture(int nsize, Color[] tp1, int2[] offsets, Color[] tp2, Color[] tp3, ref float minH,
        ref float maxH)
    {
        var tpout = new float[nsize * nsize];
        var ii = 0;
        for (int y = 0; y < nsize; y++)
        {
            for (int x = 0; x < nsize; x++)
            {
                var h1 = tp1[((int)(x + offsets[0].x) % nsize + nsize * ((int)(y + offsets[0].y) % nsize))]
                    .r;
                var h2 = tp2[((int)(x + offsets[1].x) % nsize + nsize * ((int)(y + offsets[1].y) % nsize))]
                    .r;
                var h3 = weight3 > 0 ? tp3[((int)(x + offsets[2].x) % nsize + nsize * ((int)(y + offsets[2].y) % nsize))].r : 0f;

                h1 = 2f * (h1 - 0.5f);
                h2 = 2f * (h2 - 0.5f);
                h3 = 2f * (h3 - 0.5f);

                var rh = weight1 * h1 + weight2 * h2 + weight3 * h3;
                var rhAsColor = rh / 2f + 0.5f;

                tpout[ii] = rhAsColor;
                ii++;
                if (rhAsColor < minH) minH = rhAsColor;
                if (rhAsColor > maxH) maxH = rhAsColor;
            }
        }

        return tpout;
    }

    private void ApplyHeightAndColor(Mesh mesh, List<Vector3> verticesList, List<Vector3> normalsList, List<Vector2> uvsList, int nsize, float[] tpout,
        float minH, float maxH, Texture2D colorScheme1, List<Color> colorList)
    {
        mesh.GetVertices(verticesList);
        mesh.GetNormals(normalsList);
        mesh.GetUVs(0, uvsList);
        for (int i = 0; i < mesh.vertexCount; i++)
        {
            var u = (int)(uvsList[i].x * nsize) % nsize;
            var v = (int)(uvsList[i].y * nsize) % nsize;
            var height = tpout[v * nsize + u]; // outputTexture.GetPixel((int)u, (int)v).r;
            //normalize
            height = (height - minH) / (maxH - minH);
            var pos = verticesList[i];
            //var color = colorScheme1.GetPixel((int)(Mathf.Pow(height, cpow) * colorScheme1.width), 2);
            var color = new Color(1, 1, 1, height);

            height = 2f * height - 1f;
            height = Mathf.Sign(height) * Mathf.Pow(Mathf.Abs(height), hpow);

            colorList.Add(color);
            var newPos = pos + normalsList[i] * height * heightStrength;
            verticesList[i] = newPos;
        }
    }

    private IEnumerator SetNewMesh(List<Vector3> verticesList, Mesh mesh, List<Vector2> uvsList, List<Color> colorList)
    {
        var newMesh = new Mesh()
        {
            vertices = verticesList.ToArray(),
            triangles = mesh.triangles,
            uv = uvsList.ToArray(),
            colors = colorList.ToArray()
        };
        newMesh.RecalculateBounds();
        newMesh.RecalculateNormals();
        newMesh.RecalculateTangents();
        GetComponent<MeshFilter>().mesh = newMesh;
        yield return null;
    }

    private void SetColorSchemeToMaterial(Texture2D colorScheme1)
    {
        var renderer = GetComponent<MeshRenderer>();
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(mpb);
        mpb.SetTexture("_ColorScheme", colorScheme1);
        renderer.SetPropertyBlock(mpb);
    }


    public void GenerateOld()
    {
        var rnd = new Random();
        rnd.InitState(seed);
        
        var noiseTex = new RenderTexture(size, size, 16);
        var tempTex = new Texture2D(1, 1);
        tempTex.SetPixel(0, 0, new Color(0.5f, 0.5f, 0.5f));
        tempTex.Apply();
        
        Graphics.Blit(tempTex, noiseTex);
        var count = rnd.NextInt(40, 80);
        var brushes = whiteBrushes.Concat(blackBrushes).ToList();
        for (int i = 0; i < count; i++)
        {
            var brushIdx = rnd.NextInt(brushes.Count);
            var brush = brushes[brushIdx];
            var randomSize = rnd.NextFloat(1f, 4f); 
            var randomX = rnd.NextFloat(randomSize);
            var randomY = rnd.NextFloat(randomSize);
            var randomDepth = rnd.NextFloat(0.1f, 0.6f);  
            drawMaterial.mainTexture = brush; 
            drawMaterial.mainTextureScale = new Vector2(randomSize, randomSize);
            drawMaterial.color = new Color(1, 1, 1, randomDepth);
            for (var dx = -1; dx <= 1; dx++)
            {
                for (var dy = -1; dy <= 1; dy++)
                {
                    drawMaterial.mainTextureOffset = new Vector2((float)-randomX + dx*randomSize, (float)-randomY + dy*randomSize);
                    Graphics.Blit(brush, noiseTex, drawMaterial);//, new Vector2(randomSize, randomSize), new Vector2(randomX, randomY));
                    
                }
            }
        }
        this.noiseTex = noiseTex;
        //GetComponent<MeshRenderer>().sharedMaterial.mainTexture = noiseTex;
        if (originalMesh == null) originalMesh = GetComponent<MeshFilter>().sharedMesh;
        var mesh = originalMesh;

        var verticesList = new List<Vector3>();
        var normalsList = new List<Vector3>(); 
        var uvsList = new List<Vector2>();
        var colorList = new List<Color>();
        mesh.GetVertices(verticesList);
        mesh.GetNormals(normalsList);
        mesh.GetUVs(0, uvsList);

        var colors = GetColors(rnd);

        var t2d = GetRTPixels(noiseTex);
        for (int i = 0; i < mesh.vertexCount; i++) 
        {
            var u = uvsList[i].x*size;
            var v = uvsList[i].y*size;
            var height = t2d.GetPixel((int)u, (int)v).r;
            var pos = verticesList[i];
            var color = colors[0].color;
            for (int ci = 1; ci < colors.Count; ci++)
            {
                if (colors[ci].pos >= height)
                {
                    var mixa = (height - colors[ci - 1].pos) / (colors[ci].pos - colors[ci - 1].pos);
                    color = Color.Lerp(colors[ci - 1].color, colors[ci].color, mixa);
                    break;
                }
            }

            height = 2f * height - 1f;
            height = Mathf.Sign(height) * Mathf.Pow(Mathf.Abs(height), hpow);

            colorList.Add(color);
            var newPos = pos + normalsList[i] * height*0.1f;
            verticesList[i] = newPos;
        }

        var newMesh = new Mesh()
        {
            vertices = verticesList.ToArray(),
            triangles = mesh.triangles,
            uv = uvsList.ToArray(),
            colors = colorList.ToArray()
        };
        newMesh.RecalculateBounds();
        newMesh.RecalculateNormals();
        newMesh.RecalculateTangents();
        
        GetComponent<MeshFilter>().mesh = newMesh;
    }

    struct ColorPos
    {
        public float pos;
        public Color color;
    }

    Color GenColor(Random rnd, float h)
    {
        return Color.HSVToRGB(rnd.NextFloat(1), 0.8f, h);
        
    }

    List<ColorPos> GetColors(Random rnd)
    {
        var list = new List<ColorPos>();
        //first
        list.Add(new ColorPos()
        {
            pos = 0,
            color = GenColor(rnd, 0)  
        });

        for (int i = 0; i < 3; i++)
        {
            var pos = rnd.NextFloat(1f);
            list.Add(new ColorPos()
            {
                pos = pos,
                color = GenColor(rnd, pos)  
            });
            
        }
        
        //last
        list.Add(new ColorPos()
        {
            pos = 1f,
            color = GenColor(rnd, 1)  
        });
        return list;
    }
    
    static public Texture2D GetRTPixels(RenderTexture rt)
    {
        // Remember currently active render texture
        RenderTexture currentActiveRT = RenderTexture.active;

        // Set the supplied RenderTexture as the active one
        RenderTexture.active = rt;

        // Create a new Texture2D and read the RenderTexture image into it
        Texture2D tex = new Texture2D(rt.width, rt.height);
        tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
        tex.Apply();

        // Restore previously active render texture
        RenderTexture.active = currentActiveRT;
        return tex;
    }

    public void GenerateRandom()
    {
        seed = (uint)(UnityEngine.Random.value * 111111111);
        Generate();
    }
    
    public static Color[][] PixelsCached = new Color[20][];

    public static Color[] GetCachedColors(int idx, Texture2D tex)
    {
        var pxs = PixelsCached[idx];
        if (pxs == null)
        {
            pxs = tex.GetPixels();
            PixelsCached[idx] = pxs;
        }

        return pxs;
    }
}
