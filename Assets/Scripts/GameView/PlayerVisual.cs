using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private Image playerImage;
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text playerMoney;
    [SerializeField] private TMP_Text playerTimer;
    [SerializeField] private Image timerTab;
    [SerializeField] private Image playerColor;
    [SerializeField] private Image playerTab;
    [SerializeField] private Color standartColorl;
    [SerializeField] private Color lostColor;
    private bool isAlive = true;

    public void Setup(PlayerData data,Sprite icon)
    {
        gameObject.SetActive(true);
        playerColor.color = data.PlayerColor;
        playerMoney.text = $"$<color=white>{data.PlayerWallet.GetMoney()}</color>k";
        playerImage.sprite = icon;
        playerName.text = data.PlayerName;
    }
    public void UpdateMoney(int newAmount)
    {
        playerMoney.text = $"$<color=white>{newAmount}</color>k";
    }
    public void PlayerDie()
    {
        isAlive = false;
        playerTab.color = lostColor;
        playerImage.color = lostColor;
        playerColor.color = standartColorl;
        playerMoney.gameObject.SetActive(false);
        timerTab.gameObject.SetActive(false);
    }

    public void UpdateTimer(PlayerData data, int seconds)
    {
        if (isAlive)
        {
            if (data.PlayerColor == playerColor.color)
            {
                timerTab.gameObject.SetActive(true);
                playerTimer.text = seconds.ToString();
                playerTab.color = playerColor.color;
            }
            else
            {
                timerTab.gameObject.SetActive(false);
                playerTab.color = standartColorl;
            }
        }
    }
}
