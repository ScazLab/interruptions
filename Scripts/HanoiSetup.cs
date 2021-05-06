using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HanoiSetup : MonoBehaviour
{
    // Rebecca (07/25/2020)
    // - added overall sequence tracking
    // - see CSV/hanoi-questions.CSV to set this task's question sequence
    // - note: n_blocks can be inferred from state.start.orientation.Count

    private MainGameController gameController;
    private SequenceReader.HanoiQuestion state = SequenceReader.hanoiSequence[SequenceReader.hanoiSequenceIndex];

    //public IList<int> height = new List<int>() { -50, -25, 0, 25, 50 };
    public IList<int> height = new List<int>() { -100, -50, 0, 50, 100 };
    public IList<int> peg = new List<int>() { -250, 0, 250 };

    int[,] occupied = new int[5, 3];

    int[,] goal = new int[5, 3];

    public int number_pieces = 2;
    public Transform panel;

    public GameObject block0;
    public GameObject block1;
    public GameObject block2;
    public GameObject block3;
    public GameObject block4;

    GameObject feedback_correct;
    GameObject feedback_incorrect;

    public int skip = 0;

    public int n_blocks;

    float timeTaken = 0.0f;

    void Start()
    {
        gameController = GameObject.Find("MainGameController").GetComponent<MainGameController>();

        feedback_correct = GameObject.Find("Correct");
        feedback_incorrect = GameObject.Find("Incorrect");
        feedback_correct.SetActive(false);
        feedback_incorrect.SetActive(false);

        if (gameController.interruption_just_happened == 0)
        {
            gameController.moves_to_interrupt = state.moves_to_interrupt;

            for (int i = 0; i < n_blocks; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    occupied[i, j] = -1;
                }
            }

            gameController.previousHanoi.piece0_height = state.start.orientation[3].height;
            gameController.previousHanoi.piece0_peg = state.start.orientation[3].peg;
            gameController.previousHanoi.piece1_height = state.start.orientation[2].height;
            gameController.previousHanoi.piece1_peg = state.start.orientation[2].peg;
            gameController.previousHanoi.piece2_height = state.start.orientation[1].height;
            gameController.previousHanoi.piece2_peg = state.start.orientation[1].peg;
            gameController.previousHanoi.piece3_height = state.start.orientation[0].height;
            gameController.previousHanoi.piece3_peg = state.start.orientation[0].peg;

            if (n_blocks == 4)
            {
                setLocations4(
                    state.start.orientation[3].height, state.start.orientation[3].peg, // smallest block: height, peg
                    state.start.orientation[2].height, state.start.orientation[2].peg,
                    state.start.orientation[1].height, state.start.orientation[1].peg,
                    state.start.orientation[0].height, state.start.orientation[0].peg  // largest block: height peg
                );
                setGoal4(
                    state.goal.orientation[3].height, state.goal.orientation[3].peg,   // smallest block: height, peg
                    state.goal.orientation[2].height, state.goal.orientation[2].peg,
                    state.goal.orientation[1].height, state.goal.orientation[1].peg,
                    state.goal.orientation[0].height, state.goal.orientation[0].peg    // largest block: height peg
                );
            }
            else if (n_blocks == 5)
            {
                setLocations5(
                    state.start.orientation[4].height, state.start.orientation[4].peg, // smallest block: height, peg
                    state.start.orientation[3].height, state.start.orientation[3].peg,
                    state.start.orientation[2].height, state.start.orientation[2].peg,
                    state.start.orientation[1].height, state.start.orientation[1].peg,
                    state.start.orientation[0].height, state.start.orientation[0].peg  // largest block: height peg
                );
                setGoal5(
                    state.goal.orientation[4].height, state.goal.orientation[4].peg,   // smallest block: height, peg
                    state.goal.orientation[3].height, state.goal.orientation[3].peg,
                    state.goal.orientation[2].height, state.goal.orientation[2].peg,
                    state.goal.orientation[1].height, state.goal.orientation[1].peg,
                    state.goal.orientation[0].height, state.goal.orientation[0].peg    // largest block: height peg
                );
            }
            else { /*Debug.LogError("A puzzle with " + n_blocks + " is currently not supported.");*/ }

            SequenceReader.hanoiSequenceIndex += 1; // mark current puzzle as completed
        }
        else
        {
            gameController.moves_to_interrupt = 999;
            gameController.interruption_just_happened = 0;
            for (int i = 0; i < n_blocks; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    occupied[i, j] = -1;
                }
            }

            if (n_blocks == 4)
            {
                setLocations4(
                    gameController.previousHanoi.piece0_height, gameController.previousHanoi.piece0_peg, // smallest block: height, peg
                    gameController.previousHanoi.piece1_height, gameController.previousHanoi.piece1_peg,
                    gameController.previousHanoi.piece2_height, gameController.previousHanoi.piece2_peg,
                    gameController.previousHanoi.piece3_height, gameController.previousHanoi.piece3_peg  // largest block: height peg
                );
                setGoal4(
                    state.goal.orientation[3].height, state.goal.orientation[3].peg,   // smallest block: height, peg
                    state.goal.orientation[2].height, state.goal.orientation[2].peg,
                    state.goal.orientation[1].height, state.goal.orientation[1].peg,
                    state.goal.orientation[0].height, state.goal.orientation[0].peg    // largest block: height peg
                );
            }
        }
    }

    void Update()
    {
        timeTaken+=Time.deltaTime;
        if (skip == 1)
        {
            gameController.changeScene();
        }
    }

    void setLocations5(int r_0, int p_0, int r_1, int p_1, int r_2, int p_2, int r_3, int p_3, int r_4, int p_4)
    {
        GameObject b0 = Instantiate(block0, new Vector3(peg[p_0], height[r_0], 0f), Quaternion.identity);
        GameObject b1 = Instantiate(block1, new Vector3(peg[p_1], height[r_1], 0f), Quaternion.identity);
        GameObject b2 = Instantiate(block2, new Vector3(peg[p_2], height[r_2], 0f), Quaternion.identity);
        GameObject b3 = Instantiate(block3, new Vector3(peg[p_3], height[r_3], 0f), Quaternion.identity);
        GameObject b4 = Instantiate(block4, new Vector3(peg[p_4], height[r_4], 0f), Quaternion.identity);

        b0.transform.SetParent(panel.transform, false);
        b1.transform.SetParent(panel.transform, false);
        b2.transform.SetParent(panel.transform, false);
        b3.transform.SetParent(panel.transform, false);
        b4.transform.SetParent(panel.transform, false);

        b0.GetComponent<HanoiPiece>().setLocation(0, p_0, r_0);
        b1.GetComponent<HanoiPiece>().setLocation(1, p_1, r_1);
        b2.GetComponent<HanoiPiece>().setLocation(2, p_2, r_2);
        b3.GetComponent<HanoiPiece>().setLocation(3, p_3, r_3);
        b4.GetComponent<HanoiPiece>().setLocation(4, p_4, r_4);

        occupied[r_0, p_0] = 0;
        occupied[r_1, p_1] = 1;
        occupied[r_2, p_2] = 2;
        occupied[r_3, p_3] = 3;
        occupied[r_4, p_4] = 4;
    }

    void setGoal5(int r_0, int p_0, int r_1, int p_1, int r_2, int p_2, int r_3, int p_3, int r_4, int p_4)
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                goal[i, j] = -1;
            }
        }

        goal[r_0, p_0] = 0;
        goal[r_1, p_1] = 1;
        goal[r_2, p_2] = 2;
        goal[r_3, p_3] = 3;
        goal[r_4, p_4] = 4;
    }

    void setLocations4(int r_0, int p_0, int r_1, int p_1, int r_2, int p_2, int r_3, int p_3)
    {
        GameObject b0 = Instantiate(block0, new Vector3(peg[p_0], height[r_0], 0f), Quaternion.identity);
        GameObject b1 = Instantiate(block1, new Vector3(peg[p_1], height[r_1], 0f), Quaternion.identity);
        GameObject b2 = Instantiate(block2, new Vector3(peg[p_2], height[r_2], 0f), Quaternion.identity);
        GameObject b3 = Instantiate(block3, new Vector3(peg[p_3], height[r_3], 0f), Quaternion.identity);

        b0.transform.SetParent(panel.transform, false);
        b1.transform.SetParent(panel.transform, false);
        b2.transform.SetParent(panel.transform, false);
        b3.transform.SetParent(panel.transform, false);

        b0.GetComponent<HanoiPiece>().setLocation(0, p_0, r_0);
        b1.GetComponent<HanoiPiece>().setLocation(1, p_1, r_1);
        b2.GetComponent<HanoiPiece>().setLocation(2, p_2, r_2);
        b3.GetComponent<HanoiPiece>().setLocation(3, p_3, r_3);

        occupied[r_0, p_0] = 0;
        occupied[r_1, p_1] = 1;
        occupied[r_2, p_2] = 2;
        occupied[r_3, p_3] = 3;
    }

    void setGoal4(int r_0, int p_0, int r_1, int p_1, int r_2, int p_2, int r_3, int p_3)
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                goal[i, j] = -1;
            }
        }

        goal[r_0, p_0] = 0;
        goal[r_1, p_1] = 1;
        goal[r_2, p_2] = 2;
        goal[r_3, p_3] = 3;
    }

    IEnumerator delayTransition(){
        Debug.Log(Time.time);
        yield return new WaitForSeconds(5);
        Debug.Log(Time.time);
    }

    void setFeedback(bool status){
        if (status == true){
            feedback_correct.SetActive(true);
            feedback_incorrect.SetActive(false);
        }
        else {
            feedback_incorrect.SetActive(true);
            feedback_correct.SetActive(false);
        }
        StartCoroutine(delayTransition()); 
    }

    public void checkGoal()
    {
        string configuration = "";
        int ok = 1;
        //Debug.Log("checking current state...");
        for (int i = 0; i < n_blocks ; i++)
        {
            //if(i>0)
            //{
            //    configuration += "-";
            //}
            for (int j = 0; j < 3; j++)
            {
                if (goal[i, j] != occupied[i,j])
                {
                    ok = 0;
                }
                //if(occupied[3-j,i]!=-1)
                //{
                //  configuration+=occupied[3-j,i].ToString();
                //}
            }
        }

        for(int i = 0; i < 3; i++)
        {
          if(i>0)
          {
              configuration += "-";
          }
          for(int j=n_blocks-1; j >= 0; j--)
          {
            if(occupied[j,i]!=-1)
            {
              configuration += occupied[j,i].ToString();
            }
          }
          //Debug.Log(configuration);
        }

        if (gameController.PHASE == "TUTORIAL"){
            setFeedback(ok == 1);
        }

        if (ok == 0)
        {
          data pathData = new data(0, 0,timeTaken, gameController.PHASE, configuration, 0.0f, gameController.timestamp);
          gameController.allDataResults.Add(pathData);
          timeTaken=0.0f;
        }
        if (ok == 1)
        {
            data pathData = new data(0, 0,timeTaken, gameController.PHASE, configuration, 0.0f, gameController.timestamp);
            gameController.allDataResults.Add(pathData);
            //Debug.Log("GOAL REACHED");
            timeTaken=0.0f;
            gameController.changeScene();
        }
    }


    public int spaceToOccupy(int column, HanoiPiece hp)
    {
        int size = hp.pieceSize;
        for (int i = 0; i < n_blocks; i++)
        {
            if (occupied[i, column] == -1)
            {
                if (i == 0) // is empty so can occupy lowest spot
                {
                    updateLocations(hp, size, i, column, hp.height, hp.peg);
                    return i;
                }
                else if (occupied[i - 1, column] > size)
                {
                    updateLocations(hp, size, i, column, hp.height, hp.peg);
                    return i;
                }
                else
                {
                    return -1;
                }
            }
        }
        return -1;
    }

    public void updateLocations(HanoiPiece hp, int size, int new_row, int new_column, int old_row, int old_column)
    {
        // new locations
        occupied[new_row, new_column] = size;
        hp.peg = new_column;
        hp.height = new_row;

        //clear old location
        occupied[old_row, old_column] = -1;

        if (size == 0)
        {
            gameController.previousHanoi.piece0_height = new_row;
            gameController.previousHanoi.piece0_peg = new_column;
        }
        if (size == 1)
        {
            gameController.previousHanoi.piece1_height = new_row;
            gameController.previousHanoi.piece1_peg = new_column;
        }
        if (size == 2)
        {
            gameController.previousHanoi.piece2_height = new_row;
            gameController.previousHanoi.piece2_peg = new_column;
        }
        if (size == 3)
        {
            gameController.previousHanoi.piece3_height = new_row;
            gameController.previousHanoi.piece3_peg = new_column;
        }
    }

    public int isOnTop(int row, int column)
    {
        if (row == 3 & n_blocks == 4)
        {
            return 1;
        }
        else if (row == 4 & n_blocks == 5)
        {
            return 1;
        }
        else if (occupied[row+1,column] == -1)
        {

            return 1;
        }
        else
        {
            return 0;
        }
    }

}
