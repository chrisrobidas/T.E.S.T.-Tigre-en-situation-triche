using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CharacterMovement : MonoBehaviour
{
    public GameObject camJoueur;
    public GameObject camCell;

    public GameObject pauseMenu;

    public bool isCheating;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            if (camCell.activeSelf)
            {
                //Désactive la caméra du cell pour activer celle du joueur
                camCell.SetActive(false);
                camJoueur.SetActive(true);
                isCheating = false;
            }
            else
            {
                //Désactive cam joueur pour activer celle du cell
                camCell.SetActive(true);
                camJoueur.SetActive(false);
                isCheating = true;
            }

        }
        if (isCheating)
        {
            //Code si joueur triche
        }
        else
        {
            //Code si joueur ne triche pas
        }
        if (Input.GetKeyDown("escape"))
        {
            Pause();
        }
    }
    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

}
