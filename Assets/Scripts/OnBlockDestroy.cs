using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBlockDestroy : MonoBehaviour
{
    public int scoreAmount;
    GameObject background;

    // Use this for initialization
    void Start()
    {
        background = GameObject.FindGameObjectWithTag("Background");
    }

    private void OnDestroy()
    {
        if (background != null)
        {
            background.GetComponent<ScoreUpdate>().UpdateScore(scoreAmount);
        }
    }
}
