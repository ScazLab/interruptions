using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Survey : MonoBehaviour
{
    public MainGameController gameController;
    public GameObject[] questionGroupArr;
    public QA[] qaArr;
    public GameObject AnswerPanel;
    GameObject fillWarning;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("MainGameController").GetComponent<MainGameController>();
        qaArr = new QA[questionGroupArr.Length];
        fillWarning = GameObject.Find("Warning");
        fillWarning.SetActive(false);
    }

    public void SubmitAnswer(int buttonNum){
        for (int i = 0; i < qaArr.Length; i++){
            qaArr[i] = ReadQuestionAndAnswer(questionGroupArr[i]);
        }

        //if(buttonNum==0)
        //{
        if (qaArr[0].Answer == "" || qaArr[1].Answer == "" || qaArr[2].Answer == "")
        {
            fillWarning.SetActive(true);
        }
        else {
             //Debug.Log("IN DEMOGRAPHICS");
            demographicsData demInfo = new demographicsData(qaArr[0].Answer, qaArr[1].Answer, qaArr[2].Answer, gameController.timestamp);
            data demTest = new data(qaArr[0].Answer, qaArr[1].Answer, qaArr[2].Answer, gameController.timestamp);

            //Debug.Log(qaArr[0].Answer);
            gameController.demographicsResults.Add(demInfo);
            gameController.allDataResults.Add(demTest);

            gameController.changeScene();
        }

        /* if(buttonNum==1)
        {
            if (fill_demographics)
            {
                if (qaArr[0].Answer == "" || qaArr[1].Answer == "" || qaArr[2].Answer == "" || qaArr[3].Answer == "" || qaArr[4].Answer == "" || qaArr[5].Answer == "" || qaArr[6].Answer == "" || qaArr[6].Answer == "")
                {
                    fillWarning.text = "Please fill out all answers!";
                    return;
                }
            }

            //Debug.Log(gameController.timestamp);
            CLData CLinfo = new CLData(qaArr[0].Answer, qaArr[1].Answer, qaArr[2].Answer, qaArr[3].Answer, qaArr[4].Answer, qaArr[5].Answer, qaArr[6].Answer, gameController.timestamp);
            data CLtest = new data(gameController.PHASE, qaArr[0].Answer, qaArr[1].Answer, qaArr[2].Answer, qaArr[3].Answer, qaArr[4].Answer, qaArr[5].Answer, qaArr[6].Answer, gameController.timestamp);
            gameController.cognitiveLoadResults.Add(CLinfo);
            gameController.allDataResults.Add(CLtest);

            //Debug.Log("IN COGNITIVE LOAD");
        } */
    }

    QA ReadQuestionAndAnswer(GameObject questionGroup){
        QA result = new QA();

        GameObject q = questionGroup.transform.Find("Question").gameObject;
        GameObject a = questionGroup.transform.Find("Answer").gameObject;

        //Debug.Log("here");

        result.Question = q.GetComponent<Text>().text;
        //Debug.Log("Reading question: " + result.Question);
        //Debug.Log(a.GetComponentsInChildren<Toggle>());
        //Toggle[] test = a.GetComponentsInChildren<Toggle>();
        //Debug.Log(test.Length);


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
            //Debug.Log("Found provided answers: " + result.Answer);
        } // end checkbox type answers

        return result;
    }
}

[System.Serializable]
public class QA {
    public string Question = "";
    public string Answer = "";
}
