using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] int _questionsToSolve;

    public static GameManager Instance { get; private set; }

    private int _solvedQuestionsCount;

    public void IncreaseSolvedQuestionsCount()
    {
        _solvedQuestionsCount++;
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
}
