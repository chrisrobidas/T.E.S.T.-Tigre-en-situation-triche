using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuManager : MonoBehaviour
{
    public GameObject PlayCircle;
    public GameObject OptionCircle;
    public GameObject CreditsCircle;
    public GameObject QuitCircle;
    public TMP_Text BestScore;

    public GameObject SettingsCanvas;


    // Start is called before the first frame update
    void Start()
    {
        DeactivateCircles();
        
        int bestScore = PlayerPrefs.GetInt("score");
        float bestTime = PlayerPrefs.GetFloat("time");
        
        int minutes = (int)(bestTime / 60);
        int seconds = (int)(bestTime % 60);
        
        if (bestScore > 0)
        {
            BestScore.text = $"Meilleur score : {bestScore}% en {minutes + ":" + seconds.ToString("00")}";
        }
    }

    public void DeactivateCircles(){
        PlayCircle.SetActive(false);
        OptionCircle.SetActive(false);
        CreditsCircle.SetActive(false);
        QuitCircle.SetActive(false);
    }

    public void HandlePlayClicked()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1.0f;
    }

    public void HandlePlayHovered()
    {
        DeactivateCircles();
        PlayCircle.SetActive(true);
    }

    public void HandleOptionsHovered()
    {
        DeactivateCircles();
        OptionCircle.SetActive(true);
    }

    public void HandleOptionsClicked()
    {
        SettingsCanvas.SetActive(true);
    }

    public void HandleBackClicked()
    {
        SettingsCanvas.SetActive(false);
    }
    public void HandleCreditsClicked()
    {
        SceneManager.LoadScene(2);
    }

    public void HandleCreditsHovered()
    {
        DeactivateCircles();
        CreditsCircle.SetActive(true);
    }

    public void HandleQuitClicked()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void HandleQuitHovered()
    {
        DeactivateCircles();
        QuitCircle.SetActive(true);
    }
}
