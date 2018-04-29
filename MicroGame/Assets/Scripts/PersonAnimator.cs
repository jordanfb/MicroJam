using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonAnimator : MonoBehaviour {
    public enum AnimState
    {
        Idle, Walking, Fiddling,
    }



    [Header("Graphic Components")]
    [SerializeField]
    private Transform torso;
    [SerializeField]
    private Transform head;
    [SerializeField]
    private Transform eyes;
    [SerializeField]
    private Transform hands;
    [SerializeField]
    private Transform leftHand;
    [SerializeField]
    private Transform rightHand;

    [SerializeField]
    private SpriteRenderer torsoSprite;

    private float animationTime = 0;
    private AnimState state = AnimState.Walking;
    private Vector2 velocity = Vector2.left; // this is used to determine what direction to face etc.

    private Vector3 initialTorsoPos;
    private Vector3 initialHeadPos;
    private Vector3 initialEyesPos;
    private Vector3 initialHandsPos;
    private Vector3 initialLeftHandPos;
    private Vector3 initialRightHandPos;

	// Use this for initialization
	void Start () {
        // save local offsets
        initialTorsoPos = torso.localPosition;
        initialHeadPos = head.localPosition;
        initialEyesPos = eyes.localPosition;
        initialHandsPos = hands.localPosition;
        initialLeftHandPos = leftHand.localPosition;
        initialRightHandPos = rightHand.localPosition;
    }

    public void RandomizeColor()
    {
        torsoSprite.color = new Color(Random.value, Random.value, Random.value);
    }
	
	// Update is called once per frame
	void Update () {
        animationTime += Time.deltaTime;
        if (state == AnimState.Idle)
        {
            // torso.localScale = Vector3.one * (Mathf.Sin(animationTime * .4f)*.1f + 1);
            float sin = Mathf.Sin(animationTime * 2f);
            head.localPosition = initialHeadPos + Vector3.right * (sin * .01f);
            eyes.localPosition = initialEyesPos + Vector3.right * (sin * .02f);
            float cos = Mathf.Cos(animationTime * 2f);
            leftHand.localPosition = initialLeftHandPos + Vector3.up * (cos * .02f);
            rightHand.localPosition = initialRightHandPos - Vector3.up * (cos * .02f);
            hands.localPosition = initialHandsPos;
        }
        else if (state == AnimState.Walking)
        {
            float sin = Mathf.Sin(animationTime * 2f);
            float cos = Mathf.Cos(animationTime * 2f);

            float sideScale = velocity.x;
            if (sideScale != 0)
            {
                sideScale = sideScale / Mathf.Abs(sideScale);
            }


            head.localPosition = initialHeadPos + sideScale * Vector3.right * (sin * .02f + .1f) + Vector3.up*(Mathf.Sin(animationTime * 10f)*.02f);
            eyes.localPosition = initialEyesPos + sideScale * Vector3.right * (sin * .01f + .1f);
            leftHand.localPosition = initialLeftHandPos + Vector3.up * (Mathf.Cos(animationTime * 10f) * .02f) + Vector3.right * .1f;
            rightHand.localPosition = initialRightHandPos - Vector3.up * (Mathf.Cos(animationTime * 10f) * .02f) + Vector3.right * -.1f;
            hands.localPosition = initialHandsPos + sideScale * Vector3.right * (sin * .015f + .1f) + Vector3.up * (Mathf.Sin(animationTime * 10f) * .015f);

        }
    }

    public void SetVelocity(Vector2 velocityIn)
    {
        velocity = velocityIn;
        SetFacingDown(velocityIn.y < 0);
    }

    public void SetFacingDown(bool isFacingDown)
    {
        if (!isFacingDown)
        {
            // then disable the things that should be disabled and swap z order
            eyes.gameObject.SetActive(false);
            initialTorsoPos.z = Mathf.Abs(initialTorsoPos.z);
            initialHeadPos.z = Mathf.Abs(initialHeadPos.z);
            initialHandsPos.z = Mathf.Abs(initialHandsPos.z);
            initialLeftHandPos.z = Mathf.Abs(initialLeftHandPos.z);
            initialRightHandPos.z = Mathf.Abs(initialRightHandPos.z);
        }
        else
        {
            // reenable things
            eyes.gameObject.SetActive(true);
            initialTorsoPos.z = -Mathf.Abs(initialTorsoPos.z);
            initialHeadPos.z = -Mathf.Abs(initialHeadPos.z);
            initialHandsPos.z = -Mathf.Abs(initialHandsPos.z);
            initialLeftHandPos.z = -Mathf.Abs(initialLeftHandPos.z);
            initialRightHandPos.z = -Mathf.Abs(initialRightHandPos.z);
        }
    }

    public void SetAnimationState(AnimState stateIn)
    {
        state = stateIn;
        if (stateIn == AnimState.Idle || stateIn == AnimState.Fiddling)
        {
            Debug.Log("Set facing down idle fiddling");
            SetFacingDown(true);
        }
    }
}
