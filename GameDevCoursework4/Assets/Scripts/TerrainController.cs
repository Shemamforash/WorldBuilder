using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainController : MonoBehaviour
{
    private static List<TileStack> _stacks = new List<TileStack>();
	//This is the distance to the nearest 3 stacks
	private static float neighborDistance = 0.095f;

    public static void AddStack(TileStack s)
    {
        _stacks.Add(s);
    }

    public static List<TileStack> GetStacks()
    {
        return _stacks;
    }

	public static void AssignNeighbors(){
		for(int i = 0; i < _stacks.Count; ++i){
			TileStack outerStack = _stacks[i];
			for(int j = 0; j < _stacks.Count; ++j){
				if(i == j){
					continue;
				}
				TileStack innerStack = _stacks[j];
				float distance = Vector3.Distance(outerStack.GetPosition(), innerStack.GetPosition());
				if(distance < neighborDistance){
					outerStack.AddNeighbor(innerStack);
				}
			}
		}
	}
}
