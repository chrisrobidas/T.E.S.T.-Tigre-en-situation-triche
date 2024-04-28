using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterMovement : MonoBehaviour
{
    public GameObject camProf;
    public GameObject camCell;
    public GameObject camExamen;
    public GameObject camPause;

    public GameObject PauseMenuCanvas;
    public GameObject BathroomCanvas1;
    public GameObject BathroomCanvas2;
    public GameObject TimerCanvas;
    
    private bool isPaused;

    private Color baseColor;

    public CharacterState state;

    public enum CharacterState
    {
        OnExam,
        LookingUp,
        UsingCell
    }

    private void Start()
    {
        baseColor = PauseMenuCanvas.GetComponentInChildren<Image>().color;
    }

    private void Update()
    {
        if (isPaused && Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToClass();
            return;
        }

        if(isPaused)
            return;

        if (!isPaused && Input.GetKeyDown(KeyCode.Escape))
        {
            GoToPause();
            return;
        }

        if (Input.GetKey("space"))
        {
            //Le joueur triche, active la caméra vers le cell et désactive les 2 autres
            camCell.SetActive(true);
            camProf.SetActive(false);
            camExamen.SetActive(false);
            camPause.SetActive(false);
            state = CharacterState.UsingCell;

        }
        //Joueur arrête de tricher et retourne au default state (Regarde en avant)
        if (Input.GetKeyUp("space"))
        {
            camCell.SetActive(false);
            camProf.SetActive(true);
            camExamen.SetActive(false);
            camPause.SetActive(false);
            state = CharacterState.LookingUp;
        }
        //Le joueur veut avoir l'air moins suspect, regarde la feuille
        if (Input.GetKey(KeyCode.Z))
        {
            camProf.SetActive(false);
            camCell.SetActive(false);
            camExamen.SetActive(true);
            camPause.SetActive(false);
            state = CharacterState.OnExam;
        }
        //Le joueur regade en avant
        if (Input.GetKeyUp(KeyCode.Z))
        {
            camProf.SetActive(true);
            camCell.SetActive(false);
            camExamen.SetActive(false);
            camPause.SetActive(false);
            state = CharacterState.LookingUp;
        }
    }

    private void StopTime()
    {
        Time.timeScale = 0.0f;
    }
    
    private void GoToPause()
    {
        EventSystem.current.SetSelectedGameObject(null);
        isPaused = true;

        camCell.SetActive(false);
        camProf.SetActive(false);
        camExamen.SetActive(false);
        camPause.SetActive(true);
        CancelInvoke(nameof(StopTime));
        Invoke(nameof(StopTime), 1.5f);

        PauseMenuCanvas.GetComponentInChildren<Image>().color = new Color(baseColor.r, baseColor.g, baseColor.b, 0);

        PauseMenuCanvas.SetActive(true);
        BathroomCanvas1.SetActive(true);
        BathroomCanvas2.SetActive(true);
        TimerCanvas.SetActive(false);

        PauseMenuCanvas.GetComponentInChildren<Image>().color = baseColor;
    }
    
    public void ReturnToClass()
    {
        EventSystem.current.SetSelectedGameObject(null);
        CancelInvoke(nameof(StopTime));
        Time.timeScale = 1.0f;
        isPaused = false;

        camCell.SetActive(false);
        camProf.SetActive(true);
        camExamen.SetActive(false);
        camPause.SetActive(false);

        PauseMenuCanvas.SetActive(false);
        BathroomCanvas1.SetActive(false);
        BathroomCanvas2.SetActive(false);
        TimerCanvas.SetActive(true);
    }

    public void HandleAbandonClicked()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;
    }
}
