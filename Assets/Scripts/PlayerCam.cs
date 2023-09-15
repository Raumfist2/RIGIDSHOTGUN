using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;
    public Transform camHolder;

    float xRotation;
    float yRotation;

    public GameObject WinCanvas;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;//Stops Cursor from moving
        Cursor.visible = false;//Hides cursor
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if(WinCanvas.activeSelf == true)//If win condition is met
        {
        }
        else{
        //Get Mouse Input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;
        //Apply Mouse Input
        yRotation += mouseX;
        xRotation -= mouseY;
        //Clamp Input to 90 degrees
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        //Rotate Cam and Orientation
        camHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }

    public void DoFov(float endValue)
    {
        //Changing fov with .25 transition time
        GetComponent<Camera>().DOFieldOfView(endValue, 0.25f);
    }

    public void DoTilt(float zTilt)
    {
        //Changing rotation
        transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.25f);
    }
}
