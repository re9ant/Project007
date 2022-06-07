using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Variables")]
    //[SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    
    [Header("DO NOT MESS")]
    [SerializeField] private Animator animator;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private bool backwards = false;

    private int speed_Hash;
    private int jump_Hash;
    private int grounded_Hash;
    private Vector3 move;


    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        speed_Hash = Animator.StringToHash("Speed");
        jump_Hash = Animator.StringToHash("Jump");
        grounded_Hash = Animator.StringToHash("Grounded");
    }
    
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.TransformDirection(Vector3.down));
        groundedPlayer = Physics.Raycast(ray, 1f);
        animator.SetBool(grounded_Hash, groundedPlayer);
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        move = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        float inputMagnitude = Mathf.Clamp01(move.magnitude);
        animator.SetFloat(speed_Hash, inputMagnitude, 0.05f, Time.deltaTime);

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
            animator.SetTrigger(jump_Hash);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        //controller.Move(playerVelocity * Time.deltaTime);
        if (backwards)
        {
            if(transform.rotation.y < 180)
            {
                Vector3 currRot = transform.rotation.eulerAngles;
                Vector3 targetRot = new Vector3(0, 180);
                Vector3 finalRot = Vector3.Lerp(currRot, targetRot, Time.deltaTime * 7);
                transform.rotation = Quaternion.Euler(finalRot);
            }
        }

        if (!backwards)
        {
            if (transform.rotation.y > 0)
            {
                Vector3 currRot = transform.rotation.eulerAngles;
                Vector3 targetRot = new Vector3(0, 0);
                Vector3 finalRot = Vector3.Lerp(currRot, targetRot, Time.deltaTime * 7);
                transform.rotation = Quaternion.Euler(finalRot);
            }
        }
    }

    private void OnAnimatorMove()
    {
        Vector3 velocity = animator.deltaPosition;
        velocity.x = velocity.z;
        velocity.z = 0;
        velocity.y = playerVelocity.y * Time.deltaTime;
        if(!groundedPlayer)
        {
            velocity.x = Input.GetAxis("Horizontal") * 0.01f;
        }
        controller.Move(velocity);
    }

}
