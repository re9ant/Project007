using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Variables")]
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    
    [Header("DO NOT MESS")]
    [SerializeField] private Animator animator;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private bool backwards = false;

    private int speed_Hash;
    private Vector3 move;


    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        speed_Hash = Animator.StringToHash("Speed");
    }
    
    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        move = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move.x < 0.0f)
        {
            backwards = true;
        }
        else if(move.x > 0.0f)
        {
            backwards = false;
        }

        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
        if (backwards)
        {
            transform.rotation = new Quaternion(0, 180, 0, 0);
        }

        if (!backwards)
        {
            transform.rotation = new Quaternion();
        }
        SetAnimations();
    }

    private void SetAnimations()
    {
        Debug.Log(move);
        animator.SetFloat(speed_Hash, Mathf.Abs(move.x));
    }

}
