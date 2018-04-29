using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Console : MonoBehaviour {
    [SerializeField]
    private float randomRange = 3; // num seconds standing nearby
    [SerializeField]
    private float randomBase = 3;
    [SerializeField]
    public float guaranteeFixedTime = 10; // time that it's guaranteed not to be broken for

    [SerializeField]
    private float chanceOfBreaking = .5f; // in 10 seconds, half a chance of breaking I guess?
    private bool broken = false;
    public float timeLeftBroken = 0;
    public float timeSinceFixed = 0;
    [SerializeField]
    private GameObject particles;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timeSinceFixed += Time.deltaTime;
    }

    public void SetBroken()
    {
        // makes this broken
        broken = true;
        timeLeftBroken = Random.value * randomRange + randomBase;
        particles.SetActive(true);
    }

    public void SetFixed()
    {
        broken = false;
        timeSinceFixed = 0;
        particles.SetActive(false);
    }

    public bool GetBroken()
    {
        return broken;
    }
}
