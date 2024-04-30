using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class QuizManager : MonoBehaviour
{
 
    [System.Serializable]
    public class Questions
    {
        [Header("Questions")]
        [SerializeField] public string Question;
        [Header("Options")]
        [SerializeField] public string[] Options;
        [SerializeField] public string CorrectAnswer;
        [Header("The Model That Should Appear")]
        [SerializeField] public GameObject CorrectPrefab;
    }

    [Header("Set Questions/Options")]
    [SerializeField] public Questions[] QuestionStore;


    [Header("Component References")]
    public GameObject[] Buttons;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI questionIndexText;
    public TextMeshProUGUI scoreText;
    public GameObject nextButton;
    public GameObject scoreGameObject;
    public GameObject CorrectPrefab;
    [Header("Trackers")]

    public int currentQuestionIndex;
    private bool questionAnswered;
    public int score;
    private int totalQuestions;
    public bool bShuffleQuestions, bRepeatQuestions, bShuffleOptions;

    public enum QuizType
    {
        animal
    }

    public QuizType quiztype;
    

    // Start is called before the first frame update
    void Start()
    {
        if (bShuffleQuestions)
        {
            ShuffleQuestions();
        }

        currentQuestionIndex = 0;
        questionAnswered = false;
        score = 0;
        totalQuestions = QuestionStore.Length;

        DisplayQuestion(currentQuestionIndex);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShuffleQuestions()
    {
        int questionCount = QuestionStore.Length;
        for (int i = 0; i < questionCount - 1; i++)
        {
            int randomIndex = Random.Range(i, questionCount);
            Questions tempQuestion = QuestionStore[randomIndex];
            QuestionStore[randomIndex] = QuestionStore[i];
            QuestionStore[i] = tempQuestion;
        }
    }

    public void GetNextQuestion()
    {
        currentQuestionIndex++;

        if (currentQuestionIndex < QuestionStore.Length)
        {
          
            DisplayQuestion(currentQuestionIndex);
            
        }
        else
        {
          
            // End of questions, show total score and replay button
            nextButton.SetActive(false);
            scoreGameObject.SetActive(true);
            scoreText.text = "Score: " + score + "/" + totalQuestions;
            switch(quiztype)
            {
                case QuizType.animal:
                    PlayerPrefs.SetString("AnimalScore", score + "/" + totalQuestions);
                    break;

            }
           
        }

        questionAnswered = false;
    }

    public void ShuffleOptions()
    {
        if (bShuffleOptions)
        {
            int optionsCount = QuestionStore[currentQuestionIndex].Options.Length;
            for (int i = 0; i < optionsCount - 1; i++)
            {
                int randomIndex = Random.Range(i, optionsCount);
                string tempOption = QuestionStore[currentQuestionIndex].Options[randomIndex];
                QuestionStore[currentQuestionIndex].Options[randomIndex] = QuestionStore[currentQuestionIndex].Options[i];
                QuestionStore[currentQuestionIndex].Options[i] = tempOption;
            }
        }
    }

    public void Answer()
    {
        if (questionAnswered)
        {
            return;
        }

        questionAnswered = true;

        GameObject clickedButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        TextMeshProUGUI clickedButtonText = clickedButton.GetComponentInChildren<TextMeshProUGUI>();
        string selectedOption = clickedButtonText.text;

        // Find the correct answer button and change its color to green
        for (int i = 0; i < QuestionStore[currentQuestionIndex].Options.Length; i++)
        {
            string option = QuestionStore[currentQuestionIndex].Options[i];
            if (option == QuestionStore[currentQuestionIndex].CorrectAnswer)
            {
                Buttons[i].GetComponent<Image>().color = Color.green;

                if (clickedButtonText.text == option)
                {
                    score++;
                }
            }
            else if (clickedButtonText.text == option)
            {
                Buttons[i].GetComponent<Image>().color = Color.red;
            }
        }

        nextButton.SetActive(true);
    }



    private void DisplayQuestion(int index)
    {
        questionText.text = QuestionStore[index].Question;
        questionIndexText.text = "Question " + (index + 1) + "/" + totalQuestions;
        CorrectPrefab = QuestionStore[currentQuestionIndex].CorrectPrefab;
        if (bShuffleOptions)
        {
            ShuffleOptions();
        }

        for (int i = 0; i < Buttons.Length; i++)
        {
            TextMeshProUGUI buttonText = Buttons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = QuestionStore[index].Options[i];
            Buttons[i].GetComponent<Image>().color = Color.white;
        }
    }

    public void ReplayQuiz()
    {
        currentQuestionIndex = 0;
        questionAnswered = false;
        score = 0;
        scoreGameObject.SetActive(false);
        nextButton.SetActive(false);
        ShuffleQuestions();
        DisplayQuestion(currentQuestionIndex);
    }
}
