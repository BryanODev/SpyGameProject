using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable 
{
    void OnInteract();
    void OnInteractStop();
}

public class Door : MonoBehaviour, IInteractable
{
    bool isOpen;

    public float OpenAngle;
    public float CloseAngle;
    public void OnInteract()
    {
        ToggleDoor();
    }

    void ToggleDoor() 
    {
        if (!isOpen) 
        {
            transform.Rotate(Vector3.up * OpenAngle);
        }

        if (isOpen)
        {
            transform.Rotate(Vector3.up * CloseAngle);
        }
    }

    public void OnInteractStop()
    {
        
    }
}
