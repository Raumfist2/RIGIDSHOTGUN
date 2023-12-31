using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject ControlsCanvas;
    public GameObject MENUCANVAS;

    void Start()
    {
        //default
        ControlsCanvas.SetActive(false);
        MENUCANVAS.SetActive(true);
    }

    public void ControlsButton()
    {
        //opens help ui
        ControlsCanvas.SetActive(true);
        MENUCANVAS.SetActive(false);
    }

    public void BackButton()
    {
        //hides help 
        ControlsCanvas.SetActive(false);
        MENUCANVAS.SetActive(true);
    }
}
