using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MeshGenerator : MonoBehaviour
{
    private const float MinTerrainHeight = 0;
    [SerializeField] private Shader _wireframeShader;
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private float _sizeX = 5;
    [SerializeField] private float _sizeY = 5;

    [SerializeField] private Vector2[] _octaveOffsets;
    
    private void Start()
    {
        _meshFilter.mesh = new();

        UpdateMesh(5, 1, 1, 1, 1, new EmptyFunction());

        for (int i = 0; i < _octaveOffsets.Length; i++)
        {
            var offset = _octaveOffsets[i];
            offset.x += Random.Range(-100f, 100f);
            offset.y += Random.Range(-100f, 100f);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="subdivisions"></param>
    /// <param name="noiseScale"></param>
    /// <param name="octaves"></param>
    /// <param name="persistance"></param>
    /// <param name="lacunarity"></param>
    /// <param name="mathFunction"></param>
    public void UpdateMesh(int subdivisions, float noiseScale, int octaves, float persistance, float lacunarity,
        MathFunction mathFunction)
    {
        _meshFilter.mesh.Clear();

        Vector3[] verticies = new Vector3[(subdivisions + 1) * (subdivisions + 1)];

        var xStep = _sizeX / subdivisions;
        var yStep = _sizeY / subdivisions;

        Random.InitState((int)DateTime.Now.Ticks);
        
        for (int x = 0; x <= subdivisions; x++)
        {
            for (int y = 0; y <= subdivisions; y++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;
                for (int i = 0; i < octaves; i++)
                {
                    var offset = _octaveOffsets[i];
                    var xPos = x * noiseScale * frequency + offset.x;
                    var yPos = y * noiseScale * frequency + offset.y;

                    float perlin = Mathf.PerlinNoise(xPos, yPos) * 2 - 1;

                    noiseHeight += perlin * amplitude;
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                verticies[y * (subdivisions + 1) + x] = new Vector3(x * xStep,
                    Mathf.Clamp(mathFunction.Math(noiseHeight), MinTerrainHeight, float.MaxValue), y * yStep);
            }
        }

        int[] triangles = new int[subdivisions * subdivisions * 6];

        int vertex = 0;
        int triangle = 0;

        for (int x = 0; x < subdivisions; x++)
        {
            for (int y = 0; y < subdivisions; y++)
            {
                triangles[triangle] = vertex;
                triangles[triangle + 1] = vertex + subdivisions + 1;
                triangles[triangle + 2] = vertex + 1;

                triangles[triangle + 3] = vertex + 1;
                triangles[triangle + 4] = vertex + subdivisions + 1;
                triangles[triangle + 5] = vertex + subdivisions + 2;
                vertex++;
                triangle += 6;
            }

            vertex++;
        }

        Vector2[] uv = new Vector2[(subdivisions + 1) * (subdivisions + 1)];

        for (int x = 0; x <= subdivisions; x++)
        {
            for (int y = 0; y <= subdivisions; y++)
            {
                uv[y * (subdivisions + 1) + x] =
                    new Vector2(x / (float)(subdivisions + 1), y / (float)(subdivisions + 1));
            }
        }


        _meshFilter.mesh.vertices = verticies;
        _meshFilter.mesh.triangles = triangles;
        _meshFilter.mesh.SetUVs(0, uv);
        _meshFilter.mesh.RecalculateNormals();
    }


    public void EnableWireframe(bool state)
    {
        if (state)
        {
            Material mat = new(_wireframeShader);
            var oldMaterials = _meshRenderer.sharedMaterials;
            var newMaterials = new Material[oldMaterials.Length + 1];
            Array.Copy(oldMaterials, newMaterials, oldMaterials.Length);
            newMaterials[^1] = mat;
            _meshRenderer.sharedMaterials = newMaterials;
        }
        else
        {
            _meshRenderer.sharedMaterials =
                _meshRenderer.sharedMaterials.Where(x => x.shader != _wireframeShader).ToArray();
        }
    }

    public void EnableTextures(bool state)
    {
        _meshRenderer.sharedMaterial.SetInt("_EnableTexture", state ? 1 : 0);
    }
}