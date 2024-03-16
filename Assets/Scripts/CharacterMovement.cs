using System.Collections;
using UnityEngine;
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
        if (!isPaused && Input.GetKeyDown(KeyCode.Escape))
        {
            camCell.SetActive(false);
            camProf.SetActive(false);
            camExamen.SetActive(false);
            camPause.SetActive(true);
            Invoke(nameof(StopTime), 1.5f);
            StartCoroutine(GoToPause());
        }
        else if (isPaused && Input.GetKeyDown(KeyCode.Escape))
        {
            CancelInvoke(nameof(StopTime));
            ReturnToClass();
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
    
    private IEnumerator GoToPause()
    {
        PauseMenuCanvas.GetComponentInChildren<Image>().color = new Color(baseColor.r, baseColor.g, baseColor.b, 0);
        isPaused = true;

        PauseMenuCanvas.SetActive(true);
        BathroomCanvas1.SetActive(true);
        BathroomCanvas2.SetActive(true);
        TimerCanvas.SetActive(false);

        while (PauseMenuCanvas.GetComponentInChildren<Image>().color.a < baseColor.a)
        {
            PauseMenuCanvas.GetComponentInChildren<Image>().color = Color.Lerp(baseColor, new Color(baseColor.r, baseColor.g, baseColor.b, baseColor.a), Time.deltaTime * 10);
            yield return null;
        }
    }
    
    public void ReturnToClass()
    {
        Time.timeScale = 1.0f;
        
        camCell.SetActive(false);
        camProf.SetActive(true);
        camExamen.SetActive(false);
        camPause.SetActive(false);

        PauseMenuCanvas.SetActive(false);
        BathroomCanvas1.SetActive(false);
        BathroomCanvas2.SetActive(false);
        TimerCanvas.SetActive(true);
        
        isPaused = false;
    }

    public void HandleAbandonClicked()
    {
        SceneManager.LoadScene(0);
    }
}
