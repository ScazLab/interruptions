using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ClickStartButton : MonoBehaviour
{

    public MainGameController gameController;
    public Button myButton;
    // Start is called before the first frame update

    private void Awake()
    {
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(() => { ButtonClicked(); });

    }
    private void Start()
    {
        gameController = GameObject.Find("MainGameController").GetComponent<MainGameController>();
    }

    private void ButtonClicked()
    {
        gameController.changeScene();
        //SceneManager.LoadScene("Survey", LoadSceneMode.Additive);
        //SceneManager.LoadScene("Demographics");
    }
}
