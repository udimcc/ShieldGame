using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerControls : MonoBehaviour
{
    public float speed = 350f;
    public GameObject[] shields;

    CharacterController2D controller;
    float movement = 0;
    bool isJumping = false;
    uint activeShieldIndex = 0;

    private void Start()
    {
        this.SwitchToShield(this.activeShieldIndex);
    }

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

        if (Input.mouseScrollDelta.y > 0)
        {
            this.activeShieldIndex++;

            if (this.activeShieldIndex >= this.shields.Length)
            {
                this.activeShieldIndex = 0;
            }

            this.SwitchToShield(this.activeShieldIndex);
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            if (this.activeShieldIndex == 0)
            {
                this.activeShieldIndex = (uint)this.shields.Length;
            }

            this.activeShieldIndex--;

            this.SwitchToShield(this.activeShieldIndex);
        }
    }

    private void FixedUpdate()
    {
        this.controller.Move(this.movement, this.isJumping);

        if (this.isJumping)
        {
            this.isJumping = false;
        }
    }

    private void SwitchToShield(uint shieldIndex)
    {
        foreach (GameObject shield in this.shields)
        {
            shield.SetActive(false);
        }

        this.shields[shieldIndex].SetActive(true);
    }
}
