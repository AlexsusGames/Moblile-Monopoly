using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinTab : MonoBehaviour
{
    [SerializeField] private Image color;
    [SerializeField] private TMP_Text nick;
    [SerializeField] private Button exit;

    private void Awake()
    {
        exit.onClick.AddListener(Quit);
    }

    public void SetData(PlayerData data)
    {
        color.color = data.PlayerColor;
        nick.text = data.PlayerName;
    }
    public void Quit()
    {
        PhotonNetwork.LoadLevel(0);
        PhotonNetwork.LeaveRoom();
    }
}
