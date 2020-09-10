using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerControls : MonoBehaviour
{
    public float speed = 3.0f;

    CharacterController2D controller;
    float movement = 0;
    bool isJumping = false;

    void Awake()
    {
        this.controller = this.GetComponent<CharacterController2D>();
    }

    void Update()
    {
        this.movement = Input.GetAxisRaw("Horizontal") * this.speed;

        if (Input.GetButtonDown("Jump"))
        {
            this.isJumping = true;
        }
    }

    private void FixedUpdate()
    {
        this.controller.Move(this.movement * Time.fixedDeltaTime, this.isJumping);

        if (this.isJumping)
        {
            this.isJumping = false;
        }
    }
}
