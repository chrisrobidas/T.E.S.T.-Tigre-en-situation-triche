using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game settings")]
    [SerializeField] int _questionsToSolve;
    [SerializeField] float _gameTime = 300;

    [Header("Panels")]
    [SerializeField] GameObject _endGamePanel;
    [SerializeField] GameObject _defeatPanel;

    [Header("Grades Images")]
    [SerializeField] GameObject _goodImageObject;
    [SerializeField] GameObject _okayImageObject;
    [SerializeField] GameObject _badImageObject;

    [Header("Timer")]
    [SerializeField] TMP_Text _timerText;
    [SerializeField] TMP_Text _resultText;

    public static GameManager Instance { get; private set; }

    public int _solvedQuestionsCount;

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
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        _gameTime -= Time.deltaTime;
        int minutes = (int)(_gameTime / 60);
        int seconds = (int)(_gameTime % 60);
        _timerText.text = minutes + ":" + seconds.ToString("00");

        if (_gameTime <= 0.0f)
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

        _endGamePanel.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void ShowDefeatPanel()
    {
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
