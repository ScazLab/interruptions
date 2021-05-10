
using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
//using System.Security.Cryptography.X509Certificates;

public class mono_gmail : MonoBehaviour
{
    private MainGameController gameController;
    private bool sendMail = true;

    void Start()
    {
        Debug.Log("End of the game");

        gameController = GameObject.Find("MainGameController").GetComponent<MainGameController>();
        Debug.Log(gameController.endEarly);
    }



   IEnumerator Upload()
   {
      WWWForm form = new WWWForm();
      Debug.Log("sending....");
      Debug.Log(SceneManager.GetActiveScene().name);

      if(SceneManager.GetActiveScene().name == "End")
      {
        form.AddField("id", gameController.part_id);
      }
      else
      {
        form.AddField("id", "ENDED EARLY");
      }
      form.AddField("msg", gameController.sb.ToString());

       using (UnityWebRequest www = UnityWebRequest.Post("https://interruptions2.uk.r.appspot.com/", form))
        {
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
      }

    void Update()
    {

      if(gameController.part_id!=""&&sendMail)
      {
        StartCoroutine(Upload());
        sendMail = false;
      }
      else if(gameController.endEarly&&sendMail)
      {
        StartCoroutine(Upload());
        sendMail = false;
      }
    }
}
