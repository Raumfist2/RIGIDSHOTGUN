using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffiiceChairMovement : MonoBehaviour
{
    public float pushForce = 10f; // Adjust the pushing force
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > pushForce)
        {
            Vector3 pushDirection = collision.contacts[0].point - transform.position;
            pushDirection = -pushDirection.normalized;
            rb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
        }
    }
}
