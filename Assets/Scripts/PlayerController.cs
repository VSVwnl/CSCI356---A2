using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;

    void Update()
    {
        // Handle running animation
        if (Input.GetKey(KeyCode.W)) // Change to your run key/button
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        // Handle jump animation
        if (Input.GetKeyDown(KeyCode.Space)) // Change to your jump key/button
        {
            animator.SetTrigger("Jump");
        }
    }
}
