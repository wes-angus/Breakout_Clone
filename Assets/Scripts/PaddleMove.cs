using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMove : MonoBehaviour
{
    Rigidbody2D rb;
    public float paddleSpeed = 1;
    public float ballInfluenceFactor = 10;
    public float xDist;
    float lastMove = 0;
    float xPos, startX, prevPos;
    bool sticky = false;
    float stickTimer = 0;
    public int stickTime = 30;

    public bool Sticky
    {
        get
        {
            return sticky;
        }
    }

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        startX = rb.position.x;
	}

    // Update is called once per frame
    private void Update()
    {
        if (sticky)
        {
            if(Time.time >= stickTimer)
            {
                sticky = false;
                print("Stop Sticking");
            }
        }
    }

    void FixedUpdate ()
    {
        prevPos = rb.position.x;

        if (Input.GetAxis("Horizontal") != 0)
        {
            xPos = rb.position.x + Input.GetAxis("Horizontal") * paddleSpeed;
            rb.position = (new Vector2(Mathf.Clamp(xPos, -xDist + startX, xDist + startX), rb.position.y));
        }

        lastMove = rb.position.x - prevPos;
	}

    public void StartSticking()
    {
        sticky = true;
        stickTimer = Time.time + stickTime;
    }

    public float getLastMove()
    {
        return lastMove;
    }
}
