using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {
	
	public Transform target;
//	float speed = 25f;
	Vector3[] path;
	int targetIndex;

	void Start() {
//		PathRequestManager.RequestPath (transform.position, target.position, OnPathFound); 
	}

	public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
		if (pathSuccessful) {
			path = newPath;
			StopCoroutine ("FollowPath");
			StartCoroutine ("FollowPath");
		}
	}

	IEnumerator FollowPath() {
		if (path.Length == 0)
			yield return null;
		Vector3 currentWaypoint = path[0];

		float terrainSpeed;

		while (true) {
			if (transform.position == currentWaypoint) {
				targetIndex++;
				if (targetIndex >= path.Length) {
					yield break;
				}
				currentWaypoint = path[targetIndex];
			}

			terrainSpeed = 1f / Mathf.Sqrt(Grid.GridInstance.NodeFromWorldPoint (transform.position).Terrain.moveCost);

//			transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, GameSettings.Speed * Time.deltaTime * terrainSpeed);
			transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, 20f * Time.deltaTime * terrainSpeed);
			yield return null;
		}
	}

	void Update() {
		Node closestNode = Grid.GridInstance.NodeFromWorldPoint (transform.position);
		Vector3 moveTowards = new Vector3 (transform.position.x, closestNode.worldPosition.y, transform.position.z);
		transform.position = Vector3.MoveTowards (transform.position, moveTowards, 1f);	

		if (!hasReachedTarget (closestNode))
			PathRequestManager.RequestPath (transform.position, target.position, OnPathFound);
	}	

	bool hasReachedTarget(Node closestNode) {
		bool targetReached = false;
		Node targetNode = Grid.GridInstance.NodeFromWorldPoint (target.transform.position);

		if (Mathf.Abs (targetNode.gridX - closestNode.gridX) <= 1 && Mathf.Abs (targetNode.gridY - closestNode.gridY) <= 1)
			targetReached = true;

		return targetReached;
	}

	public void OnDrawGizmos() {

		Color pathColor = Color.white;
		pathColor.a = 0.5f;

		if (path != null) {
			for (int i = targetIndex; i < path.Length; i++) {
				Gizmos.color = pathColor;
				Gizmos.DrawCube (path [i], Vector3.one);

				if (i == targetIndex) {
					Gizmos.DrawLine (transform.position, path [i]);
				} else {
					Gizmos.DrawLine (path [i - 1], path [i]);
				}
			}
		}
	}
}
