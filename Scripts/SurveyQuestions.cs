using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Question
{
    public string question;
    public List<string> answers;
}

public class SurveyQuestions : MonoBehaviour
{
    
    List<Question> all_survey_questions = new List<Question>();

    public Text question;
    public Button A1;
    public Button A2;
    public Button A3;
    public Button A4;
    public Button A5;
    public Button A6;
    public Button A7;
    public Button A8;


    private void Awake()
    {
        populateSurveyQuestions();
    }

    void populateSurveyQuestions()
    {
        Question q = new Question();
        //Question q;

        q.question = "How old are you?";
        List<string> answerList = new List<string>() { "18-20", "21-25", "26-30", "31-35", "35-40", "41-45", "46-50", "51+" };
        q.answers = answerList;
        all_survey_questions.Add(q);

        q.question = "What is your gender?";
        answerList = new List<string>() { "Male", "Female", "Non-binary", "Prefer Not to Answer"};
        q.answers = answerList;
        all_survey_questions.Add(q);



    }
    // Start is called before the first frame update
    void Start()
    {
        Question current_question = all_survey_questions[0];
        question.text = current_question.question;

        int n_ans = current_question.answers.Count;

        A1.GetComponentInChildren<Text>().text = current_question.answers[0];
        A2.GetComponentInChildren<Text>().text = current_question.answers[1];
        A3.GetComponentInChildren<Text>().text = current_question.answers[2];
        A4.GetComponentInChildren<Text>().text = current_question.answers[3];

        if (n_ans > 4) { A5.GetComponentInChildren<Text>().text = current_question.answers[4]; }
        if (n_ans > 5) { A6.GetComponentInChildren<Text>().text = current_question.answers[5]; }
        if (n_ans > 6) { A7.GetComponentInChildren<Text>().text = current_question.answers[6]; }
        if (n_ans > 7) { A8.GetComponentInChildren<Text>().text = current_question.answers[7]; }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
