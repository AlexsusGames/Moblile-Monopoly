using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;
using UnityEngine.Events;

public class TradeMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text otherPlayerName;
    [SerializeField] private GameObject playerButtons;
    [SerializeField] private GameObject otherPlayerButtons;
    [Header("Money")]
    [SerializeField] private Slider playerSlider;
    [SerializeField] private Slider otherPlayerSlider;
    [SerializeField] private TMP_Text playerMoney;
    [SerializeField] private TMP_Text otherPlayerMoney;
    [Header("Buttons")]
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button cancelButton;

    private (List<Business> businesses, int moneyCount) playerDeal;
    private (List<Business> businesses, int moneyCount) otherPlayerDeal;

    private PlayerData playerData;
    private PlayerData otherPlayerData;

    private PhotonView view;

    private void Awake()
    {
        playerSlider.onValueChanged.AddListener(ChangePlayerMoney);
        otherPlayerSlider.onValueChanged.AddListener(ChangeOtherPlayerMoney);
        view = GetComponent<PhotonView>();
    }
    public void SetData(PlayerData otherPlayer)
    {
        transform.GetChild(0).gameObject.SetActive(true);

        playerDeal.businesses = new();
        otherPlayerDeal.businesses = new();

        playerData = PhotonPlayerFinder.GetPlayerData(PhotonNetwork.LocalPlayer);
        otherPlayerData = otherPlayer;
        SetView(true);
        playerMoney.text = "0$";
        otherPlayerMoney.text = "0$";

        playerSlider.gameObject.SetActive(true);
        otherPlayerSlider.gameObject.SetActive(true);
        playerSlider.maxValue = playerData.PlayerWallet.GetMoney() / 100;
        otherPlayerSlider.maxValue = otherPlayer.PlayerWallet.GetMoney() / 100;

        UnityAction acceptAction = () =>
        {
            SendOffer();
            transform.GetChild(0).gameObject.SetActive(false);
            ChatLog.instance.AddMessage(playerData,"предложил игроку " +  otherPlayerData.PlayerName + " сделку.");
        };
        UnityAction cancelAction = () => transform.GetChild(0).gameObject.SetActive(false);

        AddButtonListeners(acceptAction, cancelAction);
    }
    private void SetData(List<Business> playerBusinesses, List<Business> otherPlayerBusinesses, int playerMoney, int otherPlayerMoney, PlayerData sender)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        playerData = PhotonPlayerFinder.GetPlayerData(PhotonNetwork.LocalPlayer);
        otherPlayerData = sender;
        SetView();
        for (int i = 0; i < playerBusinesses.Count; i++)
        {
            AddBusiness(playerBusinesses[i].GetConfig(), otherPlayerButtons);
        }
        for (int i = 0; i < otherPlayerBusinesses.Count; i++)
        {
            AddBusiness(otherPlayerBusinesses[i].GetConfig(), playerButtons);
        }
        this.playerMoney.text = $"{otherPlayerMoney}$";
        this.otherPlayerMoney.text = $"{playerMoney}$";

        playerSlider.gameObject.SetActive(false);
        otherPlayerSlider.gameObject.SetActive(false);

        playerDeal = new()
        {
            businesses = otherPlayerBusinesses,
            moneyCount = otherPlayerMoney,
        };
        otherPlayerDeal = new()
        {
            businesses = playerBusinesses,
            moneyCount = playerMoney,
        };

        UnityAction acceptAction = () =>
        {
            TakeOffer();
            transform.GetChild(0).gameObject.SetActive(false);
            ChatLog.instance.AddMessage(playerData, "согласился на сделку.");
        };
        UnityAction cancelAction = () =>
        {
            transform.GetChild(0).gameObject.SetActive(false);
            ChatLog.instance.AddMessage(playerData, "отказался от сделки.");
        };
        AddButtonListeners(acceptAction, cancelAction);
    }
    private void SetView(bool isEditing = false)
    {
        ResetButtons(isEditing);
        playerName.text = playerData.PlayerName;
        playerName.color = playerData.PlayerColor;
        otherPlayerName.text = otherPlayerData.PlayerName;
        otherPlayerName.color = otherPlayerData.PlayerColor;
        playerSlider.value = 0;
        otherPlayerSlider.value = 0;
    }
    public void AddBusiness(Business busienss)
    {
        if(transform.GetChild(0).gameObject.activeInHierarchy)
        {
            var config = busienss.GetConfig();

            if (playerData.Businesses.Contains(busienss) && !playerDeal.businesses.Contains(busienss))
            {
                if(AddBusiness(config, playerButtons))
                {
                    playerDeal.businesses.Add(busienss);
                }
            }

            if (otherPlayerData.Businesses.Contains(busienss) && !otherPlayerDeal.businesses.Contains(busienss))
            {
                if (AddBusiness(config, otherPlayerButtons))
                {
                    otherPlayerDeal.businesses.Add(busienss);
                }
            }
        }
    }
    private bool AddBusiness(BusinessConfig config, GameObject parent)
    {
        var buttonsCount = parent.transform.childCount;

        for (int i = 0; i < buttonsCount; i++)
        {
            Debug.Log(i);
            var obj = parent.transform.GetChild(i).gameObject;

            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                obj.transform.GetChild(0).GetComponent<Image>().sprite = config.businessLogo;
                return true;
            }
        }
        return false;
    }
    private void ResetButtons(bool isEditing)
    {
        for (int i = 0; i < playerButtons.transform.childCount; i++)
        {
            var playerObj = playerButtons.transform.GetChild(i).gameObject;
            var otherObj = otherPlayerButtons.transform.GetChild(i).gameObject;
            playerObj.TryGetComponent(out Button playerButton);
            otherObj.TryGetComponent(out Button otherPlayerButton);
            playerButton.onClick.RemoveAllListeners();
            otherPlayerButton.onClick.RemoveAllListeners();

            if (isEditing)
            {
                playerObj.SetActive(false);
                otherObj.SetActive(false);
                playerButton.onClick.AddListener(() => playerObj.SetActive(false));
                otherPlayerButton.onClick.AddListener(() => otherObj.SetActive(false));
            }
        }
    }
    private void ChangePlayerMoney(float value)
    {
        int sum = (int)value * 100;
        playerDeal.moneyCount = sum;
        ChangeMoney(playerMoney, sum);
    }
    private void ChangeOtherPlayerMoney(float value)
    {
        int sum = (int)value * 100;
        otherPlayerDeal.moneyCount = sum;
        ChangeMoney(otherPlayerMoney, sum);
    }
    private void ChangeMoney(TMP_Text text, int value)
    {
        text.text = $"{value}$";
    }
    private void AddButtonListeners(UnityAction accept, UnityAction cancel)
    {
        acceptButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
        acceptButton.onClick.AddListener(accept);
        cancelButton.onClick.AddListener(cancel);
    }
    private void TakeOffer()
    {
        PhotonDataUpdater.Instance.TradeBusinesses(playerDeal, otherPlayerDeal, playerData, otherPlayerData);
    }
    public void SendOffer()
    {
        var player = PhotonPlayerFinder.GetPlayer(otherPlayerData);
        int senderId = PhotonPlayerFinder.GetPlayer(playerData).ActorNumber;

        List<string> temp = new();

        string[] playerBusinesses;
        string[] otherPlayerBusinesses;

        for (int i = 0; i < playerDeal.businesses.Count; i++)
        {
            temp.Add(playerDeal.businesses[i].GetConfig().BusinessName);
        }

        playerBusinesses = temp.ToArray();
        temp = new();

        for (int i = 0; i < otherPlayerDeal.businesses.Count; i++)
        {
            temp.Add(otherPlayerDeal.businesses[i].GetConfig().BusinessName);
        }

        otherPlayerBusinesses = temp.ToArray();


        view.RPC(nameof(RPC_SendOffer), player, playerBusinesses, otherPlayerBusinesses, playerDeal.moneyCount, otherPlayerDeal.moneyCount, senderId);
    }
    [PunRPC]
    public void RPC_SendOffer(string[] playerBusinessNames, string[] otherPlayerBusinessNames, int playerMoney, int otherPlayerMoney, int senderId)
    {
        var playerList = PhotonDataUpdater.Instance.GetBusinesses(playerBusinessNames);
        var otherPlayerList = PhotonDataUpdater.Instance.GetBusinesses(otherPlayerBusinessNames);
        var sender = PhotonPlayerFinder.GetPlayerData(senderId);

        SetData(playerList, otherPlayerList, playerMoney, otherPlayerMoney, sender);
    }
}
