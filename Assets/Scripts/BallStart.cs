using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallStart : MonoBehaviour
{
    Rigidbody2D rb;
    BoxCollider2D bc;
    public float speedScale = 1;
    bool ballInPlay = false;
    bool flagStart = false;
    public float yMin;
    float spawnY;
    PaddleMove pm;
    Rigidbody2D paddleRB;
    Vector2 vel, xDir, yDir, xDir_origin1, xDir_origin2, yDir_origin1, yDir_origin2;
    LayerMask ignoreLayer;
    int blockCount;
    float winTimer = 0;
    public float winDelay = 0.5f;
    bool won = false;

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        spawnY = rb.position.y;
        GameObject paddle = GameObject.FindGameObjectWithTag("Player");
        paddleRB = paddle.GetComponent<Rigidbody2D>();
        pm = paddle.GetComponent<PaddleMove>();
        blockCount = GameObject.FindGameObjectsWithTag("Block").Length;
	}

    private void Update()
    {
        if (!ballInPlay)
        {
            if (Input.GetButtonDown("Jump"))
            {
                ballInPlay = true;
                flagStart = true;
            }
        }
        else
        {
            if (!won)
            {
                if (blockCount < 1)
                {
                    winTimer = Time.time + winDelay;
                    won = true;
                }
            }
            else
            {
                if (Time.time > winTimer)
                {
                    //TODO: Load next level
                    SceneManager.LoadScene(0);
                }
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        if (!ballInPlay)
        {
            rb.position = new Vector2(paddleRB.position.x, spawnY);
        }
        else if (flagStart)
        {
            rb.AddForce((Vector2.up * speedScale));
            Vector2 temp = new Vector2(Input.GetAxis("Horizontal") * speedScale, 0);
            rb.AddForce(temp);
            flagStart = false;
        }
        else if (rb.position.y < yMin)
        {
            rb.position = new Vector2(paddleRB.position.x, spawnY);
            rb.velocity = Vector2.zero;
            ballInPlay = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        vel = rb.velocity;
        xDir = new Vector2(rb.velocity.x, 0).normalized;
        yDir = new Vector2(0, rb.velocity.y).normalized;
        ignoreLayer = ~(1 << LayerMask.NameToLayer("Ball"));
        xDir_origin1 = new Vector2(rb.position.x, rb.position.y - bc.bounds.extents.y);
        xDir_origin2 = new Vector2(rb.position.x, rb.position.y + bc.bounds.extents.y);
        yDir_origin1 = new Vector2(rb.position.x - bc.bounds.extents.x, rb.position.y);
        yDir_origin2 = new Vector2(rb.position.x + bc.bounds.extents.x, rb.position.y);

        if (Physics2D.Raycast(rb.position, xDir, bc.bounds.extents.x + 0.02f, ignoreLayer))
        {
            vel = new Vector2(-vel.x, vel.y);
        }
        else if (Physics2D.Raycast(xDir_origin1, xDir, bc.bounds.extents.x + 0.02f, ignoreLayer))
        {
            vel = new Vector2(-vel.x, vel.y);
        }
        else if (Physics2D.Raycast(xDir_origin2, xDir, bc.bounds.extents.x + 0.02f, ignoreLayer))
        {
            vel = new Vector2(-vel.x, vel.y);
        }
        if (Physics2D.Raycast(rb.position, yDir, bc.bounds.extents.y + 0.02f, ignoreLayer))
        {
            vel = new Vector2(vel.x, -vel.y);
        }
        else if (Physics2D.Raycast(yDir_origin1, yDir, bc.bounds.extents.y + 0.02f, ignoreLayer))
        {
            vel = new Vector2(vel.x, -vel.y);
        }
        else if (Physics2D.Raycast(yDir_origin2, yDir, bc.bounds.extents.y + 0.02f, ignoreLayer))
        {
            vel = new Vector2(vel.x, -vel.y);
        }

        if (other.gameObject.tag.Equals("Player"))
        {
            rb.AddForce(new Vector2(Input.GetAxis("Horizontal") * speedScale * pm.paddleSpeed * pm.ballInfluenceFactor, 0));
        }

        if (other.gameObject.tag.Equals("Block"))
        {
            Destroy(other.gameObject);
            blockCount--;
        }

        rb.velocity = vel;
    }
}
