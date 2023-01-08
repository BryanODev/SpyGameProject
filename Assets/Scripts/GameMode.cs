using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameMode : MonoBehaviour
{
    public static GameMode Instance { get; private set; }
    PlayerCharacter character;

    public Transform lastCheckpoint;
    float learningProgress;

    public bool HasBlueprint;

    public GameObject Objectives;
    public GameObject FindBlueprintObjective;
    public GameObject EscapeObjective;
    public GameObject DeathScreen;

    public AudioClip blueprintPickUp;

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
        DeathScreen.SetActive(true);
        StartCoroutine(RestartGameLevel());
    }

    public IEnumerator RestartGameLevel() 
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddLearningProgress(float ammount) 
    {

    }

    public void OnBlueprintPickUp() 
    {
        HasBlueprint = true;
        FindBlueprintObjective.SetActive(false);
        EscapeObjective.SetActive(true);

        if (AudioManager.Instance) 
        {
            AudioManager.Instance.PlayOneShotClip(blueprintPickUp);
            AudioManager.Instance.PlayEscapeMusic();
        }
    }

    public void QuitToMenu() 
    {
        SceneManager.LoadScene(0);
    }

    public void WeEscaped() 
    {
        Objectives.SetActive(false);

        SceneManager.LoadScene(2);
    }
    
}
