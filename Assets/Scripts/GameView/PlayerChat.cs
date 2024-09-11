using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class PlayerChat : MonoBehaviour
{
    [SerializeField] private TMP_InputField input;
    [SerializeField] private Button buttonSend;

    private PlayerData playerData;
    private void Start() => playerData = PhotonPlayerFinder.GetPlayerData(PhotonNetwork.LocalPlayer);

    public void SendMessage()
    {
        string text = input.text;

        if (CheckFormat(text) && !string.IsNullOrEmpty(text))
        {
            ChatLog.instance.AddMessage(playerData, text);
            input.text = "";
        }
    }

    private bool CheckFormat(string format)
    {
        int countOfSpace = 0;
        for (int i = 0; i < format.Length; i++)
        {
            if (char.IsWhiteSpace(format[i]))
            {
                countOfSpace++;
            }
        }
        if (countOfSpace < format.Length / 2) return true;
        return false;
    }
}
