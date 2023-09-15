using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffiiceChairMovement : MonoBehaviour
{
    public float pushForce = 10f; // Adjust the pushing force
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();//Get component
    }

    private void OnCollisionEnter(Collision collision)//collison
    {
        if (collision.relativeVelocity.magnitude > pushForce)//greater than force
        {
            Vector3 pushDirection = collision.contacts[0].point - transform.position;//find direction
            pushDirection = -pushDirection.normalized;//normalize direction
            rb.AddForce(pushDirection * pushForce, ForceMode.Impulse);//apply force
        }
    }
}
