using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour {

	public float rotationSpeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
//		transform.Rotate (0, GameSettings.Speed * Time.deltaTime * rotationSpeed, 0);
		transform.Rotate (0, 20f * Time.deltaTime * rotationSpeed, 0);
	}
}
