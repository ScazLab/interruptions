using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AreaAnswers
{
    public int participantAnswer;
    public int rightAnswer;
    public float time;
    public string interruptionType;
}

public class Area : MonoBehaviour
{
    public MainGameController gameController;
    private SequenceReader.MathQuestion prompt = SequenceReader.mathSequence[SequenceReader.mathSequenceIndex];

    AreaAnswers[] area_answers = new AreaAnswers[999];

    string[] numbers = { "2", "3", "4", "5", "6", "7", "8", "9" };

    int answerIndex = 0;
    float timeTaken = 0.0f;

    Button Left;
    Button Right;
    Text leftText; // left card
    Text rightText; // right card

    Text question;

    GameObject feedback_correct;
    GameObject feedback_incorrect;

    public void recordAnswers(int card)
    {
        int answer = prompt.answer;

        area_answers[answerIndex] = new AreaAnswers();
        area_answers[answerIndex].participantAnswer = card;
        area_answers[answerIndex].rightAnswer = answer;
        area_answers[answerIndex].time = timeTaken;
        area_answers[answerIndex].interruptionType = "area";

        // check if correct card is selected
        string answerValue = "WRONG";
        if (card == answer) { answerValue = "CORRECT"; }

        interruptionData currentInterruption = new interruptionData(
            card,
            answer,
            timeTaken,
            "area",
            gameController.phase,
            answerValue,
            gameController.timestamp
        );

        gameController.interruptionsResults.Add(currentInterruption);

        data intTest = new data(card, answer, timeTaken, "area", gameController.PHASE, answerValue, gameController.timestamp);
        gameController.allDataResults.Add(intTest);

        if (gameController.phase == Constants.PHASE_TUTORIAL)
        {
            if (card == answer)
            {
                gameController.correct_in_row += 1;
            }
            else
            {
                gameController.correct_in_row = 0;
            }
        }

        answerIndex++;
        timeTaken = 0.0f;
        if (gameController.PHASE == "TUTORIAL"){
            setFeedback(card == answer);
            Invoke("placeholderDelay", 1);
        }        
        else {
            // don't delay transition to next question
            gameController.changeScene();
        }
    }

    void placeholderDelay()
    {
        // will delay before changing the scene
        gameController.changeScene();
    }

    void setAreaQuestions()
    {
        setCardText(prompt.c1_first_operand, prompt.c1_second_operand, prompt.c1_operator, 1); // set text on left card
        setCardText(prompt.c2_first_operand, prompt.c2_second_operand, prompt.c2_operator, 2); // set text on right card

        SequenceReader.mathSequenceIndex += 1; // mark current question as completed
    }

    void setCardText(string first_operand, string second_operand, string operation, int card){
        string expression = first_operand + " " + operation + " " + second_operand;
        if (card == 1) { // left card
            leftText.text = expression;
        } else { // right card
            rightText.text = expression;
        }
    }

    void setFeedback(bool status){
        if (status == true){
            feedback_correct.SetActive(true);
            feedback_incorrect.SetActive(false);
        }
        else {
            feedback_incorrect.SetActive(true);
            feedback_correct.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("MainGameController").GetComponent<MainGameController>();

        leftText = GameObject.Find("Answer1").GetComponentsInChildren<Text>()[0];
        rightText = GameObject.Find("Answer2").GetComponentsInChildren<Text>()[0];

        setAreaQuestions();

        Left = GameObject.Find("Left").GetComponentsInChildren<Button>()[0];
        Right = GameObject.Find("Right").GetComponentsInChildren<Button>()[0];

        Left.onClick.AddListener(() => recordAnswers(1));
        Right.onClick.AddListener(() => recordAnswers(2));
        
        feedback_correct = GameObject.Find("Correct");
        feedback_incorrect = GameObject.Find("Incorrect");
        feedback_correct.SetActive(false);
        feedback_incorrect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        timeTaken += Time.deltaTime;
        
    }
}
