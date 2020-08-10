using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
   
    public Transform ball;
    public Transform aimTarget;

    private float speed = 10.0f;
    private float force = 4.0f;
    private Vector3 targetPosition;
    

    // Start is called before the first frame update
    void Start()
    {
        aimTarget.gameObject.GetComponent<Renderer>().enabled = false; //don't render aimTarget

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
            Vector3 dir = aimTarget.position - transform.position;
            Debug.Log("Opponent aimTarget.position " + aimTarget.position);
            other.GetComponent<Rigidbody>().velocity = dir.normalized * force + new Vector3(0.0f, 3.3f, 0.0f);
        }
    }
}
