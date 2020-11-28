using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boards : MonoBehaviour
{
    public static Transform[] boardPoint;
    public static Board[] cube;
    public Transform[,] endPos;

    private void Awake()
    {
        boardPoint = new Transform[transform.childCount];
        cube = new Board[transform.childCount];
        endPos = new Transform[4, 4];

        for (int i = 0; i < boardPoint.Length; i++)
        {
            boardPoint[i] = transform.GetChild(i);
            cube[i] = transform.GetChild(i).GetComponent<Board>();
        }
    }

    private void Start()
    {
        
    }

}
