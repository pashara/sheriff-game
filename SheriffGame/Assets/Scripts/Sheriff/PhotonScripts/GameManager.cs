using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Pun.Demo.Cockpit;

public class GameManager : MonoBehaviour
{
    [SerializeField] TMP_Text textLastMassage;
    [SerializeField] TMP_InputField inputMassageField;

    private PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }
    public void SendButton()
    {
        photonView.RPC("Send_Date", RpcTarget.AllBuffered, PhotonNetwork.NickName, inputMassageField.text);
    }

    [PunRPC]
    public void Send_Date(string nick, string massage)
    {
        textLastMassage.text = nick + ": " + massage;
    }
}
