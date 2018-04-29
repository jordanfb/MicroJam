using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonScript : MonoBehaviour {
    [SerializeField]
    private PersonAnimator animator;
    [SerializeField]
    private Collider2D clickCollider;

    [SerializeField]
    private float walkSpeed = 1;

    private Vector2 goal;
    public bool walkingToGoal = false;

    public List<Vector2> path; // follow the path!

	// Use this for initialization
	void Start () {
		
	}

    public void Randomize()
    {
        animator.RandomizeColor();
    }
	
	// Update is called once per frame
	void Update () {
        /*if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Here");
            WalkToPlace(CameraScript.controlledCam.ScreenToWorldPoint(Input.mousePosition));
        }*/
        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.RandomizeColor();
        }
        if (!walkingToGoal && path.Count > 0)
        {
            WalkToPlace(path[0]);
        }
		if (walkingToGoal)
        {
            Vector2 dpos = goal - (Vector2)transform.position;
            animator.SetVelocity(dpos);
            if (dpos.magnitude > walkSpeed*Time.deltaTime)
            {
                dpos.Normalize();
                dpos *= walkSpeed*Time.deltaTime;
            } else if (path.Count == 0)
            {
                walkingToGoal = false;
                animator.SetAnimationState(PersonAnimator.AnimState.Idle);
            } else
            {
                // delete that point on the path
                path.RemoveAt(0);
                walkingToGoal = false;
            }
            transform.position = transform.position + (Vector3)dpos;
        }
	}

    void WalkToPlace(Vector2 pos)
    {
        walkingToGoal = true;
        goal = pos;
        animator.SetAnimationState(PersonAnimator.AnimState.Walking);
    }
}

[System.Serializable]
public class PersonData
{
    // this stores the data for the player? This is serialized
    public float hunger = 0; // 0 - 100
    public float happy = 100;
    public float alertality = 100;
    public float itchiness = 0;
    public float sanity = 100;
}