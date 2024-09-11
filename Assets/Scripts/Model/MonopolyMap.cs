using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonopolyMap 
{
    private List<IMapSector> map = new();
    private Dictionary<PlayerData, int> positions = new();

    public MonopolyMap(NotificationSercive service, List<BusinessConfig> configs,List<BusinessVisual> view)
    {
        map.Add(new MapStart());
        map.Add(new ImprovingBusiness(configs[0], service, view[0]));
        map.Add(new SurpriseSector(service));
        map.Add(new ImprovingBusiness(configs[1],service, view[1]));
        map.Add(new FineSector(service, 2000));
        map.Add(new CarsBusiness(configs[2], service, view[2]));
        map.Add(new ImprovingBusiness(configs[3], service, view[3]));
        map.Add(new SurpriseSector(service));
        map.Add(new ImprovingBusiness(configs[4], service, view[4]));
        map.Add(new ImprovingBusiness(configs[5], service, view[5]));
        map.Add(new PrisonSector());
        map.Add(new ImprovingBusiness(configs[6], service, view[6]));
        map.Add(new DevelopersBusiness(configs[7], service, view[7]));
        map.Add(new ImprovingBusiness(configs[8], service, view[8]));
        map.Add(new ImprovingBusiness(configs[9], service, view[9]));
        map.Add(new CarsBusiness(configs[10], service, view[10]));
        map.Add(new ImprovingBusiness(configs[11], service, view[11]));
        map.Add(new SurpriseSector(service));
        map.Add(new ImprovingBusiness(configs[12], service, view[12]));
        map.Add(new ImprovingBusiness(configs[13], service, view[13]));
        map.Add(new CasinoSector(service));
        map.Add(new ImprovingBusiness(configs[14], service, view[14]));
        map.Add(new SurpriseSector(service));
        map.Add(new ImprovingBusiness(configs[15], service, view[15]));
        map.Add(new ImprovingBusiness(configs[16], service, view[16]));
        map.Add(new CarsBusiness(configs[17],service, view[17]));
        map.Add(new ImprovingBusiness(configs[18], service, view[18]));
        map.Add(new ImprovingBusiness(configs[19], service, view[19]));
        map.Add(new DevelopersBusiness(configs[20], service, view[20]));
        map.Add(new ImprovingBusiness(configs[21], service, view[21]));
        map.Add(new PoliceSector());
        map.Add(new ImprovingBusiness(configs[22], service, view[22]));
        map.Add(new ImprovingBusiness(configs[23], service, view[23]));
        map.Add(new SurpriseSector(service));
        map.Add(new ImprovingBusiness(configs[24], service, view[24]));
        map.Add(new CarsBusiness(configs[25],service, view[25]));
        map.Add(new FineSector(service, 1000));
        map.Add(new ImprovingBusiness(configs[26], service, view[26]));
        map.Add(new SurpriseSector(service));
        map.Add(new ImprovingBusiness(configs[27], service, view[27]));
    }

    public void OnRefusing(Action<bool,PlayerData> action)
    {
        for (int i = 0; i < map.Count; i++)
        {
            if (map[i] is ISkipable sector)
            {
                sector.OnSkip += action;
            }
        }
    }
    public void InitPlayers(List<PlayerData> players)
    {
        for (int i = 0; i < players.Count; i++)
        {
            positions[players[i]] = 0;
        }
    }
    public bool MakeTurn(PlayerData player, int amount)
    {
        var newPosition = positions[player] + amount;

        if (newPosition > map.Count - 1)
        {
            newPosition -= map.Count;
            PhotonDataUpdater.Instance.ChangePlayerMoney(player, 2000);
            ChatLog.instance.AddMessage(player, "получил 2000$ за прохождение круга.");
        }

        positions[player] = newPosition;
        return map[newPosition].OnPlayerEnter(player);
    }
    public int GetPlayerPosition(PlayerData player)
    {
        return positions[player];
    }
    public List<Business> GetAllBusinesses()
    {
        List<Business> list = new List<Business>();
        for (int i = 0; i < map.Count; i++)
        {
            if (map[i] is Business business)
            {
                list.Add(business);
            }
        }
        return list;
    }
    public Business GetBusinessByName(string name)
    {
        var businesses = GetAllBusinesses();
        for (int i = 0; i < businesses.Count; i++)
        {
            if (businesses[i].GetConfig().BusinessName == name)
            {
                return businesses[i];
            }
        }
        throw new Exception("Name isn't correct");
    }
    public void GoToMapSector(int mapSector, PlayerData data)
    {
        map[mapSector].OnPlayerEnter(data);
        positions[data] = mapSector;

        data.PlayerMovement.MoveToParent(mapSector, false);
    }
}
