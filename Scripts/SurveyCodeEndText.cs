using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurveyCodeEndText : MonoBehaviour
{
	private MainGameController gameController;
	public Text mytext = null;
	void Start()
	{
		mytext.text  = "";
		int randomNumbers = Random.Range(100000, 999999);
		int randomNumbersTwo = Random.Range(100000, 999999);
		gameController = GameObject.Find("MainGameController").GetComponent<MainGameController>();
		gameController.part_id += randomNumbers.ToString() + "-" + randomNumbersTwo.ToString();
		Debug.Log(randomNumbers);
		mytext.text = mytext.text + "" + randomNumbers + "-" + randomNumbersTwo;
	}
}
