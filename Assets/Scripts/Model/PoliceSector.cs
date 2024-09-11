using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceSector : IMapSector
{
    public bool OnPlayerEnter(PlayerData player)
    {
        PhotonDataUpdater.Instance.GoToPrison(player);
        return true;
    }
}
