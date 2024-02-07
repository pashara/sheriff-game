using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private string region;
    [SerializeField] private string nickName;

    [SerializeField] private TMP_InputField roomName;
    [SerializeField] private ListItems itemPrefab;
    [SerializeField] private Transform content;

    private List<RoomInfo> allRoomsInfo = new List<RoomInfo>();

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion(region);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("You Connected to region " + PhotonNetwork.CloudRegion);
        if (nickName == "")
            PhotonNetwork.NickName = "User";
        else
            PhotonNetwork.NickName = nickName;

        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
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
        PhotonNetwork.LoadLevel("GameScene");
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
            for (int i = 0; i < allRoomsInfo.Count; i++)
            {
                if (allRoomsInfo[i].masterClientId == roomInfo.masterClientId)
                {
                    return;
                }
            }

            ListItems listItem = Instantiate(itemPrefab, content);
            if (listItem != null)
            {
                listItem.SetInfo(roomInfo);
                allRoomsInfo.Add(roomInfo);
            }
        }
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("GameScene");
    }
    public void JoinRandRoomButton()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void JoinButton()
    {
        PhotonNetwork.JoinRoom(roomName.text);
    }

    public void LeaveButton()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("Login");
    }
}
