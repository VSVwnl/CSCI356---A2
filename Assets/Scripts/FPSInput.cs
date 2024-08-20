using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSInput : MonoBehaviour
{
    public float speed = 3.0f;
    public float sprintMultiplier = 3.5f;
    public float gravity = -9.8f;
    public float jumpSpeed = 15.0f;
    public float terminalVelocity = -20.0f;

    private CharacterController charController;
    private Animator animator;
    private float vertSpeed;
    private bool isJumping;  // Track whether the character is in the middle of a jump

    void Start()
    {
        charController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();  // Get the Animator component
    }

    void Update()
    {
        bool isSprinting = Input.GetMouseButton(1); // Right mouse button
        float currentSpeed = isSprinting ? speed * sprintMultiplier : speed;

        // Calculate movement
        float deltaX = Input.GetAxis("Horizontal") * currentSpeed;
        float deltaZ = Input.GetAxis("Vertical") * currentSpeed;
        Vector3 movement = new Vector3(deltaX, 0, deltaZ);

        // Set animation parameters
        float animationSpeed = new Vector3(deltaX, 0, deltaZ).magnitude;
        animator.SetFloat("Speed", animationSpeed);  // Update Speed parameter
        animator.SetBool("IsSprinting", isSprinting);  // Update Sprinting parameter

        // Handle jumping logic
        if (charController.isGrounded)
        {
            if (!isJumping)  // Only reset vertSpeed when not jumping
            {
                vertSpeed = -1f;  // Apply a small constant downward force to stay grounded
            }

            // Jump if grounded and the jump button is pressed
            if (Input.GetButtonDown("Jump"))
            {
                vertSpeed = jumpSpeed;
                animator.SetBool("IsJumping", true);  // Trigger jump animation
                isJumping = true;
            }
            else
            {
                animator.SetBool("IsJumping", false);  // Stop jump animation when grounded
                isJumping = false;
            }
        }
        else
        {
            // If in the air, apply gravity
            vertSpeed += gravity * Time.deltaTime;
            if (vertSpeed < terminalVelocity)
            {
                vertSpeed = terminalVelocity;
            }

            isJumping = true;  // Still in the air
        }

        // Apply movement including vertical speed (jump/fall)
        movement = Vector3.ClampMagnitude(movement, currentSpeed);
        movement.y = vertSpeed;
        movement *= Time.deltaTime;
        movement = transform.TransformDirection(movement);
        charController.Move(movement);
    }
}
