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
            StartCoroutine(GoToPause());
        }
        else if (isPaused && Input.GetKeyDown(KeyCode.Escape))
        {
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
        if (Input.GetKeyDown(KeyCode.Z))
        {
            camProf.SetActive(true);
            camCell.SetActive(false);
            camExamen.SetActive(false);
            camPause.SetActive(false);
            state = CharacterState.OnExam;
        }
        //Le joueur regade en avant
        if (Input.GetKeyDown(KeyCode.X))
        {
            camProf.SetActive(false);
            camCell.SetActive(false);
            camExamen.SetActive(true);
            camPause.SetActive(false);
            state = CharacterState.LookingUp;
        }
    }

    private IEnumerator GoToPause()
    {
        PauseMenuCanvas.GetComponentInChildren<Image>().color = new Color(baseColor.r, baseColor.g, baseColor.b, 0);
        isPaused = true;

        PauseMenuCanvas.SetActive(true);

        while (PauseMenuCanvas.GetComponentInChildren<Image>().color.a < baseColor.a)
        {
            PauseMenuCanvas.GetComponentInChildren<Image>().color = Color.Lerp(baseColor, new Color(baseColor.r, baseColor.g, baseColor.b, baseColor.a), Time.deltaTime * 10);
            yield return null;
        }
    }

    private void GoToClass()
    {
        PauseMenuCanvas.SetActive(false);
        isPaused = false;
    }

    public void ReturnToClass()
    {
        camCell.SetActive(false);
        camProf.SetActive(true);
        camExamen.SetActive(false);
        camPause.SetActive(false);
        GoToClass();
    }

    public void HandleAbandonClicked()
    {
        SceneManager.LoadScene(0);
    }
}
