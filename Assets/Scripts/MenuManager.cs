using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuManager : MonoBehaviour
{
    public GameObject PlayCircle;
    public GameObject OptionCircle;
    public GameObject QuitCircle;

    public GameObject SettingsCanvas;


    // Start is called before the first frame update
    void Start()
    {
        DeactivateCircles();
    }

    public void DeactivateCircles(){
        PlayCircle.SetActive(false);
        OptionCircle.SetActive(false);
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
