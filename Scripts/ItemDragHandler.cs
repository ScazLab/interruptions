using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public HanoiSetup hanoiSetup;
    private Vector3 offset;
    private Vector3 screenPoint;
    private Vector3 originalPosition;
    IList<int> height = new List<int>() { -100, -50, 0, 50, 100 };
    IList<int> peg = new List<int>() { -250, 0, 250 };
    private MainGameController gameController;

    public void Start()
    {
        hanoiSetup = GameObject.Find("HanoiLogic").GetComponent<HanoiSetup>();
        gameController = GameObject.Find("MainGameController").GetComponent<MainGameController>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (hanoiSetup.isOnTop(gameObject.GetComponent<HanoiPiece>().height, gameObject.GetComponent<HanoiPiece>().peg) == 1)
        {
            originalPosition = gameObject.transform.position;
            gameObject.GetComponent<HanoiPiece>().being_dragged = 1;
            screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (gameObject.GetComponent<HanoiPiece>().being_dragged == 1)
        {
            Vector3 cursorPoint = new Vector3((int)Input.mousePosition.x, (int)Input.mousePosition.y, (int)screenPoint.z);
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
            transform.position = new Vector3((int)cursorPosition.x, (int)cursorPosition.y, (int)cursorPosition.z);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        int valid_drag = 0;
        if (gameObject.GetComponent<HanoiPiece>().being_dragged == 1)
        {
            //GameObject panel = GameObject.Find("Panel");
            GameObject canvas = GameObject.Find("Canvas-bg");
            RectTransform CanvasRect = canvas.GetComponent<RectTransform>();
            Vector3 pos = Camera.main.WorldToViewportPoint(gameObject.transform.position);
            Vector2 screenpos = new Vector2(((pos.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),((pos.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));
            gameObject.GetComponent<HanoiPiece>().being_dragged = 0;
            if ((screenpos.x > -350) & (screenpos.x < -150))
            {
                int s = hanoiSetup.spaceToOccupy(0, gameObject.GetComponent<HanoiPiece>());
                if (s != -1)
                {
                    GameObject clone = Instantiate(gameObject);
                    GameObject panel = GameObject.Find("Panel");
                    clone.transform.position = new Vector3(peg[0], height[s], 0f);
                    clone.transform.SetParent(panel.transform, false);
                    Destroy(gameObject);
                    valid_drag = 1;
                }
                else
                {
                    gameObject.transform.position = originalPosition;
                }
            }
            else if ((screenpos.x > -100) & (screenpos.x < 100))
            {
                int s = hanoiSetup.spaceToOccupy(1, gameObject.GetComponent<HanoiPiece>());
                if (s != -1)
                {
                    
                    GameObject clone = Instantiate(gameObject);
                    GameObject panel = GameObject.Find("Panel");
                    clone.transform.position = new Vector3(peg[1], height[s], 0f);
                    clone.transform.SetParent(panel.transform, false);
                    Destroy(gameObject);
                    valid_drag = 1;
                }
                else
                {
                    gameObject.transform.position = originalPosition;
                }
            }
            
            else if ((screenpos.x > 150) & (screenpos.x < 350)) //peg2
            //else if ((gameObject.transform.position.x > 31) & (gameObject.transform.position.x < 43)) //peg2
            {
                int s = hanoiSetup.spaceToOccupy(2, gameObject.GetComponent<HanoiPiece>());
                if (s != -1)
                {
                    GameObject clone = Instantiate(gameObject);
                    GameObject panel = GameObject.Find("Panel");
                    clone.transform.position = new Vector3(peg[2], height[s], 0f);
                    clone.transform.SetParent(panel.transform, false);
                    Destroy(gameObject);
                    valid_drag = 1;
                }
                else
                {
                    gameObject.transform.position = originalPosition;
                }
            }
            else
            {
                gameObject.transform.position = originalPosition;
            }

        }
        if (valid_drag == 1)
        {
            gameController.moves_to_interrupt -= 1;
        }
        hanoiSetup.checkGoal();
    }


    /*public static GameObject itemBeingDragged;
    GameObject clone;
    Vector3 origin;

    public void OnBeginDrag(PointerEventData eventData)
    {
        gameObject.GetComponent<HanoiPiece>().dragged = 1;
    }
    public void OnDrag(PointerEventData eventData)
    {
        var v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
        transform.position = Camera.main.ScreenToWorldPoint(v3); // Allows for camera space conversion
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        int moved = closestSlot(gameObject.transform.position);
        HanoiPiece p = gameObject.GetComponent<HanoiPiece>();
        if ((p.original == 1) || (moved == 0))
        {
            transform.position = origin; // put the piece back in the original position
        }
        else if ((p.original == 0) || (moved == 1))
        {
            connections.removePiece(p.snap_piece);
            Destroy(gameObject);

            //Debug.Log("11111" + gameObject.GetComponent<Piece>().snap_piece);


        }
    }

    public void closestSlot(Vector3 pos)
    {
        return 1;
    }*/
}