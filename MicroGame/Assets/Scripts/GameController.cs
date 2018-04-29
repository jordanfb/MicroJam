using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    [SerializeField]
    private float distanceToRepair = 4;

    [SerializeField]
    private float zPositionOfLines = 1.5f;

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

    // these are variables used for trying to balance the game
    private int numConsoles = 0;
    private int numConsolesBroken = 0;
    private int numConsolesBeingRepaired = 0;
    private int numCrew = 0;
    private float timeSinceBreak = 0;
    private int numCrewWorking = 0;
    private float timeSinceFix = 0;
    private float gameLength = 0;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < crew.Length; i++)
        {
            crew[i].Randomize();
        }
        lineRenderer = GetComponent<LineRenderer>();


        numConsoles = itemsToBreak.Length;
        numCrew = crew.Length;
    }
	
	// Update is called once per frame
	void Update () {
        timeSinceBreak += Time.deltaTime;
        timeSinceFix += Time.deltaTime;
        gameLength += Time.deltaTime;

        HandleMouseInput();
        RepairItems();
        BreakItems();
	}

    void BreakItems()
    {
        for (int i = 0; i < itemsToBreak.Length; i++)
        {
            if (!itemsToBreak[i].GetBroken() && itemsToBreak[i].timeSinceFixed > itemsToBreak[i].guaranteeFixedTime)
            {
                // consider breaking it
                itemsToBreak[i].SetBroken(); // lol just break it ATM.
                timeSinceBreak = 0;
            }
        }
    }

    void RepairItems()
    {
        numConsolesBroken = 0;
        numCrewWorking = 0;
        numConsolesBeingRepaired = 0;
        for (int i = 0; i < itemsToBreak.Length; i++)
        {
            if (itemsToBreak[i].timeLeftBroken > 0)
            {
                numConsolesBroken++;
                // check if any person is nearby
                bool beingRepaired = false;
                for (int j = 0; j < crew.Length; j++)
                {
                    if ((crew[j].transform.position - itemsToBreak[i].transform.position).magnitude <= distanceToRepair)
                    itemsToBreak[i].timeLeftBroken -= Time.deltaTime;
                    numCrewWorking++;
                    beingRepaired = true;
                    if (itemsToBreak[i].timeLeftBroken <= 0)
                    {
                        timeSinceFix = 0;
                        itemsToBreak[i].SetFixed();
                        break;
                    }
                }
                if (beingRepaired)
                {
                    numConsolesBeingRepaired++;
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
                    Vector3 pos = prevPoint; // to get the lines to draw at the correct depth
                    pos.z = zPositionOfLines;
                    currentPathLineRenderer.Add(pos);
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
                    Vector3 pos = prevPoint; // to get the lines to draw at the correct depth
                    pos.z = zPositionOfLines;
                    currentPathLineRenderer.Add(pos);
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
