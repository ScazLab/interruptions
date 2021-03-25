using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ClickedInstrButton : MonoBehaviour
{

    public Button myButton;
    public int button_id; // 0 is Hanoi, 1 is Drawing
    // Start is called before the first frame update

    private void Awake()
    {
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(() => { ButtonClicked(); });

    }

    private void ButtonClicked()
    {
        //Debug.Log("Pressed Instructions Button");
        if (button_id == 0)
        {
            SceneManager.LoadScene("HanoiTask4");
        }
        else
        {
            SceneManager.LoadScene("DrawTask");
        }
        //SceneManager.LoadScene("Demographics");
    }
}
