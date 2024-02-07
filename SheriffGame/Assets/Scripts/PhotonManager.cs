using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private string region;
    [SerializeField] private TMP_InputField roomName;
    [SerializeField] private ListItems itemPrefab;
    [SerializeField] private Transform content;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion(region);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("You Connected to region " + PhotonNetwork.CloudRegion);
        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("You Disconected from server!");
    }

    public void CreateRoomButton()
    {
        if (!PhotonNetwork.IsConnected)
        {
            return;
        }
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        bool a = PhotonNetwork.CreateRoom(roomName.text, roomOptions, TypedLobby.Default);
        Debug.Log(a);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room Created " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room created failed!!!");

    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo roomInfo in roomList)
        {
            ListItems listItem = Instantiate(itemPrefab, content);
            if (listItem != null)
            {
                listItem.SetInfo(roomInfo);
            }
        }
    }
}
