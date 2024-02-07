using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class ListItems : MonoBehaviour
{
    [SerializeField] TMP_Text textName;
    [SerializeField] TMP_Text textPlayerCount;

    public void SetInfo(RoomInfo info)
    {
        textName.text = info.Name;
        textPlayerCount.text = info.PlayerCount + "/" + info.MaxPlayers;
    }

    public void JoinToListRoom()
    {
        PhotonNetwork.JoinRoom(textName.text);
    }

    public void JoinRandRoomButton()
    {
        PhotonNetwork.JoinRandomRoom();
    }
}
