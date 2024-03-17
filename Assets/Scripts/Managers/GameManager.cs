using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    [Header("Game settings")]
    [SerializeField] int _questionsToSolve;
    [SerializeField] float _gameTime = 300;
    private float currentTime;
    
    [Header("Panels")]
    [SerializeField] private GameObject _endGamePanel;
    [SerializeField] private GameObject _defeatPanel;
    [SerializeField] private GameObject _tigerPanel;

    [Header("Grades Images")]
    [SerializeField] private GameObject _goodImageObject;
    [SerializeField] private GameObject _okayImageObject;
    [SerializeField] private GameObject _badImageObject;

    [Header("Timer")]
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private TMP_Text _resultText;

    public static GameManager Instance { get; private set; }

    public int _solvedQuestionsCount;

    private AudioSource _audioSource;

    public void IncreaseSolvedQuestionsCount()
    {
        _solvedQuestionsCount++;

        if (_solvedQuestionsCount == _questionsToSolve)
        {
            ShowEndGamePanel();
        }
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        currentTime = _gameTime;
    }

    private void Update()
    {
        currentTime -= Time.deltaTime;
        int minutes = (int)(currentTime / 60);
        int seconds = (int)(currentTime % 60);
        _timerText.text = minutes + ":" + seconds.ToString("00");

        if (currentTime <= 0.0f)
        {
            ShowEndGamePanel();
        }
    }

    private void ShowEndGamePanel()
    {
        int result = (int)(((float)_solvedQuestionsCount / _questionsToSolve) * 100);
        _resultText.text = result + "%";

        if (result >= 70)
        {
            _goodImageObject.SetActive(true);
        }
        else if (result >= 40)
        {
            _okayImageObject.SetActive(true);
        }
        else
        {
            _badImageObject.SetActive(true);
        }
        SaveScore();

        _endGamePanel.SetActive(true);
        Time.timeScale = 0.0f;
    }
    
    public void SaveScore()
    {
        float time = _gameTime - currentTime;
        int score = (int)(((float)_solvedQuestionsCount / _questionsToSolve) * 100);

        int bestScore = PlayerPrefs.GetInt("score");
        float bestTime = PlayerPrefs.GetFloat("time");
        //int minutes = (int)(currentTime / 60);
        //int seconds = (int)(currentTime % 60);
        //_timerText.text = minutes + ":" + seconds.ToString("00");

        if (score > bestScore)
        {
            PlayerPrefs.SetInt("score", score);
            PlayerPrefs.SetFloat("time", time);
        } else if (score == bestScore && time < bestTime)
        {
            PlayerPrefs.SetInt("score", score);
            PlayerPrefs.SetFloat("time", time);
        }
    }

    public void LoadScore()
    {
        
    }
    
    public IEnumerator ShowDefeatPanel()
    {
        _tigerPanel.SetActive(true);
        _audioSource.Play();

        yield return new WaitForSeconds(3);

        _tigerPanel.SetActive(false);
        _defeatPanel.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void HandleRestartClicked()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1.0f;
    }

    public void HandleMainMenuClicked()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;
    }
}
