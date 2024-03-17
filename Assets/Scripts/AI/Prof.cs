using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Prof : MonoBehaviour
{
    // Start is called before the first frame update
    public enum AlertLevel
    {
        NonAlert,
        Suspicious,
        Alert,
        Catching
    }

    public bool isGameOver = false;
    public bool canCatch = false;
    public CharacterMovement charMovement = null;

    public SpriteRenderer spriteRenderer = null;

    public AlertLevel alertState = AlertLevel.NonAlert;
    public float startNonAlertDuration = 1;
    public List<Vector4> minMaxStateDurations = new List<Vector4> { new Vector4(2f, 2f, 2f, 2f), new Vector4(5f, 5f, 5f, 5f) };
    public float currentStateTimeLeft = 0;
    float oneSecond = 1.0f;
    int lastTimeLeft = 0;

    public List<Sprite> tigerAlertSprites = new List<Sprite>(5);
    public List<Vector3> tigerAlertPositions = new List<Vector3>(4);

    public double[] transitionBaseProbabilities = { 60f, 30f, 10f, 0f };
    public double[] transitionProbabilities;

    public double[] nonAlertProbUpdateOnExam = { 0.5, 0.5, 2.0 };
    public double[] suspiciousProbUpdateOnExam = { 0.5, 0.5, 1.0 };
    public double[] alertProbUpdateOnExam = { 1.0, 1.0, 1.0 };

    public double[] nonAlertProbUpdateNoCell = { 0.25, 0.25, 1.0 };
    public double[] suspiciousProbUpdateNoCell = { 0.5, 0.5, 1.0 };
    public double[] alertProbUpdateNoCell = { 0.5, 0.5, 0.5 };

    public double[] nonAlertProbUpdateUsingCell = { 0.5, 0.5, 0.0 };
    public double[] suspiciousProbUpdateUsingCell = { 2.0, 1.0, 1.0 };
    public double[] alertProbUpdateUsingCell = { 2.0, 2.0, 2.0 };

    public int caughtProbability = 5;

    private int logCount = 0;
    public AudioSource src;


    void StateTransition()
    {
        double random = UnityEngine.Random.Range(0f, 100f);
        int i;
        for (i = 0; i < transitionProbabilities.Length; i++)
        {
            random -= transitionProbabilities[i];
            if (random <= 0.0)
            {
                break;
            }
        }
        if (i == (int)AlertLevel.Catching && alertState < AlertLevel.Alert)
        {
            i = (int)AlertLevel.Alert;
        }
        if (alertState != (AlertLevel)i)
        {
            src.PlayOneShot(src.clip);

        }
        alertState = (AlertLevel)i;
        gameObject.transform.position = tigerAlertPositions[i];
        spriteRenderer.sprite = tigerAlertSprites[i];
        string probs = logCount.ToString() + ' ' + transitionProbabilities[0].ToString() + ' ' + transitionProbabilities[1].ToString() + ' ' + transitionProbabilities[2].ToString() + ' ' + transitionProbabilities[3].ToString() + ' ' + transitionProbabilities.Sum();
        logCount++;
    }

    double GetFromOtherProbabilities(AlertLevel alertLevel, double wantedAmount)
    {
        double[] previousTransitionProbabilities = (double[])transitionProbabilities.Clone();
        double receivedAmount = 0.0;
        for (int i = 0; i < (int)alertLevel && receivedAmount < wantedAmount; i++)
        {
            transitionProbabilities[i] = Math.Max(0.0, transitionProbabilities[i] - (wantedAmount - receivedAmount));
            receivedAmount += previousTransitionProbabilities[i] - transitionProbabilities[i];
        }
        return receivedAmount;
    }

    double ReduceFromOtherProbabilities(AlertLevel alertLevel, double wantedAmount)
    {
        if (alertLevel == AlertLevel.Catching)
        {
            return 0.0;
        }
        double[] previousTransitionProbabilities = (double[])transitionProbabilities.Clone();
        double receivedAmount = 0.0;
        for (int i = (int)alertLevel + 1; i < transitionProbabilities.Length && receivedAmount < wantedAmount; i++)
        {
            transitionProbabilities[i] = Math.Max(transitionBaseProbabilities[i], transitionProbabilities[i] - (wantedAmount - receivedAmount));
            receivedAmount += previousTransitionProbabilities[i] - transitionProbabilities[i];
        }
        return receivedAmount;
    }

    void UpdateProbabilities(CharacterMovement.CharacterState charState)
    {
        if (alertState >= AlertLevel.Catching)
        {
            return;
        }
        double[] probsChanges = { 0.0, 0.0, 0.0 };

        switch (charState)
        {
            case CharacterMovement.CharacterState.OnExam:
                switch (alertState)
                {
                    case AlertLevel.NonAlert:
                        probsChanges = nonAlertProbUpdateOnExam;
                        break;
                    case AlertLevel.Suspicious:
                        probsChanges = suspiciousProbUpdateOnExam;
                        break;
                    case AlertLevel.Alert:
                        probsChanges = alertProbUpdateOnExam;
                        break;
                    default:
                        break;
                }
                transitionProbabilities[2] += ReduceFromOtherProbabilities(AlertLevel.Alert, probsChanges[2]);
                transitionProbabilities[1] += ReduceFromOtherProbabilities(AlertLevel.Suspicious, probsChanges[1]);
                transitionProbabilities[0] += ReduceFromOtherProbabilities(AlertLevel.NonAlert, probsChanges[0]);
                break;
            case CharacterMovement.CharacterState.LookingUp:
                switch (alertState)
                {
                    case AlertLevel.NonAlert:
                        probsChanges = nonAlertProbUpdateNoCell;
                        break;
                    case AlertLevel.Suspicious:
                        probsChanges = suspiciousProbUpdateNoCell;
                        break;
                    case AlertLevel.Alert:
                        probsChanges = alertProbUpdateNoCell;
                        break;
                    default:
                        break;
                }
                transitionProbabilities[2] += ReduceFromOtherProbabilities(AlertLevel.Alert, probsChanges[2]);
                transitionProbabilities[1] += ReduceFromOtherProbabilities(AlertLevel.Suspicious, probsChanges[1]);
                transitionProbabilities[0] += ReduceFromOtherProbabilities(AlertLevel.NonAlert, probsChanges[0]);
                break;
            case CharacterMovement.CharacterState.UsingCell:
                switch (alertState)
                {
                    case AlertLevel.NonAlert:
                        probsChanges = nonAlertProbUpdateUsingCell;
                        break;
                    case AlertLevel.Suspicious:
                        probsChanges = suspiciousProbUpdateUsingCell;
                        break;
                    case AlertLevel.Alert:
                        probsChanges = alertProbUpdateUsingCell;
                        break;
                    default:
                        break;
                }
                transitionProbabilities[3] += GetFromOtherProbabilities(AlertLevel.Catching, probsChanges[2]);
                transitionProbabilities[2] += GetFromOtherProbabilities(AlertLevel.Alert, probsChanges[1]);
                transitionProbabilities[1] += GetFromOtherProbabilities(AlertLevel.Suspicious, probsChanges[0]);
                break;
            default:
                break;
        }
    }

    bool IsCaught(CharacterMovement.CharacterState state)
    {
        return UnityEngine.Random.Range(0, caughtProbability) < (int)state;
    }

    void Start()
    {
        charMovement = GameObject.Find("Cell").GetComponent<CharacterMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        transitionProbabilities = (double[])transitionBaseProbabilities.Clone();
        alertState = AlertLevel.NonAlert;
        transform.position = tigerAlertPositions[0];
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        spriteRenderer.sprite = tigerAlertSprites[0];

        currentStateTimeLeft = startNonAlertDuration;
    }

    // Update is called once per frame
    void Update()
    {
        bool isGameStarted = true;
        if (!isGameStarted || isGameOver)
        {
            return;
        }
        currentStateTimeLeft -= Time.deltaTime;
        // Change to event with subscription to event at some point instead of checking every frame.
        CharacterMovement.CharacterState charState = charMovement.state;
        oneSecond -= Time.deltaTime;
        if (oneSecond < 0.0f)
        {
            UpdateProbabilities(charState);
            oneSecond = 1.0f;
        }

        if (alertState == AlertLevel.Catching)
        {
            if (canCatch && ((float) lastTimeLeft) >= currentStateTimeLeft && IsCaught(charState))
            {
                // Set caught to end game
                Debug.Log("Caught hahahaha");
                isGameOver = true;
                StartCoroutine(GameManager.Instance.ShowDefeatPanel());
            }
            lastTimeLeft = (int)currentStateTimeLeft;
        }
        if (currentStateTimeLeft < 0)
        {
            StateTransition();
            currentStateTimeLeft += UnityEngine.Random.Range(minMaxStateDurations[0][(int)alertState], minMaxStateDurations[1][(int)alertState]);
            if (alertState == AlertLevel.Catching)
            {
                lastTimeLeft = 5;
            }
        }

    }
}
