﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public float velocity = 2f;
    private Rigidbody2D rigidBody2d;

	// Use this for initialization
	void Start () {
        rigidBody2d = GetComponent<Rigidbody2D>();
        rigidBody2d.velocity = Vector2.left * velocity;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D()
    {
        rigidBody2d.velocity = Vector2.left * 0;
    }
}
