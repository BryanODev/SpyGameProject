using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject menuContainer;
    public GameObject gameIntroduction;
    public GameObject credits;

    public Image blackScreen;

    private void Start()
    {
        StartCoroutine(FadeOut());
    }

    public void StartGame()
    {
        //SceneManager.LoadScene(1);
        menuContainer.SetActive(false);
        gameIntroduction.SetActive(true);
    }

    public void BackToMenu() 
    {
        menuContainer.SetActive(true);
        gameIntroduction.SetActive(false);
        credits.SetActive(false);
    }

    public void Credits() 
    {
        menuContainer.SetActive(false);
        gameIntroduction.SetActive(false);
        credits.SetActive(true);
    }

    public void LoadGameLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void FadeInOut()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float fade = 1;
        Color currentColor = blackScreen.color;

        while (fade > 0)
        {
            fade -= Time.deltaTime;
            currentColor.a = fade;
            blackScreen.color = currentColor;

            yield return new WaitForEndOfFrame();
        }

        if(fade <= 0) 
        {
            blackScreen.gameObject.SetActive(false);
        }
    }

    public void FadeInStart() 
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn() 
    {
        float fade = 1;
        Color currentColor = blackScreen.color;

        if (currentColor.a <= 0 && !blackScreen.gameObject.activeSelf)
        {
            blackScreen.gameObject.SetActive(true);
        }

        while (fade > 0)
        {
            fade += Time.deltaTime;
            currentColor.a = fade;
            blackScreen.color = currentColor;

            yield return new WaitForEndOfFrame();
        }

    }

    IEnumerator FadeInToIntroduction()
    {
        float fade = 1;
        Color currentColor = blackScreen.color;

        while (fade > 0)
        {
            fade -= Time.deltaTime;
            currentColor.a = fade;
            blackScreen.color = currentColor;

            yield return new WaitForEndOfFrame();
        }

        gameIntroduction.SetActive(true);
    }

    public void QuitGame() 
    {
        Application.Quit();
    }

}
