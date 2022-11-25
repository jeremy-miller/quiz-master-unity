using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    private int _correctAnswers = 0;
    private int _questionsSeen = 0;

    public int GetCorrectAnswers()
    {
        return _correctAnswers;
    }

    public void IncrementCorrectAnswers()
    {
        _correctAnswers++;
    }

    public int GetQuestionsSeen()
    {
        return _questionsSeen;
    }

    public void IncrementQuestionsSeen()
    {
        _questionsSeen++;
    }

    public int CalculateScore()
    {
        // cast to float to force floating point division
        return Mathf.RoundToInt(_correctAnswers / (float)_questionsSeen * 100);
    }
}
