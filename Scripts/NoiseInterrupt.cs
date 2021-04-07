using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseInterrupt : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClipArray;
    //private SequenceReader.NoiseQuestion prompt = SequenceReader.noiseSequence[SequenceReader.noiseSequenceIndex];
    private MainGameController gameController;
    float timeTaken = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        /*gameController = GameObject.Find("MainGameController").GetComponent<MainGameController>();
        AudioClip clip = audioClipArray[prompt.sound];
        audioSource.PlayOneShot(clip);
        StartCoroutine(ExampleCoroutine(clip.length));
        SequenceReader.noiseSequenceIndex += 1; // mark current question as completed*/
    }

    // Update is called once per frame
    void Update()
    {
      timeTaken += Time.deltaTime;
    }

    // delays until sound interruption is completed
    IEnumerator ExampleCoroutine(float time)
    {

        //Print the time of when the function is first called.
        //Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(time);

        //After we have waited 5 seconds print the time again.
        //Debug.Log("Finished Coroutine at timestamp : " + Time.time);
        
        /*data noiseData = new data(-1, prompt.sound, timeTaken, "noise", gameController.PHASE, "N/A", gameController.timestamp);
        gameController.allDataResults.Add(noiseData);*/
        timeTaken = 0.0f;
        gameController.changeScene();
    }
}
