using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpeedClamp : MonoBehaviour
{
    Rigidbody2D rb;
    public float maxBallSpeed;

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if(rb.velocity.magnitude > maxBallSpeed)
        {
            rb.velocity *= (maxBallSpeed / rb.velocity.magnitude);
        }
	}
}
