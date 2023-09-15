using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumppad : MonoBehaviour
{
    [Range(20, 10000)]//Min Max force values
    public float bounceheight;
    public AudioSource audioSource;//Spring

    private void OnCollisionEnter(Collision collision) 
    {
        GameObject bouncer = collision.gameObject;//Defines
        Rigidbody rb = bouncer.GetComponent<Rigidbody>();//Gets Rigidbody
        rb.AddForce(Vector3.up * bounceheight, ForceMode.Impulse);//Adds force
        audioSource.Play();//Spring sound
    }
}
