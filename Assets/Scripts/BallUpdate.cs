using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallUpdate : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D ballC;
    public float speedScale = 1;
    public bool ballInPlay = false;
    bool flagStart = false;
    public float yMin;
    public float spawnY = -1.2385f;
    PaddleMove pm;
    Rigidbody2D paddleRB;
    LayerMask ignoreLayer;
    int blockCount;
    float winTimer = 0;
    public float winDelay = 0.5f;
    bool won = false;
    BlockHealth bh;
    int powerup;
    int sceneNum;
    public GameObject ballPrefab;
    float xOffset = 0;
    ScoreUpdate su;

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        ballC = GetComponent<Collider2D>();
        GameObject paddle = GameObject.FindGameObjectWithTag("Player");
        paddleRB = paddle.GetComponent<Rigidbody2D>();
        pm = paddle.GetComponent<PaddleMove>();
        blockCount = GameObject.FindGameObjectsWithTag("Block").Length;
        su = GameObject.FindGameObjectWithTag("Background").GetComponent<ScoreUpdate>();
	}

    private void Update()
    {
        if (!ballInPlay)
        {
            if (!su.Lost)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    ballInPlay = true;
                    flagStart = true;
                }
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
                    rb.velocity *= 0.2f;
                }
            }
            else
            {
                if (Time.time >= winTimer)
                {
                    sceneNum = SceneManager.GetActiveScene().buildIndex;
                    if (sceneNum < 2)
                    {
                        SceneManager.LoadScene(sceneNum + 1);
                    }
                    else
                    {
                        SceneManager.LoadScene(0);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        if (!ballInPlay)
        {
            rb.position = new Vector2(paddleRB.position.x + xOffset, spawnY);
        }
        else if (flagStart)
        {
            rb.velocity = Vector2.up * speedScale;
            if (pm.getLastMove() > 0)
            {
                rb.velocity += new Vector2(Mathf.Ceil(pm.getLastMove()) * speedScale, 0);
            }
            else
            {
                rb.velocity += new Vector2(Mathf.Floor(pm.getLastMove()) * speedScale, 0);
            }
            flagStart = false;
        }
        else if (rb.position.y < yMin)
        {
            if (GameObject.FindGameObjectsWithTag("Ball").Length > 1)
            {
                Destroy(gameObject);
            }
            else
            {
                su.ResetMult();
                rb.position = new Vector2(paddleRB.position.x, spawnY);
                rb.velocity = Vector2.zero;
                ballInPlay = false;
                xOffset = 0;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (rb.velocity.magnitude != 0)
        {
            if (collision.gameObject.tag.Equals("Player"))
            {
                if (!pm.Sticky)
                {
                    rb.velocity += new Vector2(pm.getLastMove() * speedScale * pm.ballInfluenceFactor, 0);
                }
                else
                {
                    rb.velocity = Vector2.zero;
                    ballInPlay = false;
                    xOffset = rb.position.x - collision.gameObject.GetComponent<Rigidbody2D>().position.x;
                }
            }

            if (collision.gameObject.tag.Equals("Block"))
            {
                DestroyBlock(collision.gameObject);
            }
            else if (collision.gameObject.tag.Equals("MetalBlock"))
            {
                collision.gameObject.GetComponent<Animator>().SetTrigger("hit");
                bh = collision.gameObject.GetComponent<BlockHealth>();
                bh.Health--;
                if (bh.Health < 1)
                {
                    DestroyBlock(collision.gameObject);
                }
            }
        }
    }

    //Vector2 calcBounce(Vector2 vel)
    //{
    //    if (xBounce)
    //    {
    //        vel = new Vector2(-vel.x, vel.y);
    //    }
    //    if (yBounce)
    //    {
    //        vel = new Vector2(vel.x, -vel.y);
    //    }
    //    return vel;
    //}

    void DestroyBlock(GameObject block)
    {
        powerup = Random.Range(0, 12);
        if (powerup == 0)
        {
            print(GameObject.FindGameObjectsWithTag("Ball").Length);
            if (GameObject.FindGameObjectsWithTag("Ball").Length < 3)
            {
                GameObject ballCopy = Instantiate(gameObject, block.transform.position, Quaternion.identity);
                ballCopy.GetComponent<Rigidbody2D>().velocity = Vector2.one * speedScale;
                ballCopy.GetComponent<BallUpdate>().ballInPlay = true;
            }
        }
        else if (powerup == 1)
        {
            pm.StartSticking();
            print("Start Sticking");
        }
        if (block.tag.Equals("Block"))
        {
            blockCount = GameObject.FindGameObjectsWithTag("Block").Length
                /*+ GameObject.FindGameObjectsWithTag("MetalBlock").Length*/ - 1;
        }
        Destroy(block);
        print("Blocks left: " + blockCount);
    }
}
