using Photon.Pun;
using System;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine.Events;

public abstract class Business : IMapSector, ISkipable
{
    protected NotificationSercive notification;
    protected BusinessConfig config;
    protected PlayerData ownerData;
    protected bool isBought;
    public bool isPawned;

    public event Action<PlayerData, int> OnBuy;
    public event Action<bool, PlayerData> OnSkip;
    public BusinessVisual Visual { get; }
    protected BusinessFinder finder = new();

    public Business(BusinessConfig config, NotificationSercive service, BusinessVisual view)
    {
        notification = service;
        this.config = config;
        OnBuy += view.OnBusinessLevelChange;
        view.BusinesData = this;
        Visual = view;
    }
    public PlayerData GetOwnerData()
    {
        return ownerData;
    }
    public void SetStandart()
    {

        if(ownerData != null)
        {
            ownerData.Businesses.Remove(this);
        }

        isPawned = false;
        isBought = false;

        UpdatePrice(ownerData);
        Visual.SetStandart();
        ownerData = null;

    }
    public virtual bool OnPlayerEnter(PlayerData player)
    {

        if (player == ownerData)
        {
            ChatLog.instance.AddMessage(player, "попал на свое поле.");
            return false;
        }

        if (isPawned)
        {
            ChatLog.instance.AddMessage(player, "не платит ренту, так как поле заложено.");
            return false;
        }

        NotificationData notificationData;
        UnityAction firstAction;
        UnityAction secondAction;
        if (isBought)
        {
            int rent = GetRent(ownerData);
            notificationData = new(ownerData, rent);
            secondAction = () => PhotonDataUpdater.Instance.GiveUp(player);

            firstAction = () =>
            {
                if (IsHasMoney(player, rent))
                {
                    PayMoneyToPlayer(player, ownerData, rent);
                    OnSkip?.Invoke(false, player);
                    PhotonDataUpdater.Instance.PayRent(config.BusinessName, rent);
                    ChatLog.instance.AddMessage(player, $"платит игроку {GetOwnerData().PlayerName} ренту: {rent}$");
                    notification.CloseTab();
                }
                else ErrorLog.instance.ShowError("Недостаточно денег!");
            };
        }
        else
        {
            ChatLog.instance.AddMessage(player, $"задумывается о покупке поля: {config.BusinessName}");
            notificationData = new(config);
            secondAction = () => OnSkip?.Invoke(false, player);

            firstAction = () =>
            {
                if (IsHasMoney(player, config.BusinessPrice))
                {
                    BuyBusiness(player);
                    OnSkip?.Invoke(false, player);
                    PhotonDataUpdater.Instance.BuyBusiness(config.BusinessName);
                    ChatLog.instance.AddMessage(player, $"купил поле: {config.BusinessName}");
                    notification.CloseTab();
                }
                else ErrorLog.instance.ShowError("Недостаточно денег!");
            };
        }

        notification.Open(notificationData, firstAction, secondAction, true);
        return true;
    }

    protected bool IsHasMoney(PlayerData player, int money)
    {
        if (player.PlayerWallet.Has(money)) return true;
        return false;
    }
    public void PayMoneyToPlayer(PlayerData payer, PlayerData reciever, int sum)
    {
        payer.PlayerWallet.RemoveMoney(sum);
        reciever.PlayerWallet.AddMoney(sum);
    }
    public virtual void BuyBusiness(PlayerData player)
    {
        var playerWallet = player.PlayerWallet;
        playerWallet.RemoveMoney(config.BusinessPrice);
        player.Businesses.Add(this);
        ownerData = player;
        isBought = true;
        OnBuy?.Invoke(player, 0);
        UpdatePrice(player);
    }
    public virtual void TradeBusiness(PlayerData newOwner)
    {
        ownerData.Businesses.Remove(this);
        ownerData = newOwner;
        newOwner.Businesses.Add(this);
        OnBuy?.Invoke(newOwner, 0);
        UpdatePrice(newOwner);
    }
    public abstract BusinessConfig GetConfig();
    protected abstract int GetRent(PlayerData data);
    protected abstract void UpdatePrice(PlayerData data);
}