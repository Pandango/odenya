using UnityEngine;
using System.Collections;

public class ScaleWidthCamera : MonoBehaviour {

	public float targetScreenWidth = 1136f;
	public float targetScreenHeight = 640f;

	private float targetAspect;
	private Camera camera;

	void Awake(){
		targetAspect = targetScreenWidth / targetScreenHeight;
		camera = GetComponent<Camera>();
		camera.orthographicSize = 4.5f;
	}

	void Start () {
		float windowAspect = (float)Screen.width / (float)Screen.height;
		float scaleHeight = windowAspect / targetAspect;

		if (scaleHeight < 1.0f){  
			camera.orthographicSize = camera.orthographicSize / scaleHeight;
		}
	}
}
