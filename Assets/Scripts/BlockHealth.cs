using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BlockHealth : MonoBehaviour
{
    private int health = 3;
    TextMesh health_tMesh;

    public int Health
    {
        get
        {
            return health;
        }

        set
        {
            health = value;
            health_tMesh.text = health.ToString();
        }
    }

    // Use this for initialization
    void Start ()
    {
        health_tMesh = transform.GetChild(0).GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
