using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class EventController : MonoBehaviour
{
    public int MaxContinentHeight = 8;
    public int MinContinentHeight = 6;
    public int MinOceanDepth = 1;
    public int MaxOceanDepth = 3;

    //Good values are freq 1.2, lac 10, octaves 1;
    public int Octaves = 1;
    public float Frequency = 1.2f, Lacunarity = 10f, Persistence = 0.2f;

    private static TileObject _selectedTile = null;

    private enum Tool { EARTHBUILDER, SELECTOR };
    private static Tool _activeTool = Tool.SELECTOR;

    private static Color currentColor = new Color(1f, 1f, 0f, 0f);
    private static Color originalColor;
    private float _flashRate = 1f;
    private static int _currentSeed;

    public void SelectEarthBuilderTool()
    {
        if (_activeTool == Tool.EARTHBUILDER)
        {
            _activeTool = Tool.SELECTOR;
        }
        else
        {
            _activeTool = Tool.EARTHBUILDER;
        }
    }

    public void GenerateTerrain()
    {
        List<TileStack> stacks = TerrainController.GetStacks();
        int seed = Random.Range(0, 10000);
        PerlinNoise p = new PerlinNoise(Frequency, Lacunarity, Persistence, Octaves, seed, PerlinNoise.QualityMode.Medium);
        foreach (TileStack s in stacks)
        {
            // Tile earthTile = s.GetTile(TileType.EARTH);
            Vector3 position = s.GetPosition();
            double height = p.GetValue(position.x, position.y, position.z);
            height *= 10f;
            if (height > MaxContinentHeight)
            {
                height = MaxContinentHeight;
            }
            else if (height < MinContinentHeight)
            {
                height -= 2;
                if (height > MaxOceanDepth)
                {
                    height = MaxOceanDepth;
                }
            }
            if (height < MinOceanDepth)
            {
                height = MinOceanDepth;
            }
            s.ChangeTileTargetHeight((int)height, TileType.EARTH);
        }
    }

    public void Update()
    {
        float colorDelta = Time.deltaTime * _flashRate;
        if (currentColor.r + colorDelta > 1f || currentColor.r + colorDelta < 0.25f)
        {
            _flashRate = -_flashRate;
            colorDelta = -colorDelta;
        }
        currentColor.r += colorDelta;
        currentColor.g += colorDelta;
    }

    private static PerlinNoise noise;

    private static void CreateMountains()
    {
        if (_currentInterval >= _builderIntervalInSeconds)
        {
            List<TileStack> seen = new List<TileStack>();
            List<TileStack> unexplored = new List<TileStack>();
            unexplored.Add(_selectedTile.GetStack());
            for (int i = 0; i < _mountainBuilderRange; ++i)
            {
                for (int j = unexplored.Count - 1; j >= 0; --j)
                {
                    TileStack tile = unexplored[i];
                    float rnd = Random.Range(0f, 1f);
                    bool buildMountain = rnd <= (i / 10f);
                    if (buildMountain)
                    {
                        Vector3 position = tile.GetPosition();
                        float noiseValue = (float)noise.GetValue(position.x, position.y, position.z);
                        // if (noiseValue > Random.Range(0f, 1f))
                        // {
                            int heightDifference = (int)Mathf.Round(noiseValue);
                            // Debug.Log(heightDifference);
                        // }
                        // tile.ChangeTileHeight(heightDifference, TileType.EARTH);
                    }
                    seen.Add(tile);
                    unexplored.RemoveAt(j);
                    List<TileStack> neighbors = tile.GetNeighbors();
                    foreach (TileStack neighbor in neighbors)
                    {
                        if (!seen.Contains(neighbor))
                        {
                            unexplored.Add(neighbor);
                        }
                    }
                }
            }
            _currentInterval = 0f;
        }
        _currentInterval += Time.deltaTime;
    }

    private static int _mountainBuilderRange = 10;
    private static float _builderIntervalInSeconds = 0.5f, _currentInterval = _builderIntervalInSeconds;

    public static void ResetSelectedTile()
    {
        if (_selectedTile != null)
        {
            MeshRenderer oldRenderer = _selectedTile.GetComponent<MeshRenderer>(); ;
            oldRenderer.material.color = originalColor;
            _selectedTile = null;
        }
    }

    public static void UseTool(TileObject targetTile)
    {
        MeshRenderer newRenderer = targetTile.GetComponent<MeshRenderer>();
        if (targetTile != _selectedTile)
        {
            ResetSelectedTile();
            _currentInterval = _builderIntervalInSeconds;
            _selectedTile = targetTile;
            originalColor = newRenderer.material.color;
            _currentSeed = Random.Range(0, 100000);
            noise = new PerlinNoise(1.2f, 10f, 0.2f, 2, _currentSeed, 2);
        }
        newRenderer.material.color = originalColor + currentColor;
        switch (_activeTool)
        {
            case Tool.SELECTOR:
                break;
            case Tool.EARTHBUILDER:
                CreateMountains();
                break;
            default:
                break;
        }
    }
}
