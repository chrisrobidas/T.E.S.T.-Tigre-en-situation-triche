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

    [Header("Timer")]
    [SerializeField] TMP_Text _timerText;
    [SerializeField] TMP_Text _resultText;

    public static GameManager Instance { get; private set; }

    private int _solvedQuestionsCount;

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
        _resultText.text = "" + (int)(((float)_solvedQuestionsCount / _questionsToSolve) * 100) + "%";

        _endGamePanel.SetActive(true);
        Time.timeScale = 0.0f;
    }

    private void ShowDefeatPanel()
    {
        _defeatPanel.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void HandleRestartClicked()
    {
        SceneManager.LoadScene(1);
    }

    public void HandleMainMenuClicked()
    {
        SceneManager.LoadScene(0);
    }
}
