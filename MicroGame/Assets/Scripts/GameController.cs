using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    [SerializeField]
    private Console[] itemsToBreak;

    [SerializeField]
    private Collider2D shipCollider;
    [SerializeField]
    float maxSelectDistance = 2;

    [SerializeField]
    private PersonScript[] crew;
    [SerializeField]
    private Transform healthbar;

    private PersonScript selectedCrewMember;

    List<Vector2> currentPath = new List<Vector2>();
    List<Vector3> currentPathLineRenderer = new List<Vector3>();
    Vector2 prevPoint;
    private LineRenderer lineRenderer;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < crew.Length; i++)
        {
            crew[i].Randomize();
        }
        lineRenderer = GetComponent<LineRenderer>();

    }
	
	// Update is called once per frame
	void Update () {
        HandleMouseInput();
        UpdateGame();
	}

    void UpdateGame()
    {
        // break items
        for (int i = 0; i < itemsToBreak.Length; i++)
        {
            if (itemsToBreak[i].timeLeftBroken > 0)
            {
                // check if any person is nearby
                for (int j = 0; j < crew.Length; j++)
                {
                    if (crew[j].)
                    itemsToBreak[i].timeLeftBroken -= Time.deltaTime;
                    if (itemsToBreak[i].timeLeftBroken <= 0)
                    {
                        itemsToBreak[i].SetFixed();
                        break;
                    }
                }

            }
        }
    }

    void HandleMouseInput()
    {
        // deal with selecting characters, etc.
        if (Input.GetMouseButtonDown(0))
        {
            // left click down, see if they selected a character
            Vector3 mousePos = CameraScript.controlledCam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            PersonScript selected = null;
            float minDistance = maxSelectDistance;
            for (int i = 0; i < crew.Length; i++)
            {
                if (((crew[i].transform.position - mousePos).sqrMagnitude < minDistance))
                {
                    minDistance = (crew[i].transform.position - mousePos).sqrMagnitude;
                    selected = crew[i];
                }
            }
            if (selected != null)
            {
                if (MouseInShip())
                {
                    // then do shit with them
                    selectedCrewMember = selected;
                    currentPath = new List<Vector2>();
                    currentPathLineRenderer = new List<Vector3>();
                    prevPoint = MousePoint();
                    currentPath.Add(prevPoint);
                    currentPathLineRenderer.Add(prevPoint);
                    lineRenderer.positionCount = 0;
                    selectedCrewMember.path.Clear();
                    selectedCrewMember.walkingToGoal = false;
                }
            } else
            {
                selectedCrewMember = null;
            }
        } else if (Input.GetMouseButton(0))
        {
            // then draw a path I guess?
            if (selectedCrewMember != null && MouseInShip() && (prevPoint - MousePoint()).magnitude > .2)
            {
                Vector2 dpos = MousePoint() - prevPoint;
                RaycastHit2D hit = Physics2D.Raycast(prevPoint, dpos, dpos.magnitude);
                if (!hit)
                {
                    prevPoint = MousePoint();
                    currentPath.Add(MousePoint());
                    currentPathLineRenderer.Add(prevPoint);
                    lineRenderer.positionCount = currentPath.Count;
                    lineRenderer.SetPositions(currentPathLineRenderer.ToArray());
                }
            }

        } else if (Input.GetMouseButtonUp(0))
        {
            if (selectedCrewMember != null)
            {
                // set the current path
                selectedCrewMember.path = currentPath;
                selectedCrewMember.walkingToGoal = false;
            }
        } else if (Input.GetMouseButtonDown(1))
        {
            // cancel the current path and deselect the crew member
            selectedCrewMember = null;
        }
    }

    Vector2 MousePoint()
    {
        return CameraScript.controlledCam.ScreenToWorldPoint(Input.mousePosition);
    }

    bool MouseInShip()
    {
        // returns whether or not the mouse is in the ship and not in the walls etc.
        return !Physics2D.Raycast(MousePoint(), Vector2.zero, 0);
    }
}
