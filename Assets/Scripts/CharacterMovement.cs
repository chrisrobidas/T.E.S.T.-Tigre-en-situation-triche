using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CharacterMovement : MonoBehaviour
{
    public GameObject camJoueur;
    public GameObject camCell;
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
                camCell.SetActive(false);
                camJoueur.SetActive(true);
            }
            else
            {
                camCell.SetActive(true);
                camJoueur.SetActive(false);
            }

        }
    }
}
