using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))] // enforces dependency on character controller
[AddComponentMenu("Control Script/FPS Input")]  // add to the Unity editor's component menu
public class FPSInput : MonoBehaviour
{
    // movement sensitivity
    public float speed = 3.0f;
    public float sprintMultiplier = 3.5f;

    // gravity setting
    public float gravity = -9.8f;

    // reference to the character controller
    private CharacterController charController;

    // for jump
    public float jumpSpeed = 15.0f;
    public float terminalVelocity = -20.0f;
    private float vertSpeed;

    // Start is called before the first frame update
    void Start()
    {
        // get the character controller component
        charController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isSprinting = Input.GetMouseButton(1); // Right mouse button

        // Determine movement speed
        float currentSpeed = isSprinting ? speed * sprintMultiplier : speed;
        Debug.Log($"Sprinting: {isSprinting}, Current Speed: {currentSpeed}");

        // changes based on WASD keys
        float deltaX = Input.GetAxis("Horizontal") * currentSpeed;
        float deltaZ = Input.GetAxis("Vertical") * currentSpeed;
        Vector3 movement = new Vector3(deltaX, 0, deltaZ);

        // for jump
        if (Input.GetButtonDown("Jump") && charController.isGrounded)
        {
            vertSpeed = jumpSpeed;
        }
        else if (!charController.isGrounded)
        {
            vertSpeed += gravity * 5 * Time.deltaTime;

            if (vertSpeed < terminalVelocity)
            {
                vertSpeed = terminalVelocity;
            }
        }

        // make diagonal movement consistent
        movement = Vector3.ClampMagnitude(movement, currentSpeed);

        // add gravity in the vertical direction
        movement.y = vertSpeed;

        // ensure movement is independent of the framerate
        movement *= Time.deltaTime;

        // transform from local space to global space
        movement = transform.TransformDirection(movement);

        // pass the movement to the character controller
        charController.Move(movement);
    }
}
