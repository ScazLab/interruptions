using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class PathVideo : MonoBehaviour
{
    public RawImage image;
    private VideoPlayer player;
    private VideoSource source;
    private List<VideoPlayer> videoPlayerList;
    private List<SequenceReader.PathItem> question = SequenceReader.pathSequence[SequenceReader.pathSequenceIndex].question;
    private int videoIndex = 0;
    private MainGameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        Application.runInBackground = true;
        StartCoroutine(playVideo(true));
        gameController = GameObject.Find("MainGameController").GetComponent<MainGameController>();
        if ((gameController.experimental == 2) && (gameController.phase == Constants.PHASE_TRAINING))
        {
            gameController.interrupts_this_round = 0;
        }
        else
        {
            gameController.interrupts_this_round = SequenceReader.pathSequence[SequenceReader.pathSequenceIndex].interrupts;
        }
    }

    IEnumerator playVideo(bool firstRun = true)
    {
        if (question == null || question.Count <= 0)
        {
            //Debug.LogError("Assign VideoClips using " + SequenceReader.pathStructureCSV);
            yield break;
        }

        //Init videoPlayerList first time this function is called
        if (firstRun)
        {
            videoPlayerList = new List<VideoPlayer>();
            for (int i = 0; i < question.Count; i++)
            {
                //Create new object to hold the video and then make it a child of this object
                GameObject vidHolder = new GameObject("VP" + i);
                vidHolder.transform.SetParent(transform);

                //Add VideoPlayer to the GameObject
                VideoPlayer videoPlayer = vidHolder.AddComponent<VideoPlayer>();
                videoPlayerList.Add(videoPlayer);

                //Disable play on awake
                videoPlayer.playOnAwake = false;

                //Get video clip from url
                videoPlayer.source = VideoSource.Url;
                //Debug.Log("below is the url!");
                //Debug.Log(VideoSource.Url);
                //Set video Url to play
                //videoPlayer.url = question[i].video;
                videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, question[i].video);
                //Debug.Log(videoPlayer.url);
            }
        }

        //Make sure that the NEXT VideoPlayer index is valid
        if (videoIndex >= videoPlayerList.Count)
        {
            //Debug.Log("HERE");
            yield break;
        }

        //Prepare video
        videoPlayerList[videoIndex].Prepare();

        //Wait until this video is prepared
        while (!videoPlayerList[videoIndex].isPrepared)
        {
            //Debug.Log("Preparing Index: " + videoIndex);
            yield return null;
        }
        //Debug.LogWarning("Done Preparing current Video Index: " + videoIndex);

        //Assign the Texture from Video to RawImage to be displayed
        image.texture = videoPlayerList[videoIndex].texture;

        //Play first video
        videoPlayerList[videoIndex].Play();

        //Wait while the current video is playing
        bool reachedHalfWay = false;
        int nextIndex = (videoIndex + 1);
        while (videoPlayerList[videoIndex].isPlaying)
        {
            //Debug.Log("Playing time: " + videoPlayerList[videoIndex].time + " INDEX: " + videoIndex);

            //Check if we have reached half way through
            if (!reachedHalfWay && videoPlayerList[videoIndex].time >= (question[videoIndex].time / 2))
            {
                reachedHalfWay = true; //Set to true so that we don't evaluate this again

                //Make sure that the NEXT VideoPlayer index is valid, else exit
                /*
                if (nextIndex >= videoPlayerList.Count)
                {
                    Debug.Log("End of All Videos: " + videoIndex);
                    //SceneManager.LoadScene("DrawTask2");
                    yield break;
                }
                */

                //Prepare the NEXT video
                if (nextIndex < videoPlayerList.Count)
                {
                    //Debug.Log("Ready to Prepare NEXT Video Index: " + nextIndex);
                   videoPlayerList[nextIndex].Prepare();
                }
            }

            yield return null;
        }
        //Debug.Log("Done Playing current Video Index: " + videoIndex);
        if (nextIndex >= videoPlayerList.Count)
        {
            if (gameController.interrupts_this_round == 1)
            {
                gameController.interruption_just_happened = 1;
                gameController.timer = 10;
                if (gameController.hypothesis > 4)
                {
                    if (gameController.interruption_task == 1) SceneManager.LoadScene("NoiseInterruption");
                    if (gameController.interruption_task == 2) SceneManager.LoadScene("SocialInterruption");
                }
                else if (gameController.phase == Constants.PHASE_TRAINING)
                {
                    if (gameController.task_switching == 1)
                    {
                        if (gameController.interruption_task == 1) SceneManager.LoadScene("StroopInterruption");
                        if (gameController.interruption_task == 2) SceneManager.LoadScene("AreaInterruption");
                    }
                    else
                    {
                        if (gameController.interruption_task == 1) SceneManager.LoadScene("AreaInterruption");
                        if (gameController.interruption_task == 2) SceneManager.LoadScene("StroopInterruption");
                    }
                }
                else
                {
                    if (gameController.interruption_task == 1) SceneManager.LoadScene("StroopInterruption");
                    if (gameController.interruption_task == 2) SceneManager.LoadScene("AreaInterruption");
                }
                gameController.currently_interrupting = 1;
            }
            else
            {
                gameController.interruption_just_happened = 0;
                //Debug.Log("set to zero");
                //Debug.Log("End of All Videos: " + videoIndex);
                SceneManager.LoadScene("DrawTask2");
                yield break;
            }
        }

        //Wait until NEXT video is prepared
        while (!videoPlayerList[nextIndex].isPrepared)
        {
            //Debug.Log("Preparing NEXT Video Index: " + nextIndex);
            yield return null;
        }

        //Debug.LogWarning("Done Preparing NEXT Video Index: " + videoIndex);


        //Increment Video index
        videoIndex++;

        //Play next prepared video. Pass false to it so that some codes are not executed at-all
        StartCoroutine(playVideo(false));
        //Debug.Log("END OF LOOP");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
