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

    AudioSource audioSource;
    public AudioClip DoorClip;


    void Awake() 
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnInteract()
    {
        ToggleDoor();

        
    }

    void ToggleDoor() 
    {
        if (!isOpen) 
        {
            transform.Rotate(Vector3.up * OpenAngle);
            isOpen = true;
        }
        else
        {
            transform.rotation = Quaternion.identity;
            isOpen = false;
        }

        audioSource.PlayOneShot(DoorClip);
    }

    public void OnInteractStop()
    {
        
    }
}
