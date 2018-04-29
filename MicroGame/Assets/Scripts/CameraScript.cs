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
    public static CameraScript instance;
    public static Camera controlledCam; // cause I'm lazy this is to find the camera for things

    private float ssIntensity = 0;
    private float ssTime = 0;

	// Use this for initialization
	void Start () {
        cam = GetComponent<Camera>();
        controlledCam = cam;
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            // destroy yourself probably
            Debug.LogError("OH GOD AT LEAST TWO COPIES OF CAMERASCRIPT");
        }
    }
	
	// Update is called once per frame
	void Update () {
        float mouseZoom = Input.mouseScrollDelta.y;
        cam.orthographicSize += mouseZoom * zoomSpeed;
        cam.orthographicSize = Mathf.Max(maxZoom, cam.orthographicSize);

        if (ssTime > 0)
        {
            ssTime -= Time.deltaTime;
            transform.localPosition = new Vector3(Random.value, Random.value, 0)*ssIntensity;
        }
        else
        {
            transform.localPosition = Vector3.zero;
        }
	}

    void AddScreenShake(float intensity, float time)
    {
        ssIntensity = intensity;
        ssTime = time;
    }
}
