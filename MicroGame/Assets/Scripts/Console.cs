using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Console : MonoBehaviour {
    [SerializeField]
    private float randomRange = 3; // num seconds standing nearby
    [SerializeField]
    private float randomBase = 3;
    [SerializeField]
    private float chanceOfBreaking = .5f; // in 10 seconds, half a chance of breaking I guess?
    private bool broken = false;
    public float timeLeftBroken = 0;
    [SerializeField]
    private GameObject particles;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (broken && !particles.activeSelf)
        {
            particles.SetActive(true);
        } else if (!broken && particles.activeSelf)
        {
            particles.SetActive(false);
        }

        /*if (broken)
        {
            timeLeftBroken -= Time.deltaTime;
            if (timeLeftBroken <= 0)
            {
                broken = false;
                particles.SetActive(false);
            }
        }*/
	}

    public void SetBroken()
    {
        // makes this broken
        broken = true;
        timeLeftBroken = Random.value * randomRange + randomBase;
    }

    public void SetFixed()
    {
        broken = false;
    }

    public bool GetBroken()
    {
        return broken;
    }
}
