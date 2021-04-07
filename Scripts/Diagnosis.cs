using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Diagnosis : MonoBehaviour
{
    public MainGameController gameController;
    public GameObject[] questionGroupArr;
    Toggle ASD, CB, HI, ADHD, PreferNot, None;
    GameObject fillWarning;
    Button submit;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("MainGameController").GetComponent<MainGameController>();

        fillWarning = GameObject.Find("Warning");
        fillWarning.SetActive(false);

        ASD = GameObject.Find("ASD").GetComponent<Toggle>();
        CB = GameObject.Find("CB").GetComponent<Toggle>();
        HI = GameObject.Find("HI").GetComponent<Toggle>();
        ADHD = GameObject.Find("ADHD").GetComponent<Toggle>();
        PreferNot = GameObject.Find("PreferNot").GetComponent<Toggle>();
        None = GameObject.Find("None").GetComponent<Toggle>();

        submit = GameObject.Find("SubmitButton").GetComponent<Button>();
        submit.onClick.AddListener(() => { answersSubmitted(); });
    }

    void answersSubmitted()
    {
      if(!ASD.isOn && !CB.isOn && !HI.isOn && !ADHD.isOn && !PreferNot.isOn && !None.isOn)
      {
        Debug.Log("set warning");
        fillWarning.SetActive(true);
      }
      else
      {
        data diagnosis = new data(ASD.isOn.ToString(), CB.isOn.ToString(), HI.isOn.ToString(), ADHD.isOn.ToString(), PreferNot.isOn.ToString(), None.isOn.ToString(), gameController.timestamp);
        gameController.allDataResults.Add(diagnosis);
        gameController.changeScene();
      }
    }
}
