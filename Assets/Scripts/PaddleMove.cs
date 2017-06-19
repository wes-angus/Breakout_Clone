using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMove : MonoBehaviour
{
    Rigidbody2D rb;
    float xPos;
    public float paddleSpeed = 1;
    public float ballInfluenceFactor = 10;
    public float xDist;
    float startX;

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        startX = rb.position.x;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        xPos = rb.position.x + Input.GetAxis("Horizontal") * paddleSpeed;
        rb.MovePosition(new Vector2(Mathf.Clamp(xPos, -xDist + startX, xDist + startX), rb.position.y));
	}
}
