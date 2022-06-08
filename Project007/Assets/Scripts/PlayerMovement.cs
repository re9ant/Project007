using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerState { Idle, Moving, Falling };

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Debuging")]
    public PlayerState playerState = PlayerState.Idle;

    [Header("Movement Variables")]
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float horizontalJumpMultipler = 1.25f;
    [SerializeField] private float idleRotationSpeed = 1.0f;
    [SerializeField] private float jogRotationSpeed = 3.0f;
    [SerializeField] private float gravityValue = -9.81f;
    
    [Header("DO NOT MESS")]
    [SerializeField] private Animator animator;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private int speed_Hash;
    private int angle_Hash;
    private int jump_Hash;
    private int grounded_Hash;

    private float moveAxis;
    private float turnAxis;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        speed_Hash = Animator.StringToHash("Speed");
        angle_Hash = Animator.StringToHash("Angle");
        jump_Hash = Animator.StringToHash("Jump");
        grounded_Hash = Animator.StringToHash("Grounded");
    }
    
    void Update()
    {
        //Inputs
        moveAxis = Input.GetAxis("Vertical");
        turnAxis = Input.GetAxis("Horizontal");

        //Detecting if the player is on ground by castng a ray
        Ray ray = new Ray(transform.position, transform.TransformDirection(Vector3.down));
        groundedPlayer = Physics.Raycast(ray, 1f);
        animator.SetBool(grounded_Hash, groundedPlayer);
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        //Detecting PlayerState
        if (moveAxis == 0)
        {
            if(groundedPlayer)
            {
                playerState = PlayerState.Idle;
            }
        }

        else if (moveAxis != 0)
        {
            playerState = PlayerState.Moving;
        }

        if (!groundedPlayer)
        {
            playerState = PlayerState.Falling;
        }

        //Sending animator values
        animator.SetFloat(speed_Hash, moveAxis, 0.05f, Time.deltaTime);
        animator.SetFloat(angle_Hash, turnAxis, 0.05f, Time.deltaTime);

        //Handling Jump
        if (Input.GetButtonDown("Jump") && groundedPlayer && moveAxis >= 0)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            animator.SetTrigger(jump_Hash);
        }

        //Gravity
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        //Rotate Player
        transform.Rotate(0, turnAxis * GetRelevantRotationSpeeds() * Time.deltaTime, 0);
    }

    private float GetRelevantRotationSpeeds()
    {
        if (playerState == PlayerState.Idle)
        {
            return idleRotationSpeed;
        }
        else
        {
            return jogRotationSpeed;
        }
    }

    private void OnAnimatorMove()
    {
        Vector3 velocity = animator.deltaPosition;
        if(!groundedPlayer)
        {
            velocity.x = moveAxis * horizontalJumpMultipler * Time.deltaTime;
        }
        Vector3 move = transform.forward * velocity.magnitude;
        move *= (moveAxis < 0) ? -1 : 1;
        controller.Move(move);
    }

}
