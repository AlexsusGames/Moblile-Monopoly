using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CasinoSector : IMapSector, ISkipable
{
    private NotificationSercive sercive;
    public CasinoSector(NotificationSercive sercive)
    {
        this.sercive = sercive;
    }

    public event Action<bool, PlayerData> OnSkip;

    public bool OnPlayerEnter(PlayerData player)
    {
        NotificationData data = new NotificationData()
        {
            Title = "�������� �����?",
            Message = "���������� ���������� �������, ����� ����������� ���� �������",
            FirstAction = "������",
            SecondAction = "����������",
        };
        UnityAction skipAction = () => OnSkip?.Invoke(false, player);
        UnityAction firstAction = () => Casino.Instance.SetData(player, skipAction);
        sercive.Open(data, firstAction, skipAction);
        return true;
    }
}
