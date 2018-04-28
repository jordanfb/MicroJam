using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    [SerializeField]
    private float zoomSpeed = -1; // how fast the camera zooms in and out
    [SerializeField]
    private float maxZoom = 1;
    [SerializeField]
    private float moveSpeed = 1; // I dont think I'm using this

    private Camera cam;

    public static Camera controlledCam; // cause I'm lazy this is to find the camera for things

	// Use this for initialization
	void Start () {
        cam = GetComponent<Camera>();
        controlledCam = cam;
    }
	
	// Update is called once per frame
	void Update () {
        float mouseZoom = Input.mouseScrollDelta.y;
        cam.orthographicSize += mouseZoom * zoomSpeed;
        cam.orthographicSize = Mathf.Max(maxZoom, cam.orthographicSize);
	}
}
