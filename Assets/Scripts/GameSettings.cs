using UnityEngine;
using System.Collections;

public class GameSettings : MonoBehaviour {

	public static float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static float Speed { 
		get { 
			return speed; 
		} 
		set { 
			speed = value; 
		} 
	}
}
