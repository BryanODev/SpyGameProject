using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprint : MonoBehaviour, IInteractable
{
    bool CanInteract = true;
    public void OnInteract()
    {
        if (CanInteract)
        {
            StartLearning();
        }
    }

    public void OnInteractStop()
    {
        StopLearning();
    }

    void StartLearning() 
    {
        //We start adding learning to gamemode
    }

    IEnumerator IncreaseLearning() 
    {
        while (true) 
        {
            if (GameMode.Instance) 
            {
                GameMode.Instance.AddLearningProgress(3);
            }

            yield return new WaitForSeconds(1);
        }

        yield return null;
    }

    void StopLearning() 
    {
        //We stop adding learning to gamemode
    }
}
