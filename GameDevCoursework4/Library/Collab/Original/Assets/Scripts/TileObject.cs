using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is another comment
public enum TileType { EARTH, WATER, CLOUD, NONE };

public class TileObject : MonoBehaviour
{
    private GameObject _tileObject;
    private MeshRenderer _renderer;
    private GenerateWorld _worldGenerator;
    private TileType _currentType = TileType.EARTH;
    private int _currentHeight = 1, _currentOrigin = 0; //Default tile height is 1
    private int _maxHeight = 40;
	private TileStack _parentStack;

    public void Init(GameObject _tileObject, GenerateWorld _worldGenerator, TileStack _parentStack)
    {
        this._tileObject = _tileObject;
        this._worldGenerator = _worldGenerator;
        this._currentType = TileType.EARTH;
		this._parentStack = _parentStack;
        _renderer = _tileObject.GetComponent<MeshRenderer>();
        _renderer.material = _worldGenerator.GetMaterial(_currentType);
    }	

    public TileStack GetStack() {
        return _parentStack;
    }

    public void ChangeHeight(int _amount)
    {
        int targetHeight = _currentHeight + _amount;
        if (targetHeight < 1)
        {
            targetHeight = 1;
        }
        else if (targetHeight > _maxHeight)
        {
            targetHeight = _maxHeight;
        }
        _currentHeight = targetHeight;
        _worldGenerator.GenerateTileMesh(_currentOrigin, _currentHeight, _parentStack.GetOriginalVertices(), _tileObject);
    }

    public void ChangeTargetHeight(int targetHeight)
    {
        if (targetHeight < 1)
        {
            targetHeight = 0;
        }
        else if (targetHeight > _maxHeight)
        {
            targetHeight = 20;
        }
        _currentHeight = targetHeight;
        _worldGenerator.GenerateTileMesh(_currentOrigin, _currentHeight, _parentStack.GetOriginalVertices(), _tileObject);
		Color oldColor = _tileObject.GetComponent<MeshRenderer>().material.color;
		oldColor.r = 1f / 255f * (111f + targetHeight * 20f);
		oldColor.g = 1f / 255f * (75f + targetHeight * 20f);
		oldColor.b = 1f / 255f * (43f + targetHeight * 20f);
		_tileObject.GetComponent<MeshRenderer>().material.color = oldColor;
    }
}

