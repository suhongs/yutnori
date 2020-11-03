using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public Button loginBtn;
    public InputField nameText;
    public Text ConnectionStatus;
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        loginBtn.interactable = false;
        ConnectionStatus.text = "Connection to Master Server...";
        
    }

    public void Connect()
    {
        if (nameText.text.Equals(""))
            return;
        else
        {
            PhotonNetwork.LocalPlayer.NickName = nameText.text;
            loginBtn.interactable = false;
            if (PhotonNetwork.IsConnected)
            {
                ConnectionStatus.text = "connecting to room";
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                ConnectionStatus.text = "Offline : failed to connect.\nreconnecting...";
                PhotonNetwork.ConnectUsingSettings();
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        loginBtn.interactable = true;
        ConnectionStatus.text = "Online : connected to master server";
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        loginBtn.interactable = false;
        ConnectionStatus.text = "Offline : failed to connect.\nreconnection...";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        ConnectionStatus.text = "No empty room. creating new room...";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }

    public override void OnJoinedRoom()
    {
        ConnectionStatus.text = " Success to join room";
        PhotonNetwork.LoadLevel("Main");
    }
    void Update()
    {
        
    }
}
