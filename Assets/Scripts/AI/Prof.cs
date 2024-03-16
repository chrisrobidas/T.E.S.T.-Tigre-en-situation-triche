using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Prof : MonoBehaviour
{
    // Start is called before the first frame update
    enum AlertLevel
    {
        NonAlert,
        Suspicious,
        Alert,
        Catching
    }

    bool isGameOver = false;

    AlertLevel alertState = AlertLevel.NonAlert;
    float startNonAlertDuration = 1;
    float[,] minMaxStateDurations = { { 2, 2, 2, 2 }, { 5, 5, 5, 5 } };
    float currentStateTimeLeft = 0;
    float oneSecond = 1.0f;
    int lastTimeLeft = 0;

    double[] transitionBaseProbabilities = { 60f, 30f, 10f, 0f };
    double[] transitionProbabilities;

    double[,] alertRemovalNoCell = { { -0.25, -0.25, -1 }, { -0.5, -0.5, -1 }, { -1, -1, -1 } };
    double[] alertRemovalUsingCell = { -0.5, -2, -3 };
    double[,] alertSeparationUsingCell = { { 0.25, 0.25, 0.0 }, { 1.0, 0.5, 0.5 }, { 1.0, 1.0, 1.0 } };



    void StateTransition()
    {
        double random = UnityEngine.Random.Range(0f, 100f);
        double ogValue = random;
        int i;
        for (i = 0; i < transitionProbabilities.Length; i++)
        {
            random -= transitionProbabilities[i];
            if (random <= 0.0)
            {
                break;
            }
        }
        alertState = (AlertLevel)i;
        Debug.Log(alertState);
        string probs = transitionProbabilities[0].ToString() + ' ' + transitionProbabilities[1].ToString() + ' ' + transitionProbabilities[2].ToString() + ' ' + transitionProbabilities[3].ToString() + ' ' + transitionProbabilities.Sum();
        Debug.Log(probs);
    }

    void UpdateProbabilities(bool isUsingCell)
    {
        if (alertState >= AlertLevel.Catching)
        {
            return;
        }
        if (isUsingCell)
        {
            double[] previousTransitionProbabilities = (double[]) transitionProbabilities.Clone();
            transitionProbabilities[0] = Math.Max(0, alertRemovalUsingCell[(int)alertState] + transitionProbabilities[0]);
            double removedProb = previousTransitionProbabilities[0] - transitionProbabilities[0];
            double amountToAdd = Math.Min(removedProb, alertSeparationUsingCell[(int)alertState, 2]);
            //double remainingProb = previousTransitionProbabilities[0] - transitionProbabilities[0];
            transitionProbabilities[3] = Math.Min(100, amountToAdd + transitionProbabilities[3]);
            removedProb -= transitionProbabilities[3] - previousTransitionProbabilities[3];
            amountToAdd = Math.Min(removedProb, alertSeparationUsingCell[(int)alertState, 1]);
            transitionProbabilities[2] = Math.Min(100 - transitionProbabilities[3], amountToAdd + transitionProbabilities[2]);
            removedProb -= transitionProbabilities[2] - previousTransitionProbabilities[2];
            amountToAdd = Math.Min(removedProb, alertSeparationUsingCell[(int)alertState, 0]);
            transitionProbabilities[1] = Math.Min(100 - transitionProbabilities[3] - transitionProbabilities[2], amountToAdd + transitionProbabilities[1]);
        }
        else
        {
            double[] previousTransitionProbabilities = (double[]) transitionProbabilities.Clone();
            transitionProbabilities[1] = Math.Max(transitionBaseProbabilities[1], alertRemovalNoCell[(int)alertState, 0] + transitionProbabilities[1]);
            transitionProbabilities[2] = Math.Max(transitionBaseProbabilities[2], alertRemovalNoCell[(int)alertState, 1] + transitionProbabilities[2]);
            transitionProbabilities[3] = Math.Max(transitionBaseProbabilities[3], alertRemovalNoCell[(int)alertState, 2] + transitionProbabilities[3]);
            double removedAmount = (previousTransitionProbabilities[1] - transitionProbabilities[1]) + (previousTransitionProbabilities[2] - transitionProbabilities[2]) + (previousTransitionProbabilities[3] - transitionProbabilities[3]);
            transitionProbabilities[0] = Math.Min(transitionBaseProbabilities[0], Math.Max(0.0, removedAmount) + transitionProbabilities[0]);
        }
    }

    bool IsCaught(bool isUsingCell)
    {
        return UnityEngine.Random.Range(0, 4) > (isUsingCell ? 1 : 0);
    }

    void Start()
    {
        transitionProbabilities = (double[]) transitionBaseProbabilities.Clone();
        alertState = AlertLevel.NonAlert;

        currentStateTimeLeft = startNonAlertDuration;
    }

    // Update is called once per frame
    void Update()
    {
        bool isGameStarted = true;
        if (!isGameStarted && !isGameOver)
        {
            return;
        }
        currentStateTimeLeft -= Time.deltaTime;
        bool isUsingCell = Input.GetKey(KeyCode.Space);
        Debug.Log(isUsingCell);
        oneSecond -= Time.deltaTime;
        if (oneSecond < 0.0f)
        {
            UpdateProbabilities(isUsingCell);
            oneSecond = 1.0f;
        }

        if (alertState == AlertLevel.Catching)
        {

            if (IsCaught(isUsingCell) && lastTimeLeft >= (int)currentStateTimeLeft)
            {
                // Set caught to end game
                Debug.Log("Caught hahahaha");
                isGameOver = true;
                lastTimeLeft = (int)currentStateTimeLeft;
            }
        }
        if (currentStateTimeLeft < 0)
        {
            StateTransition();
            currentStateTimeLeft += UnityEngine.Random.Range(minMaxStateDurations[0, (int)alertState], minMaxStateDurations[1, (int)alertState]);
            if (alertState == AlertLevel.Catching)
            {
                lastTimeLeft = 5;
            }
        }

    }
}
