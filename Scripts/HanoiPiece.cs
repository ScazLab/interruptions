using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HanoiPiece : MonoBehaviour
{
    public int pieceSize; //0 is smallest, 4 is biggest
    public int peg; //0 is left, 2 is right
    public int height; //0 is lowest, 4 is the highest

    public int being_dragged = 0;
    public int is_on_top = 0;

    public HanoiPiece(int size, int peg, int height)
    {
        this.pieceSize = size;
        this.peg = peg;
        this.height = height;
    }

    public void setLocation(int size, int peg, int height)
    {
        this.pieceSize = size;
        this.peg = peg;
        this.height = height;
    }
}