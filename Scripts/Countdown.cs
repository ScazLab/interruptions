using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Countdown : MonoBehaviour
{

    float timeLeft = 300.0f;
    public Text text;
    public Text timeout;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        timeLeft -= Time.deltaTime;
        float minutes = Mathf.FloorToInt(timeLeft / 60);
        float seconds = Mathf.FloorToInt(timeLeft % 60);

        if (timeLeft > 0)
        {
            timeout.text = "";
            text.text = minutes + ":" + seconds + "s";
        }
        else
        {
            text.text = "";
            timeout.text = "PRESS START TO NOT TIMEOUT";
        }
        if (timeLeft < -60)
        {
            SceneManager.LoadScene("End_nopay");
        }
    }
}
