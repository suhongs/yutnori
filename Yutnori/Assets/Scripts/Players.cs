using Photon.Pun;
using Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class Players : MonoBehaviour
{
    public string nickName;
    public int score;          //최대 4점까지
    public int numPieces;      //board에 현재 있는 말 수

    public Piece[] pieces;
    public Transform[] tr = new Transform[4];

    public PhotonView pv;

    public ExitGames.Client.Photon.Hashtable ht;
    public ExitGames.Client.Photon.Hashtable ht2;

    private void Awake()
    {
        ht = PhotonNetwork.LocalPlayer.CustomProperties;
        if (pv.IsMine)
        {
            nickName = PhotonNetwork.LocalPlayer.NickName;
            pieces = new Piece[4];
            this.score = 0;
            this.numPieces = 4;
        }
    }

    public void setMalNum()
    {
        for (int i = 0; i < 4; i++)
            pieces[i].malNum = i;
    }

    private void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
    }

    public void putNetxTurn()
    {
        pv.RPC("nextTurn", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.ActorNumber);
    }

    [PunRPC]
    public void nextTurn(int id)
    {
        int target = id + 1; ;

        if (id == PhotonNetwork.CurrentRoom.PlayerCount)
            target = 1;
        if (id == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            ht["isTurn"] = false;
            ht["isThrow"] = false;
        }

        if (target == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            ht["isTurn"] = true;
        }

        foreach (Player pl in PhotonNetwork.PlayerList)
        {
            ht2 = pl.CustomProperties;
            if ((bool)ht2["end"])
            {
                PhotonNetwork.AutomaticallySyncScene = true;
                PhotonNetwork.LoadLevel("ENDING");
            }
        }
    }

    [PunRPC]
    public void pieceOnBoard(int boardNum, int playerNum, int pieceNum)
    {
        Boards.cube[boardNum].malOnBoard(playerNum, pieceNum);
    }

    public void pieceBoard(int boardNum, Piece piece)
    {
        pv.RPC("pieceOnBoard", RpcTarget.AllBuffered, boardNum, PhotonNetwork.LocalPlayer.ActorNumber, piece.malNum);
    }

    public void resetsBoard(int itors)
    {
        if (itors == -1)
            return;
        pv.RPC("resetBoard", RpcTarget.AllBuffered, itors);
    }

    [PunRPC]
    public void resetBoard(int boardNum)
    {
        Boards.cube[boardNum].resetBoard();
    }
    public int checkBoard(int itor)
    {
        if (Boards.cube[itor].pieceCount != 0)
        {
            if (Boards.cube[itor].playerNum != PhotonNetwork.LocalPlayer.ActorNumber)
            {
                for(int i = 0; i < Boards.cube[itor].pieceCount; i++)
                {
                    pv.RPC("beCatched", RpcTarget.AllBuffered, Boards.cube[itor].playerNum, Boards.cube[itor].pieceNum[i]);
                }
                pv.RPC("resetBoard", RpcTarget.AllBuffered, itor);
                return 1;
            }
        }
        return 0;
    }

    [PunRPC]
    public void beCatched(int playerNum, int malNum)
    {
        int viewID = 1000 * playerNum + malNum + 2;

        PhotonNetwork.GetPhotonView(viewID).GetComponent<Piece>().transform.position = PhotonNetwork.GetPhotonView(viewID).GetComponent<Piece>().defaultPos;
        PhotonNetwork.GetPhotonView(viewID).GetComponent<Piece>().currnetPos = PhotonNetwork.GetPhotonView(viewID).GetComponent<Piece>().defaultPos;
        PhotonNetwork.GetPhotonView(viewID).GetComponent<Piece>().itor = -1;
    }
}
