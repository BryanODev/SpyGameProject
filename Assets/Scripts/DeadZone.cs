using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ouch!");
        PlayerCharacter player = other.GetComponent<PlayerCharacter>();

        if (player) 
        {
            if (GameMode.Instance) 
            {
                GameMode.Instance.OnPlayerCaugh();
            }
        }
    }
}
