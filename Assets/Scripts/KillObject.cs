using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillObject : MonoBehaviour
{
    public GameObject LoseCanvas;
    public GameObject MAINUI;

    void Start()
    {
        LoseCanvas.SetActive(false);
        MAINUI.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) //Check if the colliding object is the player
        {
            //Display the death screen.
            LoseCanvas.SetActive(true);
            //Hide UI.
            MAINUI.SetActive(false);

            Cursor.lockState = CursorLockMode.None;//Stops Cursor from moving
            Cursor.visible = true;//Hides cursor
        }
    }
}
