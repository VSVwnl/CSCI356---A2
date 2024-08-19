using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : MonoBehaviour
{
    public float speed = 3.0f;
    public float obstacleRange = 3.0f;
    public float rotSpeed = 10.0f;

    Quaternion direction;       // wandering direction
    bool isRotating = false;    // rotate over a number of frames

    bool isMoving = true;
    int movingCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        // start in a random direction
        float angle = Random.Range(-180.0f, 180.0f);
        direction = Quaternion.LookRotation(Quaternion.Euler(0.0f, angle, 0.0f) * transform.forward);
        isRotating = true;
    }

    void OnDrawGizmos()
    {
        // draw a red line gizmo to indicate collision avoidance distance
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * obstacleRange);
    }

    // Update is called once per frame
    void Update()
    {
        // if the agent is rotating
        if (isRotating)
        {
            isMoving = false;

            // rotate the agent over several frames
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                   direction, rotSpeed * Time.deltaTime);

            // if the agent within a certain angle of the correct direction
            if (Quaternion.Angle(transform.rotation, direction) < 1.0f)
            {
                isRotating = false;
            }
        }
        else
        {
            isMoving = true;
            movingCount = 0;

            // move the agent
            transform.Translate(0, 0, speed * Time.deltaTime);
        }

        // collision avoidance
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // cast a sphere to check whether it collides with anything
        if (Physics.SphereCast(ray, 0.75f, out hit))
        {
            // if the collision is within the collision avoidance range
            if (hit.distance < obstacleRange)
            {
                // choose a random angle
                float angle = Random.Range(-110.0f, 110.0f);

                if (!isMoving)
                {
                    movingCount++;
                    if (movingCount > 5)
                    {
                        angle = 45.0f;
                    }
                }

                // set the direction based on the angle
                direction = Quaternion.LookRotation(Quaternion.Euler(0.0f, angle, 0.0f) * transform.forward);
                isRotating = true;
            }
        }
    }
}
