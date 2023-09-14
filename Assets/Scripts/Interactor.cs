using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    interface Interactable
    {
        public void Interact();
    }

    public class IntertheAct : MonoBehaviour
    {
        public Transform InteracterSource;
        public float InteractRange;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Ray r = new Ray(InteracterSource.position, InteracterSource.forward);
                if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange)) { 
                if (hitInfo.collider.gameObject.TryGetComponent(out Interactable interactObj))
                    {
                        interactObj.Interact();
                    }
                }
            }
        }
    }
}
