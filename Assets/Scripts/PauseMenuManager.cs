using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    public Transform MainCameraTransform;
    public Transform BathroomCameraTransform;
    public GameObject PauseMenuCanvas;
    private bool isPaused;
    private Camera mainCam;

    public float LerpSpeed;

    private Color baseColor;
    void Start()
    {
        baseColor = PauseMenuCanvas.GetComponentInChildren<Image>().color;
        mainCam = Camera.main;
    }

    void Update()
    {
        if (!isPaused && Input.GetKeyDown(KeyCode.Escape))
        {
            StopAllCoroutines();
            StartCoroutine(GoToPause());
        }

        if (isPaused && Input.GetKeyDown(KeyCode.Escape))
        {
            StopAllCoroutines();
            StartCoroutine(GoToClass());
        }
    }

    IEnumerator GoToPause()
    {
        PauseMenuCanvas.GetComponentInChildren<Image>().color = new Color(baseColor.r,baseColor.g,baseColor.b,0);

        while (Vector3.Distance(mainCam.transform.position, BathroomCameraTransform.position) > 0.01f)
        {
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, BathroomCameraTransform.position, Time.deltaTime * LerpSpeed);
            mainCam.transform.rotation = Quaternion.Lerp(mainCam.transform.rotation, BathroomCameraTransform.rotation, Time.deltaTime * LerpSpeed);
            yield return null;
        }
        isPaused = true;

        PauseMenuCanvas.SetActive(true);
        float alpha = 0;

        while (PauseMenuCanvas.GetComponentInChildren<Image>().color.a < baseColor.a)
        {
            PauseMenuCanvas.GetComponentInChildren<Image>().color =
                new Color(baseColor.r, baseColor.g, baseColor.b, alpha += 0.03f);
            yield return null;
        }
    }

    IEnumerator GoToClass()
    {
        PauseMenuCanvas.SetActive(false);
        while (Vector3.Distance(mainCam.transform.position, MainCameraTransform.position) > 0.01f)
        {
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, MainCameraTransform.position, Time.deltaTime * LerpSpeed);
            mainCam.transform.rotation = Quaternion.Lerp(mainCam.transform.rotation, MainCameraTransform.rotation, Time.deltaTime * LerpSpeed);
            yield return null;
        }

        isPaused = false;
    }

    public void HandleReturnClicked()
    {
        StartCoroutine(GoToClass());
    }

    public void HandleAbandonClicked()
    {
        SceneManager.LoadScene(0);
    }
}
