using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool useInitialCameraDistance = true;
    public float zDistance = 2.3f;
    public float mouseSpeed = 1.0f;
    public Rigidbody playerRb;

    public Transform aimTargetLeft;
    public Transform aimTargetRight;

    public Transform ball;
    

 

    public GameObject wallBehind;

    //private float fwdDelta = 0.0f;
    private float shotForce = 4.6f;    

    private float actualDistance;
    private float initY;

    private ShotManager shotManager;

    

    // Start is called before the first frame update
    void Start()
    {
        //don't render aimTargets
        aimTargetLeft.gameObject.GetComponent<Renderer>().enabled = false; 
        aimTargetRight.gameObject.GetComponent<Renderer>().enabled = false;

        //wallBehind.GetComponent<Renderer>().enabled = false;

        initY = gameObject.transform.position.y;
        playerRb = GetComponent<Rigidbody>();
        Vector3 toObjectVector = transform.position - Camera.main.transform.position;
        Vector3 linearDistanceVector = Vector3.Project(toObjectVector, Camera.main.transform.forward);
        if (useInitialCameraDistance)
        {
            actualDistance = linearDistanceVector.magnitude;
        }
        else
        {
            actualDistance = zDistance;
        }

        shotManager = GetComponent<ShotManager>();
    }

    // Update is called once per frame
    void Update()
    {        
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = actualDistance;

        //if (Input.GetKey(KeyCode.W))
        //    fwdDelta += 0.05f;
        //else if (Input.GetKey(KeyCode.S))
        //    fwdDelta -= 0.05f;

        //mousePosition.z += fwdDelta;

        transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
        //transform.rotation

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Debug.Log("Player hit the ball!");
            
            

            Vector3 contactRelPosition = other.transform.position-playerRb.position;
            if (contactRelPosition.x <= 0.0f) //hit on left part of racket -> ball goes to the right
            {
                Vector3 dir = aimTargetRight.position - transform.position;
                Debug.Log("Collision position " + contactRelPosition.ToString("F8"));
                //Debug.Log("aimTarget.position " + aimTargetRight.position);
                //other.GetComponent<Rigidbody>().velocity = dir.normalized * (shotForce - (0.03f * (PlayerPrefs.GetInt("countdown") - GameLogic.countdown))) 
                //    + new Vector3(0.0f, 2.5f-(0.1f*(PlayerPrefs.GetInt("countdown")-GameLogic.countdown)), 0.0f);
                float deltaHeight = 3.0f - Mathf.Max(0.15f, initY - transform.position.y) - (0.025f * ((PlayerPrefs.GetInt("countdown") - GameLogic.countdown) > 15.0f ? -3.4f : PlayerPrefs.GetInt("countdown") - GameLogic.countdown));
                if (PlayerPrefs.GetInt("countdown") - GameLogic.countdown > 15.0f)
                    shotForce += 0.25f;
                other.GetComponent<Rigidbody>().velocity = dir.normalized * shotForce + new Vector3(0.0f, deltaHeight, 0.0f);
            }
            else //hit on right part of racket -> ball goes to the left
            {
                
                Vector3 dir = aimTargetLeft.position - transform.position;
                Debug.Log("Collision position " + contactRelPosition.ToString("F8"));
                //Debug.Log("aimTarget.position " + aimTargetLeft.position);
                //other.GetComponent<Rigidbody>().velocity = dir.normalized * (shotForce - (0.03f * (PlayerPrefs.GetInt("countdown") - GameLogic.countdown))) 
                //    + new Vector3(0.0f, 2.5f -(0.1f*(PlayerPrefs.GetInt("countdown") - GameLogic.countdown)), 0.0f);
                float deltaHeight = 3.0f - Mathf.Max(0.15f, initY - transform.position.y) - (0.025f * ((PlayerPrefs.GetInt("countdown") - GameLogic.countdown) > 15.0f ? -3.4f : PlayerPrefs.GetInt("countdown") - GameLogic.countdown));
                if (PlayerPrefs.GetInt("countdown") - GameLogic.countdown > 15.0f)
                    shotForce += 0.25f;
                other.GetComponent<Rigidbody>().velocity = dir.normalized * shotForce + new Vector3(0.0f, deltaHeight, 0.0f);

            }
            
        }
    }

    
}
