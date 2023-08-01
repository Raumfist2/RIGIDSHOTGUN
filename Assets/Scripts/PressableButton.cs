using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressableButton : MonoBehaviour
{
    public Vector3 pushedPosition = new Vector3(0, -0.1f, 0); // The position to move the button when pushed
    public Vector3 originalPosition; // The original position of the parent GameObject

    // Scale down the button by 1/10th for the pushed scale
    public Vector3 pushedScale = new Vector3(0.9f, 0.9f, 0.9f);

    public Vector3 originalScale; // The original scale of the parent GameObject

    private bool isPushed = false; // Flag to track if the button is currently pushed

    private void Start()
    {
        // Store the original position and scale of the parent GameObject
        originalPosition = transform.parent.localPosition;
        originalScale = transform.parent.localScale;
    }

    private void OnMouseDown()
    {
        // Check if the button is already pushed
        if (!isPushed)
        {
            // Move the parent GameObject down and scale it when clicked
            transform.parent.localPosition += pushedPosition;
            transform.parent.localScale = Vector3.Scale(transform.parent.localScale, pushedScale);
            isPushed = true;
        }
        else
        {
            // Reset the parent GameObject to its original position and scale
            transform.parent.localPosition = originalPosition;
            transform.parent.localScale = originalScale;
            isPushed = false;
        }
    }
}

