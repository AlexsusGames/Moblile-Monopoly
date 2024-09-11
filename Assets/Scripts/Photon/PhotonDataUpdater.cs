using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PhotonDataUpdater : MonoBehaviourPunCallbacks
{
    private PhotonView view;
    private MonopolyMap monopolyMap;
    private BusinessCenter businessCenter;

    public event Action<PlayerData> OnGiveUp;
    public static PhotonDataUpdater Instance { get; private set; }

    public void Init(MonopolyMap map, BusinessCenter center)
    {
        view = GetComponent<PhotonView>();

        businessCenter = center;
        monopolyMap = map;
        Instance = this;
    }

    public void BuyBusiness(string name)
    {
        int playerId = PhotonNetwork.LocalPlayer.ActorNumber;
        view.RPC(nameof(RPC_BuyBusiness), RpcTarget.Others, name, playerId);
    }

    [PunRPC]
    public void RPC_BuyBusiness(string name, int playerId)
    {
        var player = PhotonPlayerFinder.GetPlayerData(playerId);
        var business = monopolyMap.GetBusinessByName(name);

        business.BuyBusiness(player);
    }
    public void PayRent(string name, int sum) 
    {
        int playerId = PhotonNetwork.LocalPlayer.ActorNumber;
        view.RPC(nameof(RPC_PayRent), RpcTarget.Others, name, sum, playerId);
    }

    [PunRPC]
    public void RPC_PayRent(string name, int sum, int playerId)
    {
        var player = PhotonPlayerFinder.GetPlayerData(playerId);
        var business = monopolyMap.GetBusinessByName(name);

        business.PayMoneyToPlayer(player, business.GetOwnerData(), sum);
    }
    public void PawnBusiness(Business business)
    {
        string name = business.GetConfig().BusinessName;
        view.RPC(nameof(RPC_PawnBusiness), RpcTarget.All, name);
    }
    [PunRPC]
    public void RPC_PawnBusiness(string name)
    {
        var busienss = monopolyMap.GetBusinessByName(name);
        businessCenter.PawnBusiness(busienss);

        ChatLog.instance.AddMessage(busienss.GetOwnerData(), $"заложил поле '{name}'.");
    }
    public void UnpawnBusiness(Business business)
    {
        string name = business.GetConfig().BusinessName;
        view.RPC(nameof(RPC_UnpawnBusiness), RpcTarget.All, name);
    }
    [PunRPC]
    public void RPC_UnpawnBusiness(string name)
    {
        var busienss = monopolyMap.GetBusinessByName(name);
        businessCenter.UnpawnBusiness(busienss);

        ChatLog.instance.AddMessage(busienss.GetOwnerData(), $"выкупил поле '{name}'.");
    }
    public void ImproveBusiness(ImprovingBusiness business)
    {
        string name = business.GetConfig().BusinessName;
        view.RPC(nameof(RPC_ImproveBusiness), RpcTarget.All, name);
    }
    [PunRPC]
    public void RPC_ImproveBusiness(string name)
    {
        var business = monopolyMap.GetBusinessByName(name) as ImprovingBusiness;
        business.ImproveFillial();
    }
    public void SellLevel(ImprovingBusiness business)
    {
        string name = business.GetConfig().BusinessName;
        view.RPC(nameof(RPC_SellLevel), RpcTarget.All, name);
    }
    [PunRPC]
    public void RPC_SellLevel(string name)
    {
        var business = monopolyMap.GetBusinessByName(name) as ImprovingBusiness;
        business.SellLevel();
    }
    public void GiveUp(PlayerData player)
    {
        int playerNumber = PhotonPlayerFinder.GetPlayer(player).ActorNumber;
        view.RPC(nameof(RPC_GiveUp), RpcTarget.All, playerNumber);
    }
    [PunRPC]
    public void RPC_GiveUp(int playerNumber)
    {
        var playerData = PhotonPlayerFinder.GetPlayerData(playerNumber);
        var playerBusinesses = playerData.Businesses;

        for (int i = 0; i < playerBusinesses.Count; i++)
        {
            playerBusinesses[i].SetStandart();
        }

        playerData.PlayerMovement.Die();
        playerData.PlayerVisual.PlayerDie();

        OnGiveUp?.Invoke(playerData);
    }
    public void ChangePlayerMoney(PlayerData data, int sum)
    {
        int playerNumber = PhotonPlayerFinder.GetPlayer(data).ActorNumber;
        view.RPC(nameof(RPC_ChangePlayerMoney), RpcTarget.All, playerNumber, sum);
    }

    [PunRPC]
    public void RPC_ChangePlayerMoney(int playerNumber, int sum)
    {
        var player = PhotonPlayerFinder.GetPlayerData(playerNumber);
        var wallet = player.PlayerWallet;

        wallet.AddMoney(sum);
    }
    public void TakeRest(PlayerData player, int count)
    {
        int playerId = PhotonPlayerFinder.GetPlayer(player).ActorNumber;
        view.RPC(nameof(RPC_TakeRest), RpcTarget.All, playerId, count);
    }
    [PunRPC]
    public void RPC_TakeRest(int id, int count)
    {
        var playerData = PhotonPlayerFinder.GetPlayerData(id);
        playerData.restTurns = count;
    }
    public void HaveRest(PlayerData player)
    {
        int playerId = PhotonPlayerFinder.GetPlayer(player).ActorNumber;
        view.RPC(nameof(RPC_HaveRest), RpcTarget.All, playerId);
    }
    [PunRPC]
    public void RPC_HaveRest(int id)
    {
        var playerData = PhotonPlayerFinder.GetPlayerData(id);
        playerData.restTurns--;
    }
    public void GoToPrison(PlayerData player)
    {
        int playerId = PhotonPlayerFinder.GetPlayer(player).ActorNumber;
        view.RPC(nameof(RPC_GoToPrison), RpcTarget.All, playerId);
    }
    [PunRPC]
    public void RPC_GoToPrison(int playerId)
    {
        var playerData = PhotonPlayerFinder.GetPlayerData(playerId);
        TakeRest(playerData, 2);

        monopolyMap.GoToMapSector(10, playerData);
    }
    public List<Business> GetBusinesses(string[] names)
    {
        List<Business> list = new List<Business>();
        foreach (string name in names)
        {
            list.Add(monopolyMap.GetBusinessByName(name));
        }
        return list;
    }
    public void TradeBusinesses((List<Business> businesses,int moneyCount) playerDeal, (List<Business> businesses, int moneyCount) otherPlayerDeal, PlayerData playerData, PlayerData otherPlayerData)
    {
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

        int playerDataId = PhotonPlayerFinder.GetPlayer(playerData).ActorNumber;
        int otherPlayerDataId = PhotonPlayerFinder.GetPlayer(otherPlayerData).ActorNumber;

        view.RPC(nameof(RPC_TradeBusinesses), RpcTarget.All, playerBusinesses, otherPlayerBusinesses, playerDeal.moneyCount, otherPlayerDeal.moneyCount, otherPlayerDataId, playerDataId);
    }
    [PunRPC]
    public void RPC_TradeBusinesses(string[] playerBusinessNames, string[] otherPlayerBusinessNames, int playerMoney, int otherPlayerMoney, int senderId, int recieverId)
    {
        PlayerData playerData = PhotonPlayerFinder.GetPlayerData(recieverId);
        PlayerData otherPlayerData = PhotonPlayerFinder.GetPlayerData(senderId);
        List<Business> playerBusiness = GetBusinesses(playerBusinessNames);
        List<Business> otherPlayerBusiness = GetBusinesses(otherPlayerBusinessNames);

        for (int i = 0; i < playerBusiness.Count; i++)
        {
            playerBusiness[i].TradeBusiness(otherPlayerData);
        }
        for (int i = 0; i < otherPlayerBusiness.Count; i++)
        {
            otherPlayerBusiness[i].TradeBusiness(playerData);
        }
        otherPlayerData.PlayerWallet.RemoveMoney(otherPlayerMoney);
        playerData.PlayerWallet.AddMoney(otherPlayerMoney);
        playerData.PlayerWallet.RemoveMoney(playerMoney);
        otherPlayerData.PlayerWallet.AddMoney(playerMoney);
    }
}
