using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInLobbyView : MonoBehaviour
{
    [SerializeField] private Image color;
    [SerializeField] private Image preparednessImage;
    [SerializeField] private TMP_Text nickText;
    [SerializeField] private Sprite isReadySprite;
    [SerializeField] private Sprite isntReadySprite;

    public bool IsPrepared { get; private set; } = true;
    public int PlayerId { get; private set; }

    public void SetView(Player player, Color color)
    {
        nickText.text = player.NickName;
        this.color.color = color;
        PlayerId = player.ActorNumber;
        ChangePreparedness();
    }

    public void ChangePreparedness()
    {
        if (IsPrepared)
        {
            preparednessImage.sprite = isntReadySprite;
            preparednessImage.color = Color.red;
        }
        else
        {
            preparednessImage.sprite = isReadySprite;
            preparednessImage.color = Color.green;
        }
        IsPrepared = !IsPrepared;
    }
}
