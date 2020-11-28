using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class ChatManager : MonoBehaviourPunCallbacks
{
    public List<string> chatList = new List<string>();
    public Button sendBtn;
    public Text chatLog;
    public InputField input;
    ScrollRect scroll_rect = null;
    string chatters;
 
    void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        scroll_rect = GameObject.FindObjectOfType<ScrollRect>();
    }

    private void Awake()
    {
        PhotonView photonView = gameObject.GetComponent<PhotonView>();
    }

    public void SendButtonOnClicked()
    {
        if (input.text.Equals("")) { Debug.Log("Empty"); return; }
        string msg = string.Format("[{0}] {1}", PhotonNetwork.LocalPlayer.NickName, input.text);
        photonView.RPC("ReceiveMsg", RpcTarget.OthersBuffered, msg);
        ReceiveMsg(msg);
        input.ActivateInputField();
        input.text = "";
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !input.isFocused) SendButtonOnClicked();
    }

    [PunRPC]
    public void ReceiveMsg(string msg)
    {
        chatLog.text += msg + "\n";
        scroll_rect.verticalNormalizedPosition = 0.0f;
    }


    public void sendMsg(string msg)
    {
        photonView.RPC("ReceiveMsg", RpcTarget.OthersBuffered, "<color=#ff0000>" + msg + "</color>");
        ReceiveMsg("<color=#ff0000>" + msg + "</color>");
        input.ActivateInputField();
        input.text = "";

    }
}
