using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreUpdate : MonoBehaviour
{
    int bonus = 0;
    TextMesh scoreText;
    bool lost = false;

    public bool Lost
    {
        get
        {
            return lost;
        }
    }

    // Use this for initialization
    void Start()
    {
        scoreText = transform.Find("scoreText").GetComponent<TextMesh>();
        scoreText.text = "Score: " + PlayerStats.score;
    }

    private void Awake()
    {
        for (int i = PlayerStats.lives; i < 5; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (lost)
        {
            if (Input.GetButtonDown("Jump"))
            {
                PlayerStats.lives = 5;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    public void UpdateScore(int amount)
    {
        PlayerStats.score += amount + bonus;
        scoreText.text = "Score: " + PlayerStats.score;
        bonus++;
    }

    public void ResetMult()
    {
        PlayerStats.lives--;
        Destroy(transform.GetChild(PlayerStats.lives).gameObject);
        if (PlayerStats.lives < 1)
        {
            PlayerStats.score = 0;
            scoreText.text = "Score: " + PlayerStats.score + "\t\t You Lost...";
            lost = true;
        }
        bonus = 0;
    }
}
