using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public ParticleSystem shatterParticles;
    public AudioSource shatterSound; // Reference to the AudioSource component in the Inspector

    private bool hasCollided = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasCollided)
        {
            // Enable the particle system
            shatterParticles.Play();

            // Play the glass shattering sound
            shatterSound.Play();

            // Disable the window renderer and collider to prevent further collisions
            GetComponent<Renderer>().enabled = false;
            GetComponent<Collider>().enabled = false;

            // Set the flag to true to prevent multiple collisions
            hasCollided = true;
        }
    }
}
