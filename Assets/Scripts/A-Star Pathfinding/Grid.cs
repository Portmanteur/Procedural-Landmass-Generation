using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour { /*
public class Grid {
//*/	

	static Grid instance;

	public bool displayGridGizmos;
	//public Transform player;
//	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	public Vector3 worldCenter;
	Node[,] grid;

	float nodeDiameter;
	int gridSizeX, gridSizeY;

	void Start () {
		if (instance == null)
			instance = this;
	}

	/*
	public Grid(Vector3 _worldCenter, float _gridWorldSizeX, float _gridWorldSizeY, float _nodeRadius, bool _drawGizmos) {
		worldCenter = _worldCenter;
		gridWorldSize = new Vector2 (_gridWorldSizeX, _gridWorldSizeY);
		nodeRadius = _nodeRadius;
		displayGridGizmos = _drawGizmos;

		Awake ();
	}
	//*/

	public static Grid GridInstance {
		get {
			return instance; 
		}
	}

	void Awake() {
		if (instance == null)
			instance = this;
//		DontDestroyOnLoad (this.gameObject);
//		CreateGrid ();
	}

	public int MaxSize {
		get {
			return gridSizeX * gridSizeY;
		}
	}

	public void CreateGrid() {

		nodeDiameter = 2 * nodeRadius;
		gridSizeX = Mathf.RoundToInt (gridWorldSize.x / nodeDiameter);
		gridSizeY = Mathf.RoundToInt (gridWorldSize.y / nodeDiameter);

		grid = new Node[gridSizeX, gridSizeY];
		Vector3 worldBottomLeft = transform.position - (Vector3.right * gridWorldSize.x / 2) - (Vector3.forward * gridWorldSize.y / 2);
//		Vector3 worldBottomLeft = worldCenter - (Vector3.right * gridWorldSize.x / 2) - (Vector3.forward * gridWorldSize.y / 2);

		for (int y = 0; y < gridSizeY; y++) {
			for (int x = 0; x < gridSizeX; x++) {
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
//				bool walkable = !(Physics.CheckSphere (worldPoint, nodeRadius, unwalkableMask));
				grid [x, y] = new Node (worldPoint, x, y);
			}
		}
				
	}

	public List<Node> GetNeighbors(Node node) {
		List<Node> neighbors = new List<Node> ();

		for (int y = -1; y <= 1; y++) {
			for (int x = -1; x <= 1; x++) {
				if (x == 0 && y == 0) {
					continue;
				}
				int checkX = node.gridX + x;
				int checkY = node.gridY + y;
				if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
					neighbors.Add (grid [checkX, checkY]);
				}
			}
		}
		
		return neighbors;
	}

	public Node NodeFromGridPoint(int x, int y) {
		try {
//			(gridSizeY - y);
			return grid [x, gridSizeY - y - 1];
		}
		catch { 
			return new Node (transform.position, x, y);
//			return new Node (worldCenter, x, y);
		}
	}

	public Node NodeFromWorldPoint(Vector3 worldPosition) {
		float percentX = Mathf.Clamp01(worldPosition.x / gridWorldSize.x + 0.5f);
		float percentY = Mathf.Clamp01(worldPosition.z / gridWorldSize.y + 0.5f);

		int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

		return grid [x, y];
	}

	public void OnDrawGizmos() {
//		Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y)); 
		Gizmos.DrawWireCube(worldCenter, new Vector3(gridWorldSize.x, 1, gridWorldSize.y)); 
		if (grid != null && displayGridGizmos) {
			foreach (Node n in grid) {
				Gizmos.color = Color.white;
				if (n.gridX % 2 == 0 && n.gridY % 2 == 0)
					Gizmos.DrawCube (n.worldPosition, Vector3.one * nodeDiameter * 0.9f);
					
			}
		}
	}
}
