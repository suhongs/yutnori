using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class Piece : MonoBehaviour
{
    public int malNum;
    public Vector3 defaultPos;
    public Vector3 currnetPos;
    public Transform point;
    public Board board;
    public int itor;
    public PhotonView pv;
    public Piece[] together = new Piece[4];
    public int isOthers;
    public int startItor;

    public void Start()
    {
        startItor = -1;
        itor = -1;
        currnetPos = transform.position;
        defaultPos = transform.position;
    }

    public int moveMal(int Yut)
    {
        startItor = itor;
        int moveCount = 0;
        
        if (Yut == -1)
        {
            if (transform.position == defaultPos)
                return 99;
            if (itor == 0)
                itor = 19;
            else if (itor == 20)
                itor = 4;
            else if (itor == 27)
                itor = 22;
            else if (itor == 25)
                itor = 9;
            else if (itor == 22)
                itor = 26;
            else
                itor--;
            
            return itor;
        }

        while (moveCount!= Yut)
        {
            if (moveCount == 0 && itor == 4)
                itor = 20;
            else if (moveCount == 0 && itor == 9)
                itor = 25;
            else if (itor == 22 && (startItor == 22 || startItor == 25 || startItor == 26 || startItor == 9))
                itor = 27;
            else if (itor == 24)
                itor = 14;
            else if (itor == 26)
                itor = 22;
            else if (itor == 28)
                itor = 19;
            else if (itor == 19)
            {
                itor = 28;
                itor += (PhotonNetwork.LocalPlayer.ActorNumber - 1) * 4 + this.malNum + 1;
                return itor;
            }
            else
                itor++;

            moveCount++;
        }
        return itor;
    }

    public void setMaltransform()
    {
        if (itor > 28)
        {
            point = Boards.cube[itor].transform;
        }
        else
            point = Boards.cube[itor].malTransform[Boards.cube[itor].pieceCount];
        this.transform.position = point.position;
        transform.Translate(new Vector3(0, 0, -0.11f));
        currnetPos = transform.position;
    }

    
    public void setOthers()
    {
        isOthers = Boards.cube[itor].pieceCount;
    }
}
