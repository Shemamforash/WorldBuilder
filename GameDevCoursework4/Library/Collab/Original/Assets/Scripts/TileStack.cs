using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is a comment
public class TileStack {
	private List<TileObject> _tiles = new List<TileObject>();
	private List<TileStack> _neighbors = new List<TileStack>();
	public Vector3 center;
	private Vector3[] _originalVertices;

	public void AddNeighbor(TileStack s){
		if(!_neighbors.Contains(s)){
			_neighbors.Add(s);
		}
	}

	public List<TileStack> GetNeighbors(){
		return _neighbors;
	}

	public TileStack(Vector3[] _originalVertices){
		this._originalVertices = _originalVertices;
		center = (_originalVertices[0] + _originalVertices[1] + _originalVertices[2]) / 3;
	}

	public Vector3 GetPosition(){
		return center;
	}

	public Vector3[] GetOriginalVertices(){
		return _originalVertices;
	}

	public void AddTile(GameObject _tileObject, GenerateWorld _worldGenerator){
		_tileObject.AddComponent<TileObject>();
		TileObject t = _tileObject.GetComponent<TileObject>();
		t.Init(_tileObject, _worldGenerator, this);
		_tiles.Add(t);
	}

	public List<TileObject> GetTiles(){
		return _tiles;
	}

	public void ChangeTileHeight(int amount, TileType type){
		_tiles[(int)type].ChangeHeight(amount);
	}

	public void ChangeTileTargetHeight(int target, TileType type){
		_tiles[(int)type].ChangeTargetHeight(target);
	}

	public TileObject GetTile(TileType type){
		return _tiles[(int)type];
	}
}
