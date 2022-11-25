using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private List<QuestionSO> questions = new List<QuestionSO>();

    [Header("Answers")]
    [SerializeField] private GameObject[] answerButtons;
    
    [Header("Button Colors")]
    [SerializeField] private Sprite defaultAnswerSprite;
    [SerializeField] private Sprite correctAnswerSprite;
    
    [Header("Timer")]
    [SerializeField] private Image timerImage;

    [Header("Scoring")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Progress Bar")]
    [SerializeField] private Slider progressBar;

    public bool isComplete = false;
    
    private ScoreKeeper _scoreKeeper;
    private Timer _timer;
    private int _correctAnswerIndex;
    private bool _hasAnsweredEarly = true;
    private QuestionSO _currentQuestion;

    private void Awake()
    {
        _timer = FindObjectOfType<Timer>();
        _scoreKeeper = FindObjectOfType<ScoreKeeper>();
    }

    void Start()
    {
        progressBar.maxValue = questions.Count;
        progressBar.value = 0;
    }

    void GetNextQuestion()
    {
        if (questions.Count > 0)
        {
            SetButtonState(true);
            SetDefaultButtonSprites();
            GetRandomQuestion();
            DisplayQuestion();
            progressBar.value++;
            _scoreKeeper.IncrementQuestionsSeen();
        }
    }

    void SetDefaultButtonSprites()
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            Image buttonImage = answerButtons[i].GetComponent<Image>();
            buttonImage.sprite = defaultAnswerSprite;
        }
    }

    void GetRandomQuestion()
    {
        int index = UnityEngine.Random.Range(0, questions.Count);
        _currentQuestion = questions[index];

        if (questions.Contains(_currentQuestion))
        {
            questions.Remove(_currentQuestion);
        }
    }
    
    void DisplayQuestion()
    {
        questionText.text = _currentQuestion.GetQuestion();

        for (int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = _currentQuestion.GetAnswer(i);
        }
    }
    
    public void OnAnswerSelected(int index)
    {
        _hasAnsweredEarly = true;
        DisplayAnswer(index);
        SetButtonState(false);
        _timer.CancelTimer();
        scoreText.text = "Score: " + _scoreKeeper.CalculateScore() + "%";
    }

    void SetButtonState(bool state)
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            Button button = answerButtons[i].GetComponent<Button>();
            button.interactable = state;
        }
    }

    void Update()
    {
        timerImage.fillAmount = _timer.fillFraction;

        if (_timer.loadNextQuestion)
        {
            if (progressBar.value >= progressBar.maxValue)
            {
                isComplete = true;
                return;
            }
            
            _hasAnsweredEarly = false;
            GetNextQuestion();
            _timer.loadNextQuestion = false;
        }
        else if (!_hasAnsweredEarly && !_timer.isAnsweringQuestion)
        {
            DisplayAnswer(-1);  // make sure we don't pass in the correct answer
            SetButtonState(false);
        }
    }

    private void DisplayAnswer(int index)
    {
        Image buttonImage;
        
        if (index == _currentQuestion.GetCorrectAnswerIndex())
        {
            questionText.text = "Correct!";
            buttonImage = answerButtons[index].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
            _scoreKeeper.IncrementCorrectAnswers();
        }
        else
        {
            int correctAnswerIndex = _currentQuestion.GetCorrectAnswerIndex();
            questionText.text = "Sorry, the correct answer was:\n" + _currentQuestion.GetAnswer(correctAnswerIndex);
            buttonImage = answerButtons[correctAnswerIndex].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
        }
    }
}
