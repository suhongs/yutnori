using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
    Button startButton;
    public Transform canvas;
    public Text[] pInfo = new Text[4];

    private void Awake()
    {
        Instantiate(canvas, new Vector3(0, 0, 0), this.transform.rotation);
        ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable();

        ht.Add("isTurn", false);
        ht.Add("isThrow", false);
        ht.Add("end", false);

        PhotonNetwork.LocalPlayer.SetCustomProperties(ht);
    }
}
