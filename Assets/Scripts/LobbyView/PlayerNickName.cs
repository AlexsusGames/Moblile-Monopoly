using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNickName : MonoBehaviour
{
    [SerializeField] private TMP_Text nick;
    [SerializeField] private GameObject nickCreateWindow;

    private const string Key = "NickName";

    private void Awake()
    {
        if (!PlayerPrefs.HasKey(Key))
        {
            nickCreateWindow.SetActive(true);
        }
        SetNick();
    }

    private void SetNick()
    {
        string nick = PlayerPrefs.GetString(Key);
        PhotonNetwork.NickName = nick;
        this.nick.text = nick;
    }
    public void SaveNick(string nick)
    {
        PlayerPrefs.SetString(Key, nick);
        SetNick();
    }
}
