using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CharacterMovement : MonoBehaviour
{
    public GameObject camProf;
    public GameObject camCell;
    public GameObject camExamen;

    public GameObject pauseMenu;
    public int state;

    public enum CharacterState
    {
        OnExam,
        LookingUp,
        UsingCell
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKey("space"))
        {

            //Le joueur triche, active la caméra vers le cell et désactive les 2 autres
            camCell.SetActive(true);
            camProf.SetActive(false);
            camExamen.SetActive(false);
            state = (int)CharacterState.UsingCell;

        }
        //Joueur arrête de tricher et retourne au default state (Regarde en avant)
        if (Input.GetKeyUp("space"))
        {

            camCell.SetActive(false);
            camProf.SetActive(true);
            camExamen.SetActive(false);
            state = (int)CharacterState.LookingUp;
        }
        //Le joueur veut avoir l'air moins suspect, regarde la feuille
        if (Input.GetKeyDown(KeyCode.Z))
        {
            camProf.SetActive(true);
            camCell.SetActive(false);
            camExamen.SetActive(false);
            state = (int)CharacterState.OnExam;

        }
        //Le joueur regade en avant
        if (Input.GetKeyDown(KeyCode.X))
        {
            camProf.SetActive(false);
            camCell.SetActive(false);
            camExamen.SetActive(true);
            state = (int)CharacterState.LookingUp;

        }

    }


}
