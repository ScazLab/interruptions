using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class CountDown1 : MonoBehaviour
{
    float timeLeft = 300.0f;

    public Text text;


    void Update()
    {
        timeLeft -= Time.deltaTime;
        text.text = "time Left:" + timeLeft;
        if (timeLeft < 0)
        {
            SceneManager.LoadScene("End");
        }
    }
}