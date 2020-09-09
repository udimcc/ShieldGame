using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerControls : MonoBehaviour
{
    public float speed = 3.0f;

    CharacterController2D controller;
    float movement;

    void Awake()
    {
        this.controller = this.GetComponent<CharacterController2D>();
    }

    void Update()
    {
        this.movement= Input.GetAxisRaw("Horizontal") * this.speed;
    }

    private void FixedUpdate()
    {
        this.controller.Move(this.movement * Time.fixedDeltaTime, false, false);
    }
}
