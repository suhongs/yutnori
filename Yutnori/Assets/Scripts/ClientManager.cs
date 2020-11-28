using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class ClientManager : MonoBehaviourPunCallbacks
{
    public Players player;
    public Canvas canvas;
    public Transform point;
    public int itor;
    public Boards board;
    public Button throwBtn;
    public Player Player;
    public Text text;
    public GameObject[] cube = new GameObject[29];

    public Transform[] tr;

    public ChatManager cm;

    public int playerNum = 0;

    public int yutResult;

    public ExitGames.Client.Photon.Hashtable ht;

    public int boardNum;

    public int moveResult;
    
    private void Awake()
    {
        Player = PhotonNetwork.LocalPlayer;
        tr = new Transform[4];
        itor = 0;
        ht = PhotonNetwork.LocalPlayer.CustomProperties;

        if (GameObject.Find("P1malSp") == null || GameObject.Find("P2malSp") == null)
            return;
        player = PhotonNetwork.Instantiate("Prefabs/Players", new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Players>();
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            for (int i = 0; i < 4; i++)
            {
                tr[i] = GameObject.Find("P1malSp").transform.GetChild(i);
                player.pieces[i] = PhotonNetwork.Instantiate("Prefabs/mal1", tr[i].position, Quaternion.identity).GetComponent<Piece>();
            }
            ht["isTurn"] = true;
        }
        else if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            for (int i = 0; i < 4; i++)
            {
                tr[i] = GameObject.Find("P2malSp").transform.GetChild(i);
                player.pieces[i] = PhotonNetwork.Instantiate("Prefabs/mal2", tr[i].position, Quaternion.identity).GetComponent<Piece>();
            }
        }
        else if (PhotonNetwork.LocalPlayer.ActorNumber == 3)
        {
            for (int i = 0; i < 4; i++)
            {
                tr[i] = GameObject.Find("P3malSp").transform.GetChild(i);
                player.pieces[i] = PhotonNetwork.Instantiate("Prefabs/mal3", tr[i].position, Quaternion.identity).GetComponent<Piece>();
            }
        }
        player.setMalNum();
    }

    void Start()
    {
        throwBtn.onClick.AddListener(btnOnClick);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            yutResult = 1;
            ht["isThrow"] = true;
            cm.sendMsg("[" + PhotonNetwork.LocalPlayer.NickName + "]가 도를 던졌습니다");
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            yutResult = 2;
            ht["isThrow"] = true;
            cm.sendMsg("[" + PhotonNetwork.LocalPlayer.NickName + "]가 개를 던졌습니다");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            yutResult = 3;
            ht["isThrow"] = true;
            cm.sendMsg("[" + PhotonNetwork.LocalPlayer.NickName + "]가 걸를 던졌습니다");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            yutResult = 4;
            ht["isThrow"] = true;
            cm.sendMsg("[" + PhotonNetwork.LocalPlayer.NickName + "]가 윷를 던졌습니다");
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            yutResult = 5;
            ht["isThrow"] = true;
            cm.sendMsg("[" + PhotonNetwork.LocalPlayer.NickName + "]가 모를 던졌습니다");
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            yutResult = -1;
            ht["isThrow"] = true;
            cm.sendMsg("[" + PhotonNetwork.LocalPlayer.NickName + "]가 뒷도를 던졌습니다");
        }
        if (Input.GetMouseButtonDown(0))
        {
            if ((bool)ht["isTurn"] && (bool)ht["isThrow"]) 
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hitInformation = Physics2D.Raycast(touchPos, Camera.main.transform.forward);
                if (hitInformation.collider != null)
                {
                    Piece touchedObject = hitInformation.transform.gameObject.GetComponent<Piece>();
                    if (((PhotonView)touchedObject.GetComponent<PhotonView>()).IsMine)
                    {
                        int startItor = touchedObject.itor;
                        if (touchedObject.currnetPos == touchedObject.defaultPos)
                        {
                            boardNum = touchedObject.moveMal(yutResult);
                            if (boardNum > 70)
                                return;
                            moveResult = player.checkBoard(boardNum);
                            touchedObject.setMaltransform();
                            player.pieceBoard(boardNum, touchedObject);
                        }
                        else
                        {
                            for (int i = 0; i < Boards.cube[startItor].pieceCount; i++)
                            {
                                boardNum = player.pieces[Boards.cube[startItor].pieceNum[i]].moveMal(yutResult);
                                if(boardNum > 28)
                                {
                                    player.pieces[Boards.cube[startItor].pieceNum[i]].setMaltransform();
                                    player.pieceBoard(boardNum, player.pieces[Boards.cube[startItor].pieceNum[i]]);
                                    moveResult = 0;
                                    player.score++;
                                    continue;
                                }
                                if (boardNum > 70)
                                    return;
                                if(i == 0)
                                    moveResult = player.checkBoard(boardNum);
                                else
                                    player.checkBoard(boardNum);
                                player.pieces[Boards.cube[startItor].pieceNum[i]].setMaltransform();
                                player.pieceBoard(boardNum, player.pieces[Boards.cube[startItor].pieceNum[i]]);
                            }
                        }
                        player.resetsBoard(startItor);
                        if (yutResult == 4 || yutResult == 5 || moveResult == 1)
                        {
                            Player.CustomProperties["isThrow"] = false;
                            return;
                        }

                        if (player.score == 4)
                        {
                            ht["end"] = true;
                        }

                        player.putNetxTurn();
                        cm.sendMsg("[" + PhotonNetwork.LocalPlayer.NickName + "]의 턴이 종료 되었습니다.");
                    }
                }
            }
        }
    }


    public void throwYut()
    {
       
        yutResult = 0;
        int num;
       
        Random rand = new Random();
        for (int i = 0; i < 4; i++)
        {
            num = Random.Range(1, 12);
            if (num < 6)
                yutResult++;
        }
        if (yutResult == 1)
        {
            num = Random.Range(1, 8);
            if (num == 7)
                yutResult = -1;
        }
        if (yutResult == 0)
            yutResult = 5;

        return;

    }
    public void btnOnClick()
    {
        if ((bool)ht["isTurn"] && !(bool)ht["isThrow"])
        {
            throwYut();
            ht["isThrow"] = true;

            if (yutResult == 1)
                cm.sendMsg("[" + PhotonNetwork.LocalPlayer.NickName + "]가 도를 던졌습니다");
            else if (yutResult == 2)
                cm.sendMsg("[" + PhotonNetwork.LocalPlayer.NickName + "]가 개를 던졌습니다");
            else if (yutResult == 3)
                cm.sendMsg("[" + PhotonNetwork.LocalPlayer.NickName + "]가 걸를 던졌습니다");
            else if (yutResult == 4)
                cm.sendMsg("[" + PhotonNetwork.LocalPlayer.NickName + "]가 윷를 던졌습니다");
            else if (yutResult == 5)
                cm.sendMsg("[" + PhotonNetwork.LocalPlayer.NickName + "]가 모를 던졌습니다");
            else if (yutResult == -1)
                cm.sendMsg("[" + PhotonNetwork.LocalPlayer.NickName + "]가 뒷도를 던졌습니다");
        }
    }
}
