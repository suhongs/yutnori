using Photon.Pun;
using Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class Board : MonoBehaviour
{
    public int pieceCount;
    public int[] pieceNum = new int[4];
    public int playerNum;
    public Transform[] malTransform = new Transform[4];


    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < malTransform.Length; i++)
        {
            malTransform[i] = transform.GetChild(i);
        }
        pieceCount = 0;
    }

    public void malOnBoard(int playerNum, int malNum)
    {
        this.playerNum = playerNum;
        this.pieceNum[pieceCount++] = malNum;
    }

    public void resetBoard()
    {
        pieceCount = 0;
    }
}
