using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Component")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI finalTimerText;
    public GameObject WinCanvas;
    public GameObject MAINUI;

    [Header("Timer Settings")]
    public float currentTime;
    public float lastTime;

    void Start()
    {
        //Sets default
        WinCanvas.SetActive(false);
        MAINUI.SetActive(true);
    }

    void Update()
    {
            currentTime = currentTime += Time.deltaTime;//Updates per Time.deltatime
            SetTimerText();
    }

    private void SetTimerText()
    {
        timerText.text = currentTime.ToString("000.000");//Sets to text
    }
}

        

