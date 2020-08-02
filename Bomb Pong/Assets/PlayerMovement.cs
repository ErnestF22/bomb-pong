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

    public Transform aimTarget;
    private float fwdDelta = 0.0f;
    

    private float actualDistance;

    // Start is called before the first frame update
    void Start()
    {
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
            Vector3 dir = aimTarget.position - transform.position;
            Debug.Log("aimTarget.position " + aimTarget.position);
            other.GetComponent<Rigidbody>().velocity = dir.normalized;
        }
    }
}
