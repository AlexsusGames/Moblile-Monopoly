using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SurpriseSector : IMapSector, ISkipable
{
    private int[] amounts = { 100, 250, 500, 750, 1000 };
    private string[] getReasons = { "выиграл в лоторею", "получил в наследство", "нашел на дороге", "получил из-за банковской ошибки" };
    private string[] lostReasons = { "проиграл в казино", "получил штраф в размере:", "оплатил расходы в размере:", "оплатил страховку за" };
    private NotificationSercive sercive;
    public SurpriseSector(NotificationSercive sercive)
    {
        this.sercive = sercive;
    }

    public event Action<bool, PlayerData> OnSkip;

    public bool OnPlayerEnter(PlayerData player)
    {
        int random = UnityEngine.Random.Range(0, 2);
        switch (random)
        {
            case 0: return Surprise1(player);
            case 1: return Surprise2(player);
        }
        return false;
    }
    private bool Surprise1(PlayerData data)
    {
        int payment = amounts[UnityEngine.Random.Range(0, amounts.Length)];
        string reason = lostReasons[UnityEngine.Random.Range(0, lostReasons.Length)];
        NotificationData notification = new NotificationData(payment);
        UnityAction firstAction = () =>
        {
            PhotonDataUpdater.Instance.ChangePlayerMoney(data, -payment);
            ChatLog.instance.AddMessage(data, $"{reason} {payment}$.");
            OnSkip?.Invoke(false, data);
        };
        UnityAction secondAction = () =>
        {
            PhotonDataUpdater.Instance.GiveUp(data);
        };
        sercive.Open(notification, firstAction, secondAction);
        return true;
    }
    private bool Surprise2(PlayerData data)
    {
        int payment = amounts[UnityEngine.Random.Range(0, amounts.Length)];
        string reason = getReasons[UnityEngine.Random.Range(0, getReasons.Length)];
        PhotonDataUpdater.Instance.ChangePlayerMoney(data,payment);
        ChatLog.instance.AddMessage(data, $"{reason} {payment}$.");
        return false;
    }
}
