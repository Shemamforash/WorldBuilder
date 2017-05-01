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
    private static PerlinNoise noise;

    private static int _mountainBuilderRange = 6;
    private static float _builderIntervalInSeconds = 0.5f, _currentInterval = _builderIntervalInSeconds;


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
        PerlinNoise p = new PerlinNoise(Frequency, Lacunarity, Persistence, Octaves);
        foreach (TileStack s in stacks)
        {
            // Tile earthTile = s.GetTile(TileType.EARTH);
            Vector3 position = s.GetPosition();
            int height = (int)Mathf.Round((float)(p.GetValue(position.x, position.y, position.z) * 10f));
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
            s.ChangeTileTargetHeight(height, TileType.EARTH);
        }
    }

    public void Start()
    {
        GeneratePerlin();
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

    private static void CreateMountains()
    {
        if (_currentInterval >= _builderIntervalInSeconds)
        {
            HashSet<string> seen = new HashSet<string>();
            List<TileStack> unexplored = new List<TileStack>();
            unexplored.Add(_selectedTile.GetStack());
            for (int i = 0; i < _mountainBuilderRange; ++i)
            {
                for (int j = unexplored.Count - 1; j >= 0; --j)
                {
                    TileStack tile = unexplored[j];
                    float rnd = Random.Range(0f, 1f);
                    rnd = Mathf.Pow(rnd, 2.71828f);
					rnd *= _mountainBuilderRange;
                    if (rnd > i)
                    {
						Vector3 pos = tile.GetPosition();
						int height = (int)Mathf.Round((float)noise.GetValue(pos.x, pos.y, pos.z) * 2f);
                        tile.ChangeTileHeight(height, TileType.EARTH);
                    }
                    seen.Add(tile.GetId());
                    unexplored.RemoveAt(j);
                    List<TileStack> neighbors = tile.GetNeighbors();
                    foreach (TileStack neighbor in neighbors)
                    {
                        if (!seen.Contains(neighbor.GetId()))
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

    public static void ResetSelectedTile()
    {
        if (_selectedTile != null)
        {
            MeshRenderer oldRenderer = _selectedTile.GetComponent<MeshRenderer>(); ;
            oldRenderer.material.color = originalColor;
            _selectedTile = null;
        }
    }

    public static void GeneratePerlin()
    {
        noise = new PerlinNoise(1f, 5f, 0.2f, 2);
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
