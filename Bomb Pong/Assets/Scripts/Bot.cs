using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
   
    public Transform ball;
    public Transform aimTargetLeft;
    public Transform aimTargetRight;

    private float speed = 10.0f;
    private float force = 3.8f;
    private Vector3 targetPosition;
    

    // Start is called before the first frame update
    void Start()
    {
        //don't render aimTargets
        aimTargetLeft.gameObject.GetComponent<Renderer>().enabled = false;
        aimTargetRight.gameObject.GetComponent<Renderer>().enabled = false;

        targetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        targetPosition.x = ball.position.x;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Debug.Log("Opponent hit the ball!");
            Vector3 dir = aimTargetLeft.position - transform.position;
            Debug.Log("Opponent aimTarget.position " + aimTargetLeft.position);
            other.GetComponent<Rigidbody>().velocity = dir.normalized * force + new Vector3(0.0f, 3.3f, 0.0f);
        }
    }
}
