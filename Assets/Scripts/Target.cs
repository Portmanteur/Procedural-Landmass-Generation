using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		Node closestNode = Grid.GridInstance.NodeFromWorldPoint (transform.position);
		transform.position = Vector3.MoveTowards (transform.position, closestNode.worldPosition, 1f);	
	}
}
