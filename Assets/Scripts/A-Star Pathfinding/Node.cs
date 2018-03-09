using UnityEngine;
using System.Collections;

public class Node : IHeapItem<Node> {

//	public bool walkable;
	public Vector3 worldPosition;

	public int gridX;
	public int gridY;

	public int gCost;
	public int hCost;

	public Node parent;
	int heapIndex;

	TerrainType terrain;
	float elevation;

	public Node( Vector3 _worldPos, int x, int y) {
		worldPosition = _worldPos;
		gridX = x;
		gridY = y;
	}

	public int fCost {
		get {
			return gCost + hCost;
		}
	}

	public float Elevation {
		get { return elevation; }
		set {
			elevation = value;
			worldPosition.y = value * 10 + 10f;
		}
	}

	public TerrainType Terrain { get; set; }

//	public int TerrainPenalty {
//		get {
//			return terrain

	public int HeapIndex { get; set; }

	public int CompareTo(Node node) {
		int compare = fCost.CompareTo (node.fCost);
		if (compare == 0)
			compare = hCost.CompareTo (node.hCost);
		return -compare;
	}
}
