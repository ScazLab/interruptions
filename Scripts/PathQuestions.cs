using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class PathQuestions : MonoBehaviour
{
	private MainGameController gameController;
    private Text[] textList;
    private Toggle[] toggleList;
	public GameObject warningMsg;

	GameObject feedback_correct;
    GameObject feedback_incorrect;

	RawImage option1a;
	RawImage option1b;
	RawImage option1c;
	RawImage option1d;
	RawImage option1e;
	RawImage[] option1 = new RawImage[5];

	RawImage option2a;
	RawImage option2b;
	RawImage option2c;
	RawImage option2d;
	RawImage option2e;
	RawImage[] option2 = new RawImage[5];

	RawImage option3a;
	RawImage option3b;
	RawImage option3c;
	RawImage option3d;
	RawImage option3e;
	RawImage[] option3 = new RawImage[5];

	RawImage option4a;
	RawImage option4b;
	RawImage option4c;
	RawImage option4d;
	RawImage option4e;
	RawImage[] option4 = new RawImage[5];

	private int currentNumber = 1;
	private IDictionary<int, string> paths = new Dictionary<int, string>();
	private List<SequenceReader.PathItem> answer_order = SequenceReader.pathSequence[SequenceReader.pathSequenceIndex].answer_order;
	public List<string> paths_selected;
	float timeTaken = 0.0f;

    // Start is called before the first frame update
    void Start()
    {

		// get elements in answer panel
    	gameController = GameObject.Find("MainGameController").GetComponent<MainGameController>();
		gameController.interrupts_this_round = SequenceReader.pathSequence[SequenceReader.pathSequenceIndex].interrupts;
        //GameObject[] choices = GameObject.FindGameObjectsWithTag("AnswerChoice");
		warningMsg = GameObject.Find("Warning");
		textList = GameObject.Find("Answers").GetComponentsInChildren<Text>();
        toggleList = GameObject.Find("Answers").GetComponentsInChildren<Toggle>();
		//imageList = GameObject.Find("Answers").GetComponentsInChildren<RawImage>();

		GameObject choice0 = GameObject.Find("Path-0");
		GameObject choice1 = GameObject.Find("Path-1");
		GameObject choice2 = GameObject.Find("Path-2");
		GameObject choice3 = GameObject.Find("Path-3");
		GameObject choice4 = GameObject.Find("Path-4");
		GameObject choice5 = GameObject.Find("Path-5");
		GameObject choice6 = GameObject.Find("Path-6");
		GameObject choice7 = GameObject.Find("Path-7");
		GameObject choice8 = GameObject.Find("Path-8");

		GameObject[] choices = new GameObject[9];
		choices[0] = choice0;
		choices[1] = choice1;
		choices[2] = choice2;
		choices[3] = choice3;
		choices[4] = choice4;
		choices[5] = choice5;
		choices[6] = choice6;
		choices[7] = choice7;
		choices[8] = choice8;


		option1a = GameObject.Find("Option1a").GetComponentInChildren<RawImage>();
		option1b = GameObject.Find("Option1b").GetComponentInChildren<RawImage>();
		option1c = GameObject.Find("Option1c").GetComponentInChildren<RawImage>();
		option1d = GameObject.Find("Option1d").GetComponentInChildren<RawImage>();
		option1e = GameObject.Find("Option1e").GetComponentInChildren<RawImage>();

		option2a = GameObject.Find("Option2a").GetComponentInChildren<RawImage>();
		option2b = GameObject.Find("Option2b").GetComponentInChildren<RawImage>();
		option2c = GameObject.Find("Option2c").GetComponentInChildren<RawImage>();
		option2d = GameObject.Find("Option2d").GetComponentInChildren<RawImage>();
		option2e = GameObject.Find("Option2e").GetComponentInChildren<RawImage>();

		option3a = GameObject.Find("Option3a").GetComponentInChildren<RawImage>();
		option3b = GameObject.Find("Option3b").GetComponentInChildren<RawImage>();
		option3c = GameObject.Find("Option3c").GetComponentInChildren<RawImage>();
		option3d = GameObject.Find("Option3d").GetComponentInChildren<RawImage>();
		option3e = GameObject.Find("Option3e").GetComponentInChildren<RawImage>();

		option4a = GameObject.Find("Option4a").GetComponentInChildren<RawImage>();
		option4b = GameObject.Find("Option4b").GetComponentInChildren<RawImage>();
		option4c = GameObject.Find("Option4c").GetComponentInChildren<RawImage>();
		option4d = GameObject.Find("Option4d").GetComponentInChildren<RawImage>();
		option4e = GameObject.Find("Option4e").GetComponentInChildren<RawImage>();

		option1[0] = option1a;
		option1[1] = option1b;
		option1[2] = option1c;
		option1[3] = option1d;
		option1[4] = option1e;

		option2[0] = option2a;
		option2[1] = option2b;
		option2[2] = option2c;
		option2[3] = option2d;
		option2[4] = option2e;

		option3[0] = option3a;
		option3[1] = option3b;
		option3[2] = option3c;
		option3[3] = option3d;
		option3[4] = option3e;

		option4[0] = option4a;
		option4[1] = option4b;
		option4[2] = option4c;
		option4[3] = option4d;
		option4[4] = option4e;

		feedback_correct = GameObject.Find("Correct");
        feedback_incorrect = GameObject.Find("Incorrect");
        feedback_correct.SetActive(false);
        feedback_incorrect.SetActive(false);

		warningMsg.SetActive(false);
		paths_selected = new List<string>(answer_order.Count);

		// Debug.Log(answer_order.Count);
		// Debug.Log("--------");
		// set image labels according to answer order
		for (int i = 1; i <= answer_order.Count; i++){
			//choices[i - 1].SetActive(true);

			//Loads all the images
			if(i-1==0)
			{
				option1[answer_order[i-1].id-1].enabled = true;
			}

			if(i-1==1)
			{
				option2[answer_order[i-1].id-1].enabled = true;
			}

			if(i-1==2)
			{
				option3[answer_order[i-1].id-1].enabled = true;
			}

			if(i-1==3)
			{
				option4[answer_order[i-1].id-1].enabled = true;
			}


			paths.Add(i, answer_order[i - 1].image);
		}

		// hide all other options
		/*for (int j = answer_order.Count; j < choices.Length; j++)
		{
			choices[j].SetActive(false);
		}*/

		for (int j = answer_order.Count; j < choices.Length; j++)
		{
			choices[j].SetActive(false);
		}



	}
	public void updateDisplays(int toggleNum)
    {
    	if(toggleList[toggleNum].isOn)
    	{
    		textList[toggleNum].text = currentNumber.ToString();
    		currentNumber++;
    	}
    	else
    	{
    		int previousNum = int.Parse(textList[toggleNum].text);
    		textList[toggleNum].text = "";
    		currentNumber--;

    		//Debug.Log(answer_order.Count);

    		for(int i = 0; i < answer_order.Count; i++)
    		{
    			if(toggleList[i].isOn)
    			{
	    			int currVal = int.Parse(textList[i].text);
	    			if(currVal > previousNum)
	    			{
	    				currVal--;
	    				textList[i].text = currVal.ToString();
	    			}
	    		}
    		}
    	}
    }

    public void collectAnswers()
    {
		// check if all answers are provided
    	for(int i = 0; i < answer_order.Count; i++)
    	{
			if (string.IsNullOrEmpty(textList[i].text)) {
				warningMsg.SetActive(true);
				return; // exit function prematurely
			}
    	}
			string givenOrder = "";
			for(int i = 0; i < answer_order.Count; i++)
	    	{
				givenOrder += textList[i].text;
			}

		string givenAnswer = "";
		for(int i = 0; i < answer_order.Count; i++)
    	{
			givenAnswer+=answer_order[givenOrder.IndexOf((i+1).ToString())].id.ToString();
			paths_selected.Add(paths[int.Parse(textList[i].text)]);

    	}

		string correctAnswer = correct();

		int rightOptions = 0;
		int totalOptions = correctAnswer.Length;

		for(int i=0; i<totalOptions;i++){
			if(givenAnswer[i].Equals(correctAnswer[i]))
			{
				rightOptions++;
			}
		}

		double percentRight = (double) rightOptions / (double) totalOptions * 100;

		string answerValue = percentRight.ToString() + "%";

		data pathData = new data(int.Parse(givenAnswer), int.Parse(correctAnswer), timeTaken, "path", gameController.PHASE, answerValue, gameController.timestamp);

        gameController.allDataResults.Add(pathData);
    	timeTaken=0.0f;
		if (gameController.PHASE == "TUTORIAL"){
            setFeedback(percentRight == 100.0);
        }
		if (percentRight < 90 && gameController.phase == Constants.PHASE_TUTORIAL)
		{
			// Debug.Log("got draw task wrong");
			gameController.interruption_just_happened = 0;
			SceneManager.LoadScene("Instructions_Draw");
		}
		else
		{
			SequenceReader.pathSequenceIndex += 1; // mark question as completed
			gameController.interruption_just_happened = 0;
			gameController.changeScene();
		}
    }

	IEnumerator delayTransition(){
        Debug.Log(Time.time);
        yield return new WaitForSeconds(5);
        Debug.Log(Time.time);
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
        StartCoroutine(delayTransition()); 
    }

	public string correct(){
		List<SequenceReader.PathItem> correct_answer = SequenceReader.pathSequence[SequenceReader.pathSequenceIndex].question;
		string correctAnswer = "";

		for (int i = 0; i < correct_answer.Count; i++){
			correctAnswer += correct_answer[i].id;
		}
		return correctAnswer;
	}

    // Update is called once per frame
    void Update()
    {
        timeTaken+=Time.deltaTime;
    }
}
