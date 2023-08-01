using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerAudio : MonoBehaviour
{
    public AudioSource speakerAudioSource; // Reference to the AudioSource on the speaker GameObject

    // Optional: You can assign an AudioClip for this specific button to play a unique sound.
    public AudioClip buttonTriggerSound;

    // OnTriggerEnter is called when an object enters the trigger collider of the button
    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering object is the player or anything you want to interact with the button
        // In this example, we're checking if the tag of the entering object is "Player".
        if (other.CompareTag("Player"))
        {
            // Play the sound on the speaker when the button is triggered
            if (speakerAudioSource != null)
            {
                // Play the unique buttonTriggerSound if assigned, otherwise, use the default sound from the speaker's AudioSource
                if (buttonTriggerSound != null)
                {
                    speakerAudioSource.PlayOneShot(buttonTriggerSound);
                }
                else
                {
                    speakerAudioSource.Play();
                }
            }

            // You can add your button's functionality here, like activating some event or animation.
        }
    }
}
