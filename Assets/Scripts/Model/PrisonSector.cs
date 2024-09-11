using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonSector : IMapSector, ISkipable
{
    public event Action<bool, PlayerData> OnSkip;

    public bool OnPlayerEnter(PlayerData player)
    {
        if(player.restTurns > 0)
        {
            ChatLog.instance.AddMessage(player, "попал в тюрьму на 2 хода.");
            OnSkip?.Invoke(false, player);
        }
        else ChatLog.instance.AddMessage(player, "посетил тюрьму с экскурсией.");
        return false;
    }
}
