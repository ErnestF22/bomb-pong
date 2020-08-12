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
    public Transform serveTargetPlayerSide;


    

    private float serveForce = 4.0f;


    public GameObject wallBehind;

    private float fwdDelta = 0.0f;
    private float shotForce = 3.8f;    

    private float actualDistance;

    

    private ShotManager shotManager;

    void Serve()
    {
        Debug.Log("Serving");
        Vector3 dir = serveTargetPlayerSide.position - transform.position;
        ball.GetComponent<Rigidbody>().velocity = dir.normalized * serveForce + new Vector3(0.0f, 3.0f, 0.0f);
    }

    // Start is called before the first frame update
    void Start()
    {
        //don't render aimTargets
        aimTargetLeft.gameObject.GetComponent<Renderer>().enabled = false; 
        aimTargetRight.gameObject.GetComponent<Renderer>().enabled = false;
        serveTargetPlayerSide.gameObject.GetComponent<Renderer>().enabled = false;

        wallBehind.GetComponent<Renderer>().enabled = false;

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

        Serve();

        shotManager = GetComponent<ShotManager>();
    }

    // Update is called once per frame
    void Update()
    {        
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = actualDistance;

        if (Input.GetKey(KeyCode.W))
            fwdDelta += 0.05f;
        else if (Input.GetKey(KeyCode.S))
            fwdDelta -= 0.05f;

        mousePosition.z += fwdDelta;

        transform.position = Camera.main.ScreenToWorldPoint(mousePosition);

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
                other.GetComponent<Rigidbody>().velocity = dir.normalized * shotForce + new Vector3(0.0f, 3.3f, 0.0f);
            }
            else //hit on right part of racket -> ball goes to the left
            {
                
                Vector3 dir = aimTargetLeft.position - transform.position;
                Debug.Log("Collision position " + contactRelPosition.ToString("F8"));
                //Debug.Log("aimTarget.position " + aimTargetLeft.position);
                other.GetComponent<Rigidbody>().velocity = dir.normalized * shotForce + new Vector3(0.0f, 3.3f, 0.0f);

            }
            
        }
    }

    
}
