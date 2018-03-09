using UnityEngine;
using System.Collections;

public class SitOnTheGround : MonoBehaviour {

	void Update () {
		Node closestNode = Grid.GridInstance.NodeFromWorldPoint (transform.position);
		transform.position = Vector3.MoveTowards (transform.position, closestNode.worldPosition - Vector3.up * 5, 1f);	
	}
}
