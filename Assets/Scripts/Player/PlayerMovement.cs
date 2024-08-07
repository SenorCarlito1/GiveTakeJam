using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    private bool groundedPlayer;
    private float origSpeed;
    private Vector3 move;
    private Vector3 playerVeclocity;

    [Header("----Components----")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject woodenBuilding;
    //private cameraControl camera;
    

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
    private PlayerHealth playerHealth;
    private PlayerAttack attackScript;

    private void Start()
    {
        origSpeed = fowardSpeed;
        //camera = GetComponentInChildren<cameraControl>();
        animator = GetComponent<Animator>();

        playerHealth = GetComponent<PlayerHealth>();
        attackScript = GetComponentInChildren<PlayerAttack>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt)){playerHealth.TakeDamage(1);}
   
        Movement();
        Sprint();
        Jump();
    }
    private void FixedUpdate()
    {
       
        //RaycastHit hit;
        //if(Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, Mathf.Infinity))
        //{
           
        //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
        //    Debug.Log("Did Hit");
        //}
        //else
        //{
        //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
        //    Debug.Log("Did not Hit");
        //}
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

    public void ColliderOn()
    {
        if (GameManager.instance.currentTool == 0)
        {
            attackScript.fistCollider.enabled = true;
        }
        else
        {
            attackScript.toolCollider.enabled = true;
        }
    }

    public void ColliderOff()
    {
        if (GameManager.instance.currentTool == 0)
        {
            attackScript.fistCollider.enabled = false;
        }
        else
        {
            attackScript.toolCollider.enabled = false;
        }
    }
}
