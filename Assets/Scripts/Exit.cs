using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        PlayerCharacter player = other.GetComponent<PlayerCharacter>();

        if (player) 
        {
            if (GameMode.Instance) 
            {
                if (GameMode.Instance.HasBlueprint) 
                {
                    GameMode.Instance.WeEscaped();
                }
            }
        }
    }
}
