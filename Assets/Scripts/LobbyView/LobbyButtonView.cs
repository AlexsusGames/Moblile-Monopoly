using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Realtime;

public class LobbyButtonView : MonoBehaviour
{
    [SerializeField] private TMP_Text lobbyName;
    [SerializeField] private TMP_Text countOfPlayer;

    public void SetView(RoomInfo info)
    {
        lobbyName.text = info.Name;
        countOfPlayer.text = info.PlayerCount + "/" + info.MaxPlayers;
    }
}
