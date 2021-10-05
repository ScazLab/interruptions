using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Feedback : MonoBehaviour
{
    public MainGameController gameController;
    public GameObject[] questionGroupArr;
    public QA[] qaArr;
    GameObject fillWarning;
    Button submit;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("MainGameController").GetComponent<MainGameController>();

        fillWarning = GameObject.Find("Warning");
        fillWarning.SetActive(false);

        qaArr = new QA[questionGroupArr.Length];

        submit = GameObject.Find("SubmitButton").GetComponent<Button>();
        submit.onClick.AddListener(() => { answersSubmitted(); });
    }

    void answersSubmitted()
    {
        for (int i = 0; i < qaArr.Length; i++){
            qaArr[i] = ReadQuestionAndAnswer(questionGroupArr[i]);
        }

        if (qaArr[0].Answer == "" || qaArr[1].Answer == "")
        {
            fillWarning.SetActive(true);
        } else {

            string currScene = SceneManager.GetActiveScene().name;
            currScene = currScene.Replace("Difficulty-", "");
            if(currScene=="Draw")
            {
              currScene="path";
            }
            else if(currScene=="Math")
            {
              currScene = "area";
            }

            //CLData CLinfo = new CLData(qaArr[0].Answer, qaArr[1].Answer, qaArr[2].Answer, qaArr[3].Answer, qaArr[4].Answer, qaArr[5].Answer, qaArr[6].Answer, gameController.timestamp);
            data CLtest = new data(gameController.PHASE, qaArr[0].Answer, qaArr[1].Answer, currScene, gameController.timestamp);
            //gameController.cognitiveLoadResults.Add(CLinfo);
            gameController.allDataResults.Add(CLtest);
            //gameController.changeScene();

            if(gameController.PHASE=="TESTING"&&(currScene=="area"||currScene=="Stroop"))
            {
              gameController.dataSet = true;
            }

            Debug.Log("submit responses");
            Debug.Log(currScene);
            gameController.changeScene();
        }
    }
    QA ReadQuestionAndAnswer(GameObject questionGroup){
        QA result = new QA();

        GameObject q = questionGroup.transform.Find("Question").gameObject;
        GameObject a = questionGroup.transform.Find("Answer").gameObject;

        result.Question = q.GetComponent<Text>().text;

        // store answer from various answer types
        if (a.GetComponentsInChildren<Toggle>().Length != 0){
            //Debug.Log("Reading answer type: " + a.transform.childCount + " radio boxes");
            for (int i = 0; i < a.transform.childCount; i++){
                if (a.transform.GetChild(i).GetComponent<Toggle>().isOn){
                    result.Answer = a.transform.GetChild(i).Find("Label").GetComponent<Text>().text;
                    //Debug.Log("Found selected answer: " + result.Answer);
                    break;
                }
            };
        } // end radio button type answers
        else if (a.GetComponent<InputField>() != null) {
            //Debug.Log("Reading answer type: input field");
            result.Answer = a.transform.Find("Text").GetComponent<Text>().text;
            //Debug.Log("Found provided answer: " + result.Answer);
        } // end input type answers
        else if (a.GetComponentsInChildren<Toggle>() == null && a.GetComponent<InputField>() == null) {
            //Debug.Log("Reading answer type: " + a.transform.childCount + " checkboxes");
            string s = "";
            int counter = 0;

            for (int i = 0; i < a.transform.childCount; i++){
                if (a.transform.GetChild(i).GetComponent<Toggle>().isOn){
                    if (s != "") {
                        s = s + ", ";
                    }
                    s = s + a.transform.GetChild(i).Find("Label").GetComponent<Text>().text;
                    //Debug.Log("\tFound a selected answer: @" + i + " = " + a.transform.GetChild(i).Find("Label").GetComponent<Text>().text);
                }
                if (i == a.transform.childCount - 1){ // last iteration
                    s = s + ".";
                }
                counter++;
            }

            result.Answer = s;
        } // end checkbox type answers

        return result;
    }
}
