using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ChatLog : MonoBehaviour
{
    [SerializeField] private List<TMP_Text> chatLines;
    private List<string> messages = new();
    public static ChatLog instance = null;

    private PhotonView view;

    private void Awake()
    {
        view = GetComponent<PhotonView>();

        ClearChat();

        if (instance == null)
        {
            instance = this;
        }
    }

    private void ClearChat()
    {
        for (int i = 0; i < chatLines.Count; i++)
        {
            chatLines[i].text = string.Empty;
        }
    }

    public void AddMessage(PlayerData playerData, string message)
    {
        string colorId = ColorUtility.ToHtmlStringRGB(playerData.PlayerColor);
        string msg = $"<color=#{colorId}>{playerData.PlayerName}:</color> {message}";
        int charCount = msg.Length - message.Length;
        view.RPC(nameof(RPC_AddMessage), RpcTarget.All, $"<color=#{colorId}>{playerData.PlayerName}:</color> {message}", charCount);
    }

    [PunRPC]
    private void RPC_AddMessage(string message, int charCount)
    {
        if (messages.Contains(message))
        {
            return;
        }

        int lastCharIndex = 0;
        int length = 30;

        for (int i = charCount; i < message.Length;)
        {
            i += length;
            string newMsg;

            if(message.Length -1 > i)
            {
                int offset = lastCharIndex == 0 ? charCount : 0;
                newMsg = message.Substring(lastCharIndex, length + offset);
            }
            else newMsg = message.Substring(lastCharIndex);

            messages.Insert(0, newMsg);

            if (messages.Count == chatLines.Count)
            {
                messages.RemoveAt(chatLines.Count - 1);
            }

            for (int j = 0; j < messages.Count; j++)
            {
                chatLines[j].text = messages[j];
            }

            lastCharIndex = i;
        }
    }
}
