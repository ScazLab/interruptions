using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StroopAnswers
{
    public int participantAnswer;
    public int rightAnswer;
    public float time;
    public string interruptionType;
}

public class Stroop : MonoBehaviour
{

    public MainGameController gameController;
    private SequenceReader.StroopQuestion prompt = SequenceReader.stroopSequence[SequenceReader.stroopSequenceIndex];

    StroopAnswers[] answers = new StroopAnswers[999];

    string[] colors = {"red", "black", "yellow", "green", "blue"};


    float timeTaken = 0.0f;
    int answerIndex = 0;
    Button Yes;
    Button No;
    Text leftText;
    Text rightText;


    public Text question;


    public void recordAnswers(int number)
    {
        int answer = prompt.answer;
        //Debug.Log("The answer was button number: " + number);
        //Debug.Log("Time taken was: " + timeTaken);
        answers[answerIndex] = new StroopAnswers();
        answers[answerIndex].participantAnswer = number;
        answers[answerIndex].rightAnswer = answer;
        answers[answerIndex].time = timeTaken;
        answers[answerIndex].interruptionType = "stroop";

        //bool isCorrect = false;
        string answerValue = "WRONG";

        if(number==answer)
        {
            //isCorrect = true;
            answerValue = "CORRECT";
        }

        interruptionData currentInterruption = new interruptionData(
            number,
            answer,
            timeTaken,
            "stroop",
            gameController.phase,
            answerValue,
            gameController.timestamp
        );

        gameController.interruptionsResults.Add(currentInterruption);

        data intTest = new data(number, answer, timeTaken, "stroop", gameController.PHASE, answerValue, gameController.timestamp);

        gameController.allDataResults.Add(intTest);

        answerIndex++;
        timeTaken=0.0f;

        if (gameController.phase == Constants.PHASE_TUTORIAL)
        {
            if (number == answer)
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

        gameController.changeScene();

    }

    void setStroopQuestions()
    {
        setCardText(prompt.c1_color, prompt.c1_text, 1); // set left card content
        setCardText(prompt.c2_color, prompt.c2_text, 2); // set right card content

        SequenceReader.stroopSequenceIndex += 1; // mark current question as completed
    }

    void setCardText(string color, string word, int card){
        string html = "<color=" + color + ">" + word + "</color>";
        if (card == 1) { // left card
            leftText.text = html;
        } else { // right card
            rightText.text = html;
        }
    }

    string setColorString(string color, string word)
    {
       string colorString = "<color=";
       colorString += color + ">";
       colorString += word;
       colorString += "</color>";
       return colorString;
    }

    void setLeftText(string color, string word)
    {

	     leftText.text = setColorString(color, word);
    }

    void setRightText(string color, string word)
    {

       rightText.text = setColorString(color, word);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("MainGameController").GetComponent<MainGameController>();

        //Text [] answerTexts = new Text[2];
        leftText = GameObject.Find("Answer1").GetComponentsInChildren<Text>()[0];
        rightText = GameObject.Find("Answer2").GetComponentsInChildren<Text>()[0];
        
        // answer = setStroopQuestions();
        setStroopQuestions();

        Yes = GameObject.Find("Yes").GetComponentsInChildren<Button>()[0];
        No = GameObject.Find("No").GetComponentsInChildren<Button>()[0];

        Yes.onClick.AddListener(() => recordAnswers(1));
        No.onClick.AddListener(() => recordAnswers(0));

    }

    // Update is called once per frame
    void Update()
    {
        timeTaken += Time.deltaTime;
    }
}
