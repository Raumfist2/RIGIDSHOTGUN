using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinObject : MonoBehaviour
{
    public GameObject WinCanvas;
    public GameObject MAINUI;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI finalTimerText;

    void Start()
    {
        WinCanvas.SetActive(false);
        MAINUI.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) //Check if the colliding object is the player
        {
            //Display the death screen.
            WinCanvas.SetActive(true);
            //Hide UI.
            MAINUI.SetActive(false);

            finalTimerText.text = timerText.text;//Show Final time

            Cursor.lockState = CursorLockMode.None;//Stops Cursor from moving
            Cursor.visible = true;//Hides cursor
        }
    }
}
