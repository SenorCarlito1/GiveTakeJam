using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool groundedPlayer;
    private float origSpeed;
    private Vector3 move;
    private Vector3 playerVeclocity;

    [Header("----Components----")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private AudioSource audioSource;
    private cameraControl camera;


    [Header("----Movement Parameters----")]
    [SerializeField] private float fowardSpeed;
    [SerializeField] private float backSpeed;
    [SerializeField] private float sprintMod;

    [Header("----Jump Parameters----")]
    [SerializeField] private float jumpHeight;
    [SerializeField] private float gravityValue;
    private int jumpedTimes;
    [SerializeField] private int jumpMax;

    Animator animator;

    private void Start()
    {
        origSpeed = fowardSpeed;
        camera = GetComponentInChildren<cameraControl>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Movement();
        Sprint();
        Jump();
    }

    private void Movement()
    {
        groundedPlayer = controller.isGrounded;

        if (groundedPlayer)
        {
            if (playerVeclocity.y < 0)
            {
                playerVeclocity.y = 0f;
                jumpedTimes = 0;
            }
        }

        move = (transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical"));

        float speed;

        float dot = Vector3.Dot(move, transform.forward);
        if (dot > 0.9)
        {
            speed = fowardSpeed;
            
        }
        else
        {
            speed = backSpeed;
        }

        // Run Animation Logic
        if (Input.GetButton("Vertical"))
        {
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }
        
        controller.Move(move * Time.deltaTime * speed);

        playerVeclocity.y -= gravityValue * Time.deltaTime;
        controller.Move(playerVeclocity * Time.deltaTime);
    }

    private void Sprint()
    {
        if (Input.GetButtonDown("Sprint")) 
        {
            fowardSpeed *= sprintMod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            fowardSpeed = origSpeed;
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && jumpedTimes < jumpMax)
        {
            jumpedTimes++;
            playerVeclocity.y = jumpHeight;
        }
    }
}
