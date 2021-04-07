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
        //Debug.Log(gameController.timestamp);
        gameController.allDataResults.Add(intTest);

        if (gameController.phase == Constants.PHASE_TUTORIAL)
        {
            if (card == answer)
            {
                //Debug.Log("CORRECT");
                gameController.correct_in_row += 1;
            }
            else
            {
                //Debug.Log("WRONG");
                gameController.correct_in_row = 0;
            }
        }

        answerIndex++;
        timeTaken = 0.0f;
        //Debug.Log("CHANGING FROM AREA...");
        gameController.changeScene();
    }

    void setAreaQuestions()
    {
        /*Debug.Log("Question: ("
            + prompt.c1_first_operand + prompt.c1_operator + prompt.c1_second_operand + ") ? ("
            + prompt.c2_first_operand + prompt.c2_operator + prompt.c2_second_operand + ") with answer: "
            + "card #" + prompt.answer);*/

        /*
        int answerLeft = 0;

        string leftFirst = prompt.c1_first_operand; //numbers[Random.Range(0,8)];
        string leftSecond = prompt.c1_second_operand; //numbers[Random.Range(0,8)];
        string rightFirst = prompt.c2_first_operand; // numbers[Random.Range(0,8)];
        string rightSecond = prompt.c2_second_operand; // numbers[Random.Range(0,8)];


        int leftVal = Int32.Parse(leftFirst) * Int32.Parse(leftSecond);
        int rightVal = Int32.Parse(rightFirst) * Int32.Parse(rightSecond);

        while(leftVal==rightVal){
            leftFirst = numbers[Random.Range(0,8)];
            leftVal = Int32.Parse(leftFirst) * Int16.Parse(leftSecond);
        }

        if(rightVal>leftVal)
        {
            answerLeft=1;
        }
        */

        //setLeftText(leftFirst, leftSecond);
        //setRightText(rightFirst, rightSecond);
        setCardText(prompt.c1_first_operand, prompt.c1_second_operand, prompt.c1_operator, 1); // set text on left card
        setCardText(prompt.c2_first_operand, prompt.c2_second_operand, prompt.c2_operator, 2); // set text on right card

        SequenceReader.mathSequenceIndex += 1; // mark current question as completed

        // return prompt.answer; // made this method void

    }

    void setCardText(string first_operand, string second_operand, string operation, int card){
        string expression = first_operand + " " + operation + " " + second_operand;
        if (card == 1) { // left card
            leftText.text = expression;
        } else { // right card
            rightText.text = expression;
        }
    }

    /*
    string setAreaString(string leftTerm, string rightTerm)
    {
        string areaString = leftTerm + " ⁎ " + rightTerm;

        return areaString;
    }

    void setLeftText(string leftTerm, string rightTerm)
    {

        leftText.text = setAreaString(leftTerm, rightTerm);
    }

    void setRightText(string leftTerm, string rightTerm)
    {

        rightText.text = setAreaString(leftTerm, rightTerm);
    }
    */

    // Start is called before the first frame update
    void Start()
    {
        //uncomment this for when running the gamecontroller
        gameController = GameObject.Find("MainGameController").GetComponent<MainGameController>();

        leftText = GameObject.Find("Answer1").GetComponentsInChildren<Text>()[0];
        rightText = GameObject.Find("Answer2").GetComponentsInChildren<Text>()[0];

        // answer = setAreaQuestions();
        setAreaQuestions();

        Left = GameObject.Find("Left").GetComponentsInChildren<Button>()[0];
        Right = GameObject.Find("Right").GetComponentsInChildren<Button>()[0];

        Left.onClick.AddListener(() => recordAnswers(1));
        Right.onClick.AddListener(() => recordAnswers(2));
    }

    // Update is called once per frame
    void Update()
    {
        timeTaken += Time.deltaTime;
    }
}
