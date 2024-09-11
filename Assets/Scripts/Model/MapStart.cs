using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapStart : IMapSector
{
    public bool OnPlayerEnter(PlayerData player)
    {
        PhotonDataUpdater.Instance.ChangePlayerMoney(player, 1000);
        ChatLog.instance.AddMessage(player, "попал на сектор '—тарт' и получил дополнительно 1000$.");
        return false;
    }
}
