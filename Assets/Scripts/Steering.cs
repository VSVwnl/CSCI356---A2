using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering : MonoBehaviour
{
    public Transform target;
    bool hasTarget = false;
    public float maxSpeed = 10.0f;
    public float rotSpeed = 10.0f; 
    float mass = 1.0f;
    Vector3 currentVelocity = Vector3.zero;

    public enum SteeringState
    {
        Seek = 0,
        Arrive = 1
    }

    public SteeringState state = SteeringState.Seek;
    public float slowingRadius = 5.0f;
    public float deceleration = 2.0f;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit mouseHit;
            if (Physics.Raycast(ray, out mouseHit))
            {
                GameObject hitObject = mouseHit.transform.gameObject;
                if (hitObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    target.position = mouseHit.point;
                    hasTarget = true;
                    state = SteeringState.Seek;
                }
            }
        }

        if(hasTarget)
        {
            Vector3 steeringForce;

            if (state == SteeringState.Seek)
                steeringForce = Seek();
            else
                steeringForce = Arrive();

            Vector3 acceleration = steeringForce / mass;
            currentVelocity += acceleration * Time.deltaTime;
            currentVelocity = Vector3.ClampMagnitude(currentVelocity, maxSpeed);
            transform.position += currentVelocity * Time.deltaTime;

            if (currentVelocity != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(currentVelocity);
            }
        }
 
    }

    Vector3 Seek()
    {
        Vector3 toTarget = target.position - transform.position;
        toTarget.y = 0;
        Vector3 desiredVelocity = toTarget.normalized * maxSpeed;
        state = SteeringState.Arrive;
        return (desiredVelocity - currentVelocity);
    }

    Vector3 Arrive()
    {
        Vector3 toTarget = target.position - transform.position;

        toTarget.y = 0;
        float distance = toTarget.magnitude;
        Vector3 desiredVelocity = Vector3.zero;

        if (distance < 0.01f)
        {
            currentVelocity = Vector3.zero;
            hasTarget = false;
        }
        else if (distance < slowingRadius)
        {
            desiredVelocity = toTarget.normalized * maxSpeed
*           (distance / (deceleration * slowingRadius));
        }
        else
        {
            desiredVelocity = toTarget.normalized * maxSpeed;
        }

        return (desiredVelocity - currentVelocity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);
    }
}
