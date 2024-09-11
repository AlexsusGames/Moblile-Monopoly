using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FineSector : IMapSector, ISkipable
{
    private NotificationSercive sercive;
    private int fineAmount;

    public FineSector(NotificationSercive sercive,int fineAmount)
    {
        this.sercive = sercive;
        this.fineAmount = fineAmount;
    }

    public event Action<bool, PlayerData> OnSkip;

    public bool OnPlayerEnter(PlayerData player)
    {
        UnityAction firstAction = () =>
        {
            if (player.PlayerWallet.Has(fineAmount))
            {
                PhotonDataUpdater.Instance.ChangePlayerMoney(player, -fineAmount);
                OnSkip?.Invoke(false, player);
                ChatLog.instance.AddMessage(player, $"заплатил штраф банку: {fineAmount}$");
                sercive.CloseTab();
            }
            else ErrorLog.instance.ShowError("Не хватает денег!");
        };

        UnityAction secondAction = () => PhotonDataUpdater.Instance.GiveUp(player);

        NotificationData data = new NotificationData(fineAmount);

        sercive.Open(data, firstAction, secondAction, true);
        return true;
    }
}
