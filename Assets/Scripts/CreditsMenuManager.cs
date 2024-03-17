using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CreditsMenuManager : MonoBehaviour
{
    public GameObject BackCircle;


    // Start is called before the first frame update
    void Start()
    {
        DeactivateCircles();
    }

    public void DeactivateCircles(){
        BackCircle.SetActive(false);
    }

    public void HandleReturnClicked()
    {
        SceneManager.LoadScene(0);
    }

    public void HandleReturnHovered()
    {
        DeactivateCircles();
        BackCircle.SetActive(true);
    }
}
