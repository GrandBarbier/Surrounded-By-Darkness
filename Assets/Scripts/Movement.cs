using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public Animator animator;

    public float speed = 6f;

    public float turnSmoothTime = 0.1f;

    private float turnSmoothVelocity;

    public bool isGrounded;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    float TurnSmoothVelocity;

    public float gravity = -9;
    private Vector3 velocity;
    public float jumpHeight = 3.0f;

    public Transform cameraFollowTranfom;
    public float sensibility;

    private PlayerInput playerInput;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.actions["Jump"].performed += context => Jump();
    }

    // Update is called once per frame
    void Update()
    {
        
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        
        //float horizontal = Input.GetAxisRaw("Horizontal");
        //float vertical = Input.GetAxisRaw("Vertical");
        Vector2 v = playerInput.actions["Move"].ReadValue<Vector2>();
        Vector3 direction = new Vector3(v.x, 0, v.y);//new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f,  angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
            
            animator.SetBool("IsWalking", true);
            
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }

        velocity.y -= gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

    }

    public void Jump()
    {
        if (isGrounded)
        {
            Debug.Log("jump!");
            velocity.y = Mathf.Sqrt(jumpHeight * gravity);
        }
    }

    public void RotateCamera()
    {
        Vector2 v = playerInput.actions["CameraMove"].ReadValue<Vector2>();
        cameraFollowTranfom.rotation *= Quaternion.AngleAxis(v.x * sensibility, Vector3.up);
        
        cameraFollowTranfom.rotation *= Quaternion.AngleAxis(v.y * sensibility, Vector3.right);

        var angles = cameraFollowTranfom.localEulerAngles;

        angles.z = 0;

        var angle = cameraFollowTranfom.localEulerAngles.x;
        
        //Clamp rotation
        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }else if (angle < 180 && angle < 40)
        {
            angles.x = 40;
        }
    }
}

