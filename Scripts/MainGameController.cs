using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text;
using System.IO;
//using System;



public class data
{

    public int participantAnswer, rightAnswer;
    public float timeTaken, timestamp;
    public string interruptionType, dataType, answerValue, duringPhaseNumber;


    public data(int parAns, int rAns, float t, string iType, string p, string a, float ts)
    {
        participantAnswer = parAns;
        rightAnswer = rAns;
        timeTaken = t;
        interruptionType = iType;
        duringPhaseNumber = p;
        answerValue = a;
        timestamp = ts;
        dataType = "interruption/path";
    }

    public string asd, cb, hi, adhd, prefer, none;

    public data(string a, string c, string h, string ad, string p, string n, float ts)
    {
        asd = a;
        cb = c;
        hi = h;
        adhd = ad;
        prefer = p;
        none = n;
        timestamp = ts;
        dataType = "diagnosis";
    }

    public int optimal;
    public float resumptionLag;

    public data(int parAns, int opt, float t, string p, string a, float rL, float ts)
    {
        participantAnswer = parAns;
        optimal = opt;
        timeTaken = t;
        duringPhaseNumber = p;
        answerValue = a;
        resumptionLag = rL;
        timestamp = ts;
        dataType = "hanoi";
    }

    public string age, gender, educationLevel;

    public data(string a, string g, string el, float ts)
    {
        age = a;
        gender = g;
        educationLevel = el;
        timestamp = ts;
        dataType = "demographic";
    }

    public string correctly, effort, task;

    public data(string p, string c, string e, string t, float ts)
    {
        duringPhaseNumber = p;
        correctly = c;
        effort = e;
        task = t;
        timestamp = ts;
        dataType = "cognitiveload";
    }


}

public class previousHanoiGame
{
    public int piece0_height;
    public int piece0_peg;
    public int piece1_height;
    public int piece1_peg;
    public int piece2_height;
    public int piece2_peg;
    public int piece3_height;
    public int piece3_peg;

}

public class interruptionData
{
    public int participantAnswer;
    public int rightAnswer;
    public float time;
    public string interruptionType, answerValue;
    //public int interruptionNumber;
    public int duringPhaseNumber;
    public float timestamp;


    public interruptionData(int parAns, int rAns, float t, string iType, int p, string a, float ts)
    {
        participantAnswer = parAns;
        rightAnswer = rAns;
        time = t;
        interruptionType = iType;
        duringPhaseNumber = p;
        answerValue = a;
        timestamp = ts;
    }
}


public class demographicsData
{
    public string age;
    public string gender;
    public string educationLevel;
    public float timestamp;

    public demographicsData(string a, string g, string el, float ts)
    {
        age = a;
        gender = g;
        educationLevel = el;
        timestamp = ts;
    }
}

public class CLData
{

    public string simultaneous;
    public string complex;
    public string effort;
    public string understand;
    public string exhausting;
    public string inconvenient;
    public string difficult;
    public float timestamp;

    public CLData(string s, string c, string e, string u, string ex, string i, string d, float ts)
    {
        simultaneous = s;
        complex = c;
        effort = e;
        understand = u;
        exhausting = ex;
        inconvenient = i;
        difficult = d;
        timestamp = ts;
    }
}

//[Serializable]
public class MainGameController : MonoBehaviour
{

    public string part_id = "";
    public bool dataSet = false;
    // lets assume now that we are only testing transferability on a novel task
    public List<string[]> csvRows = new List<string[]>();
    public StringBuilder sb = new StringBuilder();
    public int starting_task = 1; // if 1 then Drawing, else Hani
    public int interruption_task = 1; // if 1 then Stroop, if 2 Area
    public int task_switching = 1; // if 1 then change task, else change interruption
    public int experimental = 1; // if 0 then control


    public previousHanoiGame previousHanoi = new previousHanoiGame();
    public int interruption_just_happened = 0;

    public float timestamp = 0.0f;
    public float timer = 10;
    public int interrupts_this_round = 0;
    public int moves_to_interrupt = 999;
    public int listIndex = 0;
    public int inBreak = 0;

    public int hypothesis;
    public int counter = 0;
    int middle_break = 1;
    public int tutorial_n = 1;
    //public int tutorial_n = 7;
    public int h4_counterbalance;

    public bool endEarly = false;
    public int scene = Constants.START;
    //public int scene = Constants.SURVEY_COGNITIVELOAD;
    public int phase = Constants.PHASE_TESTING;

    //make 1 for normal sequence
    //int pretraining = 1;
    //int assessment = 0;
    //int pretraining_total = 4;
    //int pre_training_break = 1;
    int number_of_tasks = 17;
    //int number_of_tasks = 2;
    int in_break = 0;
    int answered_one = 0;

    public float inactive_time = 0;

    public List<demographicsData> demographicsResults = new List<demographicsData>();
    public List<CLData> cognitiveLoadResults = new List<CLData>();
    public List<data> allDataResults = new List <data>();

    public int currently_interrupting = 0;
    public int correct_in_row = 1;
    //public int correct_in_row_2 = 0;

    public string PHASE = "";

    public List<interruptionData> interruptionsResults = new List<interruptionData>();

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    private void Start()
    {

        /*interruption_task = Random.Range(1, 3);
        starting_task = Random.Range(1, 3);
        task_switching = Random.Range(1, 3);*/


        if (task_switching == 1) hypothesis = Constants.HYP_H1;
        if (task_switching == 2) hypothesis = Constants.HYP_H2;
        h4_counterbalance = Random.Range(1, 7); // 1NS 1SN N1S NS1 S1N SN1
    }

    public void changeScene()
    {
        if (phase == Constants.PHASE_SURVEYS)
        {
            if (scene == Constants.START)
            {
                scene = Constants.INSTRUCTIONS;
                SceneManager.LoadScene("Instructions");
                //SceneManager.LoadScene("DrawTask2");
            }
            else if (scene == Constants.INSTRUCTIONS)
            {
                scene = Constants.SURVEY_DEMOGRAPHICS;
                SceneManager.LoadScene("Demographics");

            }
            else if (scene == Constants.SURVEY_DEMOGRAPHICS)
            {
                scene = Constants.SURVEY_COGNITIVELOAD;
                //SceneManager.LoadScene("Cognitive-Load");
                SceneManager.LoadScene("Diagnosis");
                //csvStart();
                //SceneManager.LoadScene("End");
            }
            else if (scene == Constants.SURVEY_COGNITIVELOAD)
            {
                phase = Constants.PHASE_TUTORIAL;
                SceneManager.LoadScene("StroopInstruction");
                /*if (interruption_task == 1) SceneManager.LoadScene("StroopInterruption");
                else SceneManager.LoadScene("AreaInterruption");*/

            }
        }
        else if ((hypothesis == Constants.HYP_H1) || (hypothesis == Constants.HYP_H2)) changeScene_H1H2();
        else if (hypothesis == Constants.HYP_H4) changeScene_H4();
        else if ((hypothesis == Constants.HYP_H5) || (hypothesis == Constants.HYP_H6)) changeScene_H5H6();
    }

    public void changeScene_H1H2()
    {
        /*if (phase == Constants.PHASE_SURVEYS)
        {
            if (scene == Constants.START)
            {
                scene = Constants.SURVEY_DEMOGRAPHICS;
                SceneManager.LoadScene("Demographics");

            }
            else if (scene == Constants.SURVEY_DEMOGRAPHICS)
            {
                scene = Constants.SURVEY_COGNITIVELOAD;
                SceneManager.LoadScene("Cognitive-Load");

            }
            else if (scene == Constants.SURVEY_COGNITIVELOAD)
            {
                phase = Constants.PHASE_TUTORIAL;
                if (interruption_task == 1) SceneManager.LoadScene("StroopInterruption");
                else SceneManager.LoadScene("AreaInterruption");
            }
        }*/
        if (phase == Constants.PHASE_TUTORIAL)
        {
            if ((tutorial_n == 1) || (tutorial_n == 3))
            {
                if (correct_in_row > 2)
                {
                    tutorial_n += 1;
                    correct_in_row = 0;
                }
            }
            if (tutorial_n == 1)
            {
                Debug.Log("stroop");
                SceneManager.LoadScene("StroopInterruption");
                //tutorial_n = 3;
            }
            else if (tutorial_n == 2)
            {

                SceneManager.LoadScene("AreaInstructions");
                tutorial_n += 1;
            }
            else if (tutorial_n == 3)
            {

                SceneManager.LoadScene("AreaInterruption");
                //tutorial_n += 1;
            }
            else if (tutorial_n == 4)
            {

                SceneManager.LoadScene("Instructions_Hanoi");
                tutorial_n += 1;
            }
            else if (tutorial_n == 5)
            {

                SceneManager.LoadScene("HanoiTask4");
                tutorial_n += 1;
            }
            else if (tutorial_n == 6)
            {
                SceneManager.LoadScene("Instructions_Draw");
                tutorial_n += 1;
            }
            else if (tutorial_n == 7)
            {
                SceneManager.LoadScene("Instructions_General");
                tutorial_n += 1;
            }

            else if (tutorial_n == 8)
            {
                counter = 0;
                phase = Constants.PHASE_ASSESSMENT;
                currently_interrupting = 0;
            }
        }
        if (phase == Constants.PHASE_ASSESSMENT)
        {
            if (currently_interrupting == 0)
            {
                answered_one = 0;
                if (starting_task == 1) SceneManager.LoadScene("DrawTask");
                if (starting_task == 2) SceneManager.LoadScene("HanoiTask4");
                counter += 1;
            }
            else
            {
                //Debug.Log("iside assessment");
                answered_one = 1;
                if (interruption_task == 1) SceneManager.LoadScene("StroopInterruption");
                if (interruption_task == 2) SceneManager.LoadScene("AreaInterruption");
            }
            if (counter == number_of_tasks)
            {
                phase = Constants.PHASE_DIFF_ASSESS;
                counter = 0;
            }
        }
        if (phase == Constants.PHASE_DIFF_ASSESS)
        {
            if (counter == 0)
            {
                if (starting_task == 1) SceneManager.LoadScene("Difficulty-Draw");
                if (starting_task == 2) SceneManager.LoadScene("Difficulty-Hanoi");
                counter++;
            }
            else if (counter == 1)
            {
                if (interruption_task == 1) SceneManager.LoadScene("Difficulty-Stroop");
                if (interruption_task == 2) SceneManager.LoadScene("Difficulty-Math");
                counter++;
            }
            else
            {
                counter = 0;
                phase = Constants.PHASE_TRAINING;
            }


        }
        if ((experimental == 2) && (phase == Constants.PHASE_TRAINING))
        {
            if (middle_break == 1)
            {
                in_break = 1;
                middle_break = 0;
                counter = 0;
                SceneManager.LoadScene("Break");
                counter = 0;
                in_break = 0;
            }
            else if (counter >= 0 && counter < 16)
            {
                if (task_switching == 1)
                {
                    if (starting_task == 1) SceneManager.LoadScene("HanoiTask4");
                    if (starting_task == 2) SceneManager.LoadScene("DrawTask");
                }
                else
                {
                    if (starting_task == 1) SceneManager.LoadScene("DrawTask");
                    if (starting_task == 2) SceneManager.LoadScene("HanoiTask4");
                }
                counter += 1;
            }
            else if (counter > 15 && counter < 32)
            {
                Debug.Log(counter + "in main");
                currently_interrupting = 1;
                if (task_switching == 1)
                {
                    if (interruption_task == 1) SceneManager.LoadScene("StroopInterruption");
                    if (interruption_task == 2) SceneManager.LoadScene("AreaInterruption");
                }
                else
                {
                    if (interruption_task == 1) SceneManager.LoadScene("AreaInterruption");
                    if (interruption_task == 2) SceneManager.LoadScene("StroopInterruption");
                }
                answered_one = 1;
            }
            else if (counter == 32)
            {
                counter = 0;
                phase = Constants.PHASE_DIFF_TRAIN;
            }

        }
        if ((experimental == 1) && (phase == Constants.PHASE_TRAINING))
        {
            if (middle_break == 1)
            {
                in_break = 1;
                middle_break = 0;
                counter = 0;
                SceneManager.LoadScene("Break");
                //csvStart();
                //SceneManager.LoadScene("End");
            }
            else if (currently_interrupting == 0)
            {
                answered_one = 0;
                if (interruption_just_happened == 0)
                {
                    counter += 1;
                }
                in_break = 0;
                if (task_switching == 1)
                {
                    if (starting_task == 1) SceneManager.LoadScene("HanoiTask4");
                    if (starting_task == 2) SceneManager.LoadScene("DrawTask");
                }
                else
                {
                    if (starting_task == 1) SceneManager.LoadScene("DrawTask");
                    if (starting_task == 2) SceneManager.LoadScene("HanoiTask4");
                }

            }
            else
            {
                currently_interrupting = 1;
                
                if (task_switching == 1)
                {
                    if (interruption_task == 1) SceneManager.LoadScene("StroopInterruption");
                    if (interruption_task == 2) SceneManager.LoadScene("AreaInterruption");
                }
                else
                {
                    if (interruption_task == 1) SceneManager.LoadScene("AreaInterruption");
                    if (interruption_task == 2) SceneManager.LoadScene("StroopInterruption");
                }
                answered_one = 1;
            }
            if (counter == number_of_tasks)
            {
                counter = 0;
                phase = Constants.PHASE_DIFF_TRAIN;
                currently_interrupting = 0;
            }

        }
        if (phase == Constants.PHASE_DIFF_TRAIN)
        {
            if (counter == 0)
            {
                if (task_switching == 1)
                {
                    if (starting_task == 1) SceneManager.LoadScene("Difficulty-Hanoi");
                    if (starting_task == 2) SceneManager.LoadScene("Difficulty-Draw");
                }
                else
                {
                    if (starting_task == 1) SceneManager.LoadScene("Difficulty-Draw");
                    if (starting_task == 2) SceneManager.LoadScene("Difficulty-Hanoi");
                }
                counter++;
            }
            else if (counter == 1)
            {
                if (task_switching == 1)
                {
                    if (interruption_task == 1) SceneManager.LoadScene("Difficulty-Stroop");
                    if (interruption_task == 2) SceneManager.LoadScene("Difficulty-Math");
                }
                else
                {
                    if (interruption_task == 1) SceneManager.LoadScene("Difficulty-Math");
                    if (interruption_task == 2) SceneManager.LoadScene("Difficulty-Stroop");
                }
                counter++;
            }
            else
            {

                counter = 0;
                phase = Constants.PHASE_TESTING;

            }


        }
        if (phase == Constants.PHASE_TESTING)
        {
            if (currently_interrupting == 0)
            {
                answered_one = 0;
                if (starting_task == 1) SceneManager.LoadScene("DrawTask");
                if (starting_task == 2) SceneManager.LoadScene("HanoiTask4");
                counter += 1;
            }
            else
            {
                answered_one = 1;
                if (interruption_task == 1) SceneManager.LoadScene("StroopInterruption");
                if (interruption_task == 2) SceneManager.LoadScene("AreaInterruption");
            }
            if (counter == number_of_tasks)
            {
                phase = Constants.PHASE_DIFF_TEST;
                //SceneManager.LoadScene("End");
                counter = 0;
            }
        }
        if (phase == Constants.PHASE_DIFF_TEST)
        {
            if (counter == 0)
            {
                if (starting_task == 1) SceneManager.LoadScene("Difficulty-Draw");
                if (starting_task == 2) SceneManager.LoadScene("Difficulty-Hanoi");
                counter++;
            }
            else if (counter == 1)
            {
                if (interruption_task == 1) SceneManager.LoadScene("Difficulty-Stroop");
                if (interruption_task == 2) SceneManager.LoadScene("Difficulty-Math");
                counter++;
            }
            else
            {
                //phase = Constants.PHASE_END;
                //counter = 0;
                //if(dataSet)
                //{
                //  csvStart();
                //  SceneManager.LoadScene("End");
                  phase = Constants.PHASE_END;
                  counter = 0;
                //}
            }

        }
    }

    public void changeScene_H4()
    {
        /*if (phase == Constants.PHASE_SURVEYS)
        {
            if (scene == Constants.START)
            {
                scene = Constants.SURVEY_DEMOGRAPHICS;
                SceneManager.LoadScene("Demographics");

            }
            else if (scene == Constants.SURVEY_DEMOGRAPHICS)
            {
                scene = Constants.SURVEY_COGNITIVELOAD;
                SceneManager.LoadScene("Cognitive-Load");

            }
            else if (scene == Constants.SURVEY_COGNITIVELOAD)
            {
                phase = Constants.PHASE_TUTORIAL;
                SceneManager.LoadScene("StroopInterruption");
            }
        }*/
        if (phase == Constants.PHASE_TUTORIAL)
        {
            if (tutorial_n < 3)
            {
                if (correct_in_row > 2)
                {
                    tutorial_n += 1;
                    correct_in_row = 0;
                }
            }

            if (tutorial_n == 1) SceneManager.LoadScene("StroopInterruption");
            else if (tutorial_n == 2) SceneManager.LoadScene("NoiseInterruption");
            else if (tutorial_n == 3) SceneManager.LoadScene("SocialInterruption");
            else if (tutorial_n == 4)
            {
                if (starting_task == 1) SceneManager.LoadScene("DrawTask");
                else if (starting_task == 1) SceneManager.LoadScene("HanoiTask4");
            }

            else if (tutorial_n == 5)
            {
                phase = Constants.PHASE_ASSESSMENT;
                counter = 0;
            }
            tutorial_n++;
        }

        if (phase == Constants.PHASE_ASSESSMENT)
        {
            if (currently_interrupting == 0)
            {
                counter += 1;
                if (starting_task == 1) SceneManager.LoadScene("DrawTask");
                if (starting_task == 2) SceneManager.LoadScene("HanoiTask4");

            }
            else
            { // 1NS 1SN N1S NS1 S1N SN1
                if (counter < 3)
                {
                    Debug.Log("Counter is less than 3");
                    if (h4_counterbalance == 1) SceneManager.LoadScene("StroopInterruption");
                    if (h4_counterbalance == 2) SceneManager.LoadScene("StroopInterruption");
                    if (h4_counterbalance == 3) SceneManager.LoadScene("NoiseInterruption");
                    if (h4_counterbalance == 4) SceneManager.LoadScene("NoiseInterruption");
                    if (h4_counterbalance == 5) SceneManager.LoadScene("SocialInterruption");
                    if (h4_counterbalance == 6) SceneManager.LoadScene("SocialInterruption");
                }
                else if (counter < 6)
                {
                    Debug.Log("Counter is less than 6");
                    if (h4_counterbalance == 1) SceneManager.LoadScene("NoiseInterruption");
                    if (h4_counterbalance == 2) SceneManager.LoadScene("SocialInterruption");
                    if (h4_counterbalance == 3) SceneManager.LoadScene("StroopInterruption");
                    if (h4_counterbalance == 4) SceneManager.LoadScene("SocialInterruption");
                    if (h4_counterbalance == 5) SceneManager.LoadScene("StroopInterruption");
                    if (h4_counterbalance == 6) SceneManager.LoadScene("NoiseInterruption");
                }
                else if (counter < 9)
                {
                    Debug.Log("Counter is less than 9");
                    if (h4_counterbalance == 1) SceneManager.LoadScene("SocialInterruption");
                    if (h4_counterbalance == 2) SceneManager.LoadScene("NoiseInterruption");
                    if (h4_counterbalance == 3) SceneManager.LoadScene("SocialInterruption");
                    if (h4_counterbalance == 4) SceneManager.LoadScene("StroopInterruption");
                    if (h4_counterbalance == 5) SceneManager.LoadScene("NoiseInterruption");
                    if (h4_counterbalance == 6) SceneManager.LoadScene("StroopInterruption");
                }
            }

            if (counter == 9)
            {
              //if(dataSet)
              //{
              //  csvStart();
              //  SceneManager.LoadScene("End");
                phase = Constants.PHASE_END;
                counter = 0;
              //}
              //  phase = Constants.PHASE_END;
              //  csvStart();
              //  SceneManager.LoadScene("End");
            }
        }
    }

    public void changeScene_H5H6()
    {
        /*if (phase == Constants.PHASE_SURVEYS)
        {
            if (scene == Constants.START)
            {
                scene = Constants.SURVEY_DEMOGRAPHICS;
                SceneManager.LoadScene("Demographics");

            }
            else if (scene == Constants.SURVEY_DEMOGRAPHICS)
            {
                scene = Constants.SURVEY_COGNITIVELOAD;
                SceneManager.LoadScene("Cognitive-Load");

            }
            else if (scene == Constants.SURVEY_COGNITIVELOAD)
            {
                phase = Constants.PHASE_TUTORIAL;
            }
        }*/
        if (phase == Constants.PHASE_TUTORIAL)
        {
            if (tutorial_n < 3)
            {
                if (correct_in_row > 2)
                {
                    tutorial_n += 1;
                    correct_in_row = 0;
                }
            }

            if (tutorial_n == 1)
            {
                if (interruption_task == 1) SceneManager.LoadScene("NoiseInterruption");
                if (interruption_task == 2) SceneManager.LoadScene("SocialInterruption");
                tutorial_n += 1;
            }
            else if (tutorial_n == 2)
            {
                if (starting_task == 1) SceneManager.LoadScene("Instructions_Hanoi");
                if (starting_task == 2) SceneManager.LoadScene("Instructions_Draw");
                tutorial_n += 1;
            }
            else if (tutorial_n == 3)
            {
                if (starting_task == 1) SceneManager.LoadScene("Instructions_Draw");
                if (starting_task == 2) SceneManager.LoadScene("Instructions_Hanoi");
                tutorial_n += 1;
            }
            else if (tutorial_n == 4)
            {
                counter = 0;
                phase = Constants.PHASE_ASSESSMENT;
            }
        }
        if (phase == Constants.PHASE_ASSESSMENT)
        {
            if (currently_interrupting == 0)
            {
                if (starting_task == 1) SceneManager.LoadScene("DrawTask");
                if (starting_task == 2) SceneManager.LoadScene("HanoiTask4");
                counter += 1;
            }
            else
            {
                if (interruption_task == 1) SceneManager.LoadScene("NoiseInterruption");
                if (interruption_task == 2) SceneManager.LoadScene("SocialInterruption");
            }

            if (counter == 4)
            {
                phase = Constants.PHASE_TRAINING;
                counter = 0;
            }

        }
        if (phase == Constants.PHASE_TRAINING)
        {
            if (middle_break == 1)
            {
                in_break = 1;
                middle_break = 0;
                counter = 0;
                SceneManager.LoadScene("Break");
            }
            else if (currently_interrupting == 0)
            {
                in_break = 0;
                Debug.Log("choosing task from h56");
                if (starting_task == 1) SceneManager.LoadScene("HanoiTask4");
                if (starting_task == 2) SceneManager.LoadScene("DrawTask");

                if (interruption_just_happened == 0)
                {
                    counter += 1;
                }
            }
            else
            {
                if (interruption_task == 1) SceneManager.LoadScene("NoiseInterruption");
                if (interruption_task == 2) SceneManager.LoadScene("SocialInterruption");
            }
            if (counter == 4)
            {
                counter = 0;
                phase = Constants.PHASE_TESTING;

            }

        }
        if (phase == Constants.PHASE_TESTING)
        {
            if (currently_interrupting == 0)
            {
                counter += 1;
                if (starting_task == 1) SceneManager.LoadScene("DrawTask");
                if (starting_task == 2) SceneManager.LoadScene("HanoiTask4");

            }
            else
            {
                if (interruption_task == 1) SceneManager.LoadScene("NoiseInterruption");
                if (interruption_task == 2) SceneManager.LoadScene("SocialInterruption");
            }

            if (counter == 4)
            {
              //if(dataSet)
              //{
                //csvStart();
                //SceneManager.LoadScene("End");
                phase = Constants.PHASE_END;
                counter = 0;
              //}
              //  phase = Constants.PHASE_END;
              //  csvStart();
              //  SceneManager.LoadScene("End");
            }
        }
    }


    public void csvStart()
    {
        //string[] csvRowData = new string[21];

        /*
        if(endEarly)
        {
          csvRows.Add(new string[1]{"------------------------   ENDED EARLY   ------------------------"});
          csvRows.Add(new string[1]{"newline"});
        }
        */
        Debug.Log("IN CSV START");
        string[] firstHeader = new string[4] {"UID", "Interruption Task", "Starting Task", "Task Switching"};

        csvRows.Add(firstHeader);

        string[] firstEntry = new string[4];

        firstEntry[0] = "1";
        firstEntry[1] = interruption_task.ToString();
        firstEntry[2] = starting_task.ToString();
        firstEntry[3] = task_switching.ToString();

        csvRows.Add(firstEntry);

        csvRows.Add(new string[1]{"newline"});

        //Debug.Log(allDataResults.Count);

        for(int i =0; i<allDataResults.Count;i++)
        {
            //Debug.Log(i);
            Debug.Log(allDataResults[i].dataType);
            if(allDataResults[i].dataType=="demographic")
            {

                string[] demHeader = new string[7] {"Phase", "Type", "Task","Age", "Gender", "Education Level", "Time"};
                csvRows.Add(demHeader);
                string[] demographicEntry = new string[7];

                demographicEntry[0] = "SURVEYS";
                demographicEntry[1] = "SURVEY";
                demographicEntry[2] = "DEMOGRAPHICS";
                demographicEntry[3] = allDataResults[i].age.ToString();
                demographicEntry[4] = allDataResults[i].gender;
                demographicEntry[5] = allDataResults[i].educationLevel;
                demographicEntry[6] = allDataResults[i].timestamp.ToString();

                csvRows.Add(demographicEntry);
            }
            else if(allDataResults[i].dataType=="diagnosis")
            {
                Debug.Log("in diagnosis");
                csvRows.Add(new string[1]{"newline"});
                string[] diagHeader = new string[10] {"Phase", "Type", "Task","ASD", "Color Blind","Hearing Impaired", "ADHD", "Prefer Not to Say", "None", "Time"};
                csvRows.Add(diagHeader);
                string[] diagEntry = new string[10];

                diagEntry[0] = "SURVEYS";
                diagEntry[1] = "SURVEY";
                diagEntry[2] = "DIAGNOSIS";
                diagEntry[3] = allDataResults[i].asd.Trim('\n');
                diagEntry[4] = allDataResults[i].cb.Trim('\n');
                diagEntry[5] = allDataResults[i].hi.Trim('\n');
                diagEntry[6] = allDataResults[i].adhd.Trim('\n');
                diagEntry[7] = allDataResults[i].prefer.Trim('\n');
                diagEntry[8] = allDataResults[i].none.Trim('\n');
                diagEntry[9] = allDataResults[i].timestamp.ToString();

                csvRows.Add(diagEntry);
            }
            else if(allDataResults[i].dataType=="cognitiveload")
            {

                csvRows.Add(new string[1]{"newline"});

                string[] clHeader = new string[6] {"Phase", "Type", "Task", "Effort Invested", "Confident Correct", "Time"};
                csvRows.Add(clHeader);
                string[] clEntry = new string[6];

                clEntry[0] = allDataResults[i].duringPhaseNumber;
                clEntry[1] = "SURVEY";
                clEntry[2] = allDataResults[i].task.Trim('\n');
                clEntry[3] = allDataResults[i].effort.Trim('\n');
                clEntry[4] = allDataResults[i].correctly.Trim('\n');
                clEntry[5] = allDataResults[i].timestamp.ToString();

                csvRows.Add(clEntry);

                //csvRows.Add(new string[1]{"newline"});
                //csvRows.Add(new string[1]{"newline"});
                //string[] interHeader = new string[9] {"Phase", "Type", "Task", "Participant Answer", "Right Answer", "Correct/Wrong", "Time Taken","Resumption Lag", "Time"};
                //csvRows.Add(interHeader);
            }
            else if(allDataResults[i].dataType=="interruption/path")
            {

                //csvRows.Add(new string[1]{"newline"});
                //string[] interHeader = new string[8] {"Phase", "Type", "Task", "Participant Answer", "Right Answer", "Correct/Wrong", "Time Taken","Time"};
                //csvRows.Add(interHeader);
                string[] interEntry = new string[9];

                string typeOfTask = "INTERRUPTION";
                if(allDataResults[i].interruptionType=="path")
                {
                    typeOfTask = "PRIMARY";
                }

                interEntry[0] = allDataResults[i].duringPhaseNumber;
                interEntry[1] = typeOfTask;
                interEntry[2] = allDataResults[i].interruptionType;
                interEntry[3] = allDataResults[i].participantAnswer.ToString();
                interEntry[4] = allDataResults[i].rightAnswer.ToString();
                interEntry[5] = allDataResults[i].answerValue;
                interEntry[6] = allDataResults[i].timeTaken.ToString();
                interEntry[7] = "N/A";
                interEntry[8] = allDataResults[i].timestamp.ToString();

                csvRows.Add(interEntry);

            }
            else if(allDataResults[i].dataType=="hanoi")
            {

                //csvRows.Add(new string[1]{"newline"});
                //string[] hanoiHeader = new string[9] {"Phase", "Type", "Task", "Participant Moves", "Optimal Moves", "Correct/Wrong", "Time Taken", "Resumption Lag", "Time"};
                //csvRows.Add(hanoiHeader);
                string status = "incomplete";
                if(allDataResults[i].answerValue.ToString()=="--0123")
                {
                    status = "complete";
                }
                string[] hanoiEntry = new string[9];

                hanoiEntry[0] = allDataResults[i].duringPhaseNumber;
                hanoiEntry[1] = "PRIMARY";
                hanoiEntry[2] = "HANOI";
                hanoiEntry[3] = allDataResults[i].answerValue.ToString();
                hanoiEntry[4] = "--0123";
                hanoiEntry[5] = status;
                hanoiEntry[6] = allDataResults[i].timeTaken.ToString();
                hanoiEntry[7] = allDataResults[i].resumptionLag.ToString();
                hanoiEntry[8] = allDataResults[i].timestamp.ToString();

                csvRows.Add(hanoiEntry);

            }

        }

        //StringBuilder sb = new StringBuilder();

        for(int i =0; i<csvRows.Count; i++)
        {
            if(csvRows[i][0]=="newline"){
                sb.AppendLine();
            }
            else
            {
                sb.AppendLine(string.Join(",", csvRows[i]));
            }
        }

        //string filePath = Application.dataPath + "/CSV/TestData.csv";

        //Debug.Log(filePath);

        //StreamWriter outStream = new StreamWriter(filePath);

        //outStream.WriteLine(sb);

        //outStream.Flush();
        //outStream.Close();

        //Debug.Log("GOING TO END");
        SceneManager.LoadScene("End");
    }

    // Update is called once per frame
    void Update()
    {
        timestamp += Time.deltaTime;
        if (phase == Constants.PHASE_SURVEYS) PHASE = "SURVEYS";
        if (phase == Constants.PHASE_TUTORIAL) PHASE = "TUTORIAL";
        if (phase == Constants.PHASE_ASSESSMENT) PHASE = "ASSESSMENT";
        if (phase == Constants.PHASE_TRAINING) PHASE = "TRAINING";
        if (phase == Constants.PHASE_TESTING) PHASE = "TESTING";
        if (phase == Constants.PHASE_END) PHASE = "END";

        if(phase==Constants.PHASE_END)
        {
          if(dataSet)
          {
            csvStart();
            SceneManager.LoadScene("End");
            dataSet=false;
          }
        }

        // check if it should interrupt
        if (((phase == Constants.PHASE_TRAINING) || (phase == Constants.PHASE_TESTING) || (phase == Constants.PHASE_ASSESSMENT)) && (in_break == 0))
        {

            if ((currently_interrupting == 1))
            {
                timer -= Time.deltaTime;
            }
            if ((moves_to_interrupt == 0) && (currently_interrupting == 0))
            {
                interruption_just_happened = 1;
                timer = 10;
                currently_interrupting = 1;
                if (phase == Constants.PHASE_TRAINING)
                {
                    if (hypothesis < 5)
                    {
                        if (task_switching == 1)
                        {
                            if (interruption_task == 1) SceneManager.LoadScene("StroopInterruption");
                            if (interruption_task == 2) SceneManager.LoadScene("AreaInterruption");
                        }
                        else
                        {
                            if (interruption_task == 1) SceneManager.LoadScene("AreaInterruption");
                            if (interruption_task == 2) SceneManager.LoadScene("StroopInterruption");
                        }
                    }
                    else
                    {
                        if (interruption_task == 1) SceneManager.LoadScene("NoiseInterruption");
                        if (interruption_task == 2) SceneManager.LoadScene("SocialInterruption");
                    }
                }
                else
                {
                    if (hypothesis == 4)
                    {
                        if (counter < 3)
                        {
                            Debug.Log("Counter is less than 3");
                            if (h4_counterbalance == 1) SceneManager.LoadScene("StroopInterruption");
                            if (h4_counterbalance == 2) SceneManager.LoadScene("StroopInterruption");
                            if (h4_counterbalance == 3) SceneManager.LoadScene("NoiseInterruption");
                            if (h4_counterbalance == 4) SceneManager.LoadScene("NoiseInterruption");
                            if (h4_counterbalance == 5) SceneManager.LoadScene("SocialInterruption");
                            if (h4_counterbalance == 6) SceneManager.LoadScene("SocialInterruption");
                        }
                        else if (counter < 6)
                        {
                            Debug.Log("Counter is less than 6");
                            if (h4_counterbalance == 1) SceneManager.LoadScene("NoiseInterruption");
                            if (h4_counterbalance == 2) SceneManager.LoadScene("SocialInterruption");
                            if (h4_counterbalance == 3) SceneManager.LoadScene("StroopInterruption");
                            if (h4_counterbalance == 4) SceneManager.LoadScene("SocialInterruption");
                            if (h4_counterbalance == 5) SceneManager.LoadScene("StroopInterruption");
                            if (h4_counterbalance == 6) SceneManager.LoadScene("NoiseInterruption");
                        }
                        else if (counter < 9)
                        {
                            Debug.Log("Counter is less than 9");
                            if (h4_counterbalance == 1) SceneManager.LoadScene("SocialInterruption");
                            if (h4_counterbalance == 2) SceneManager.LoadScene("NoiseInterruption");
                            if (h4_counterbalance == 3) SceneManager.LoadScene("SocialInterruption");
                            if (h4_counterbalance == 4) SceneManager.LoadScene("StroopInterruption");
                            if (h4_counterbalance == 5) SceneManager.LoadScene("NoiseInterruption");
                            if (h4_counterbalance == 6) SceneManager.LoadScene("StroopInterruption");
                        }
                    }
                    else if (hypothesis > 4)
                    {
                        if (interruption_task == 1) SceneManager.LoadScene("NoiseInterruption");
                        if (interruption_task == 2) SceneManager.LoadScene("SocialInterruption");
                    }
                    else if (interruption_task == 1) SceneManager.LoadScene("StroopInterruption");
                    else if (interruption_task == 2) SceneManager.LoadScene("AreaInterruption");
                }
            }
            if (timer <= 0 && answered_one == 1)
            {
                currently_interrupting = 0;
                timer = 10;
                if (phase == Constants.PHASE_TRAINING)
                {
                    if (experimental == 1)
                    {
                        if (hypothesis > 4)
                        {
                            if (starting_task == 1) SceneManager.LoadScene("HanoiTask4");
                            if (starting_task == 2) SceneManager.LoadScene("DrawTask2");
                        }
                        else if (task_switching == 1)
                        {
                            if (starting_task == 1) SceneManager.LoadScene("HanoiTask4");
                            if (starting_task == 2) SceneManager.LoadScene("DrawTask2");
                        }
                        else
                        {
                            if (starting_task == 1) SceneManager.LoadScene("DrawTask2");
                            if (starting_task == 2) SceneManager.LoadScene("HanoiTask4");
                        }
                    }
                    else
                    {
                        if (answered_one == 1)
                        {
                            answered_one = 0;
                            counter += 1;
                            timer = 10;
                            currently_interrupting = 1;
                            if (counter > 16 && counter < 32)
                            {
                                Debug.Log(counter + "in timer");
                                if (task_switching == 1)
                                {
                                    if (interruption_task == 1) SceneManager.LoadScene("StroopInterruption");
                                    if (interruption_task == 2) SceneManager.LoadScene("AreaInterruption");
                                }
                                else
                                {
                                    if (interruption_task == 1) SceneManager.LoadScene("AreaInterruption");
                                    if (interruption_task == 2) SceneManager.LoadScene("StroopInterruption");
                                }
                            }
                            else if (counter == 32)
                            {
                                counter = 0;
                                phase = Constants.PHASE_DIFF_TRAIN;
                                currently_interrupting = 0;
                            }
                            //answered_one = 1;
                        }
                    }
                }
                if (phase == Constants.PHASE_TESTING || phase == Constants.PHASE_ASSESSMENT)
                {
                    if (starting_task == 1) SceneManager.LoadScene("DrawTask2");
                    if (starting_task == 2) SceneManager.LoadScene("HanoiTask4");
                    /*if (phase == Constants.PHASE_ASSESSMENT)
                    {
                        if (counter == 4) SceneManager.LoadScene("End");
                    }*/
                }
            }
        }

        // check if the person has been inactive for more than 2 minutes

        if (phase != Constants.PHASE_END)
        {
            if (!Input.anyKey)
            {
                if (in_break == 0)
                {
                    //Starts counting when no button is being pressed
                    inactive_time += Time.deltaTime;
                }
            }
            else
            {

                // If a button is being pressed restart counter to Zero
                inactive_time = 0;
            }

            //warning
            if (inactive_time > 60)
            {
                //Now you could set time too zero so this happens every 100 frames
                //SceneManager.LoadScene("End");
            }


            //end game, no pay -- 120
            if (inactive_time > 120)
            {
                //Debug.Log("INACTIVE");

                //Now you could set time too zero so this happens every 100 frames
                phase = Constants.PHASE_END;
                endEarly = true;
                csvStart();
                SceneManager.LoadScene("End_nopay");
            }
        }
    }
}
