using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public bool scrollAtEdge;
	public float turnSpeed;		// Speed of camera turning when mouse moves in along an axis
	public float zoomSpeed;		// Speed of the camera going back and forth

	private float rotationY;
	private float rotationX;

	void Start() {
		UnityEngine.Cursor.lockState = CursorLockMode.None;

		rotationX = transform.localEulerAngles.y;
		rotationY = transform.localEulerAngles.x;
	}

	// Update is called once per frame
	void Update () {
		
		// Get the left mouse button
		if(Input.GetMouseButton(0))
		{

		}

		// On Right-Click, rotate the camera
		if(Input.GetMouseButtonDown(1))
		{
			rotationX += Input.GetAxis ("Mouse X") * turnSpeed * Time.deltaTime;
			rotationY += Input.GetAxis ("Mouse Y") * turnSpeed * Time.deltaTime;
			rotationY = Mathf.Clamp (rotationY, 15f, 90f);
			transform.localEulerAngles = new Vector3 (rotationY, rotationX, 0);
		}

		// Zoom with scroll wheel
		// TODO: Place limits on orthographicSize
		if (Input.GetAxis ("Mouse ScrollWheel") > 0)
			Camera.main.orthographicSize -= zoomSpeed;
		else if (Input.GetAxis ("Mouse ScrollWheel") < 0)
			Camera.main.orthographicSize += zoomSpeed;


		// Pan the camera if the mouse is at the edges
		if (scrollAtEdge) {
			if (Input.mousePosition.x > Screen.width - 50 && Input.mousePosition.x < Screen.width) {
				transform.position += Vector3.right * Camera.main.orthographicSize * Time.deltaTime * 0.5f;
			}
			if (Input.mousePosition.x < 50 && Input.mousePosition.x > 0) {
				transform.position += Vector3.left * Camera.main.orthographicSize * Time.deltaTime * 0.5f;
			}
			if (Input.mousePosition.y > Screen.height - 50 && Input.mousePosition.y < Screen.height) {
				transform.position += Vector3.forward * Camera.main.orthographicSize * Time.deltaTime * 0.5f;
			}
			if (Input.mousePosition.y < 50 && Input.mousePosition.y > 0) {
				transform.position += Vector3.back * Camera.main.orthographicSize * Time.deltaTime * 0.5f;
			}
		}

		// Pan the camera with WASD and the Arrow Keys
		if (Input.GetKey ("w") || Input.GetKey("up")) {
			transform.position += Vector3.forward * Camera.main.orthographicSize * Time.deltaTime;
		}
		if (Input.GetKey ("a") || Input.GetKey("left")) {
			transform.position += Vector3.left * Camera.main.orthographicSize * Time.deltaTime;
		}
		if (Input.GetKey ("s") || Input.GetKey("down")) {
			transform.position += Vector3.back * Camera.main.orthographicSize * Time.deltaTime;
		}
		if (Input.GetKey ("d") || Input.GetKey("right")) {
			transform.position += Vector3.right * Camera.main.orthographicSize * Time.deltaTime;
		}
	}
}
