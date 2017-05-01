using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileStack {
	private List<TileObject> _tiles = new List<TileObject>();
	private List<TileStack> _neighbors = new List<TileStack>();
	public Vector3 center;
	private Vector3[] _originalVertices;
	private string id;

	public string GetId(){
		return id;
	}

	public void AddNeighbor(TileStack s){
		if(!_neighbors.Contains(s)){
			_neighbors.Add(s);
		}
	}

	public List<TileStack> GetNeighbors(){
		return _neighbors;
	}

	public TileObject GetTileBelow(TileType type){
		if(type == 0){
			return null;
		}
		return _tiles[(int)(type - 1)];
	}

	public TileObject GetTileAbove(TileType type){
		try {
			return _tiles[(int)(type + 1)];
		} catch {
			return null;
		}
	}

	public TileStack(Vector3[] _originalVertices){
		this._originalVertices = _originalVertices;
		center = (_originalVertices[0] + _originalVertices[1] + _originalVertices[2]) / 3;
		id = (center.x + center.y + center.z).ToString();
	}

	public Vector3 GetPosition(){
		return center;
	}

	public Vector3[] GetOriginalVertices(){
		return _originalVertices;
	}

	public void AddTile(GameObject _tileObject, GenerateWorld _worldGenerator, TileType type, int targetHeight){
		_tileObject.AddComponent<TileObject>();
		TileObject t = _tileObject.GetComponent<TileObject>();
		t.Init(_tileObject, _worldGenerator, this, type);
		t.ChangeTargetHeight(targetHeight);
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
