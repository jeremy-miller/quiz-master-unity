using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private float timeToCompleteQuestion = 30.0f;
    [SerializeField] private float timeToShowCorrectAnswer = 10.0f;

    public bool loadNextQuestion;
    public bool isAnsweringQuestion = false;
    public float fillFraction;
    
    private float _timerValue;
    
    void Update()
    {
        UpdateTimer();
    }

    void UpdateTimer()
    {
        _timerValue -= Time.deltaTime;

        if (isAnsweringQuestion)
        {
            if (_timerValue > 0)
            {
                fillFraction = _timerValue / timeToCompleteQuestion;
            }
            else
            {
                isAnsweringQuestion = false;
                _timerValue = timeToShowCorrectAnswer;
            }
        }
        else
        {
            if (_timerValue > 0)
            {
                fillFraction = _timerValue / timeToShowCorrectAnswer;
            }
            else
            {
                isAnsweringQuestion = true;
                _timerValue = timeToCompleteQuestion;
                loadNextQuestion = true;
            }
        }
    }

    public void CancelTimer()
    {
        _timerValue = 0;
    }
}
