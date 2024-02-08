using UnityEngine;
using TMPro;
using Photon.Realtime;

public class ListItems : MonoBehaviour
{
    [SerializeField] TMP_Text textName;
    [SerializeField] TMP_Text textPlayerCount;

    public void SetInfo(RoomInfo info)
    {
        textName.text = info.Name;
        textPlayerCount.text = info.PlayerCount + "/" + info.MaxPlayers;
    }
}
