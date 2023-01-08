using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMode : MonoBehaviour
{
    public static GameMode Instance { get; private set; }
    PlayerCharacter character;

    public Transform lastCheckpoint;
    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }


    public void OnPlayerCaugh() 
    {
        //restart scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
