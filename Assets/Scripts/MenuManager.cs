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
    }

    public void HandlePlayHovered()
    {
        DeactivateCircles();
        PlayCircle.SetActive(true);
    }

    public void HandleOptionsClicked()
    {

    }

    public void HandleOptionsHovered()
    {
        DeactivateCircles();
        OptionCircle.SetActive(true);
    }
    
    public void HandleQuitClicked()
    {
        Application.Quit();
    }

    public void HandleQuitHovered()
    {
        DeactivateCircles();
        QuitCircle.SetActive(true);
    }
}