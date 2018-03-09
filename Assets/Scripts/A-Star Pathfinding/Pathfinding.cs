using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour {

//	public Transform seeker, target;

	PathRequestManager requestManager;
	Grid grid;

	void Awake() {
		requestManager = GetComponent<PathRequestManager> ();
//		grid = GetComponent<Grid> ();
//		grid = GetComponent<MapGenerator> ().MapGrid;
	}

//	void Update() {
////		if (Input.GetButtonDown ("Jump")) {
//			FindPath (seeker.position, target.position); 
////		}
//	}

	public void StartFindPath(Vector3 startPos, Vector3 targetPos) {
		StartCoroutine (FindPath (startPos, targetPos));
	}

	IEnumerator FindPath(Vector3 startPos, Vector3 targetPos) {

		Vector3[] waypoints = new Vector3[0];
		bool pathSuccess = false;

		if (grid == null)
			grid = Grid.GridInstance;

		if (grid == null)
			print ("Still Null");

//		print (grid.MaxSize);

		Node startNode = grid.NodeFromWorldPoint (startPos);
		Node targetNode = grid.NodeFromWorldPoint (targetPos);


		Heap<Node> openSet = new Heap<Node> (grid.MaxSize);
		HashSet<Node> closedSet = new HashSet<Node> ();

		openSet.Add (startNode);

		while (openSet.Count > 0) {

			Node currentNode = openSet.RemoveFirst ();
			closedSet.Add (currentNode);

			if (currentNode == targetNode) {
				pathSuccess = true;
//				RetracePath (startNode, targetNode);
//				return;
				break;
			}

			foreach (Node neighbor in grid.GetNeighbors(currentNode)) {
				if (closedSet.Contains (neighbor)) {
					continue;
				}

				int newMovementCostToNeighbor = currentNode.gCost + GetDistance (currentNode, neighbor) + currentNode.Terrain.moveCost;
				if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains (neighbor)) { 
					neighbor.gCost = newMovementCostToNeighbor;
					neighbor.hCost = GetDistance (neighbor, targetNode);
					neighbor.parent = currentNode;

					if (!openSet.Contains (neighbor))
						openSet.Add (neighbor);
					else
						openSet.UpdateItem (neighbor);
				}
			}
		}
		yield return null;
		if (pathSuccess) {
			waypoints = RetracePath (startNode, targetNode);
		}
		requestManager.FinishedProcessingPath (waypoints, pathSuccess);
	}

	Vector3[] RetracePath(Node startNode, Node endNode) {
		List<Node> path = new List<Node> ();
		Node currentNode = endNode;

		while (currentNode != startNode) {
			path.Add (currentNode);
			currentNode = currentNode.parent;
		}

		Vector3[] waypoints = SimplifyPath (path);
		Array.Reverse (waypoints);
		return waypoints;
	}

	Vector3[] SimplifyPath(List<Node> path) {
		List<Vector3> waypoints = new List<Vector3> ();
		Vector2 directionOld = Vector2.zero;

		for (int i = 1; i < path.Count; i++) {
			Vector2 directionNew = new Vector2 (path [i - 1].gridX - path [i].gridX, path [i - 1].gridY - path [i].gridY);
			if (directionNew != directionOld) 
				waypoints.Add (path [i].worldPosition);
			directionOld = directionNew;
		}
		return waypoints.ToArray ();
	}

	int GetDistance(Node a, Node b) {
		int dx = Mathf.Abs (a.gridX - b.gridX);
		int dy = Mathf.Abs (a.gridY - b.gridY);

		return (dx > dy) ? 14 * dy + 10 * (dx - dy) : 14 * dx + 10 * (dy - dx);
	}

}
