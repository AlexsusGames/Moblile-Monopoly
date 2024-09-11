using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class PhotonPlayerFinder 
{
    private static Dictionary<Player, PlayerData> playersMap = new();

    public static List<PlayerData> CreateMap()
    {
        if (playersMap.Count == 0)
        {
            List<PlayerData> list = new();
            Player[] players = PhotonNetwork.PlayerList;

            for (int i = 0; i < players.Length; i++)
            {
                PlayerData playerData = new PlayerData(players[i].NickName);
                list.Add(playerData);
                playersMap[players[i]] = playerData;
            }
            return list;
        }
        else
        {
            return playersMap.Values.ToList();
        }
    }

    public static PlayerData GetPlayerData(Player player)
    {
        return playersMap[player];
    }

    public static PlayerData GetPlayerData(int number)
    {
        foreach(var player in playersMap.Keys)
        {
            if (player.ActorNumber == number)
            {
                return playersMap[player];
            }
        }
        return null;
    }
    public static Player GetPlayer(PlayerData data)
    {
        foreach (var player in playersMap.Keys)
        {
            if (playersMap[player] == data)
            {
                return player;
            }
        }
        return null;
    }
}
