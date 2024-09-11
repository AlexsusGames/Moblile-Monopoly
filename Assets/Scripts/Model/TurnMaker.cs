using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TurnMaker 
{
    private MonopolyMap map;
    private PlayerMovementVisual view;

    public event Action<int,int, PlayerData> OnDice;
    public event Action<bool, PlayerData> OnFinishTurn;
    public event Action<int> OnUpdateTimer;

    public static int LastDiceCount;

    public TurnMaker(MonopolyMap map, PlayerMovementVisual view, List<PlayerData> players)
    {
        this.map = map;
        this.view = view;

        map.OnRefusing(SkipTurn);
        map.InitPlayers(players);
    }

    private void SkipTurn(bool value, PlayerData player)
    {
        OnFinishTurn?.Invoke(value, player);
    }

    public void MakeTurn(PlayerData data)
    {
        int firstDice = UnityEngine.Random.Range(1, 7);
        int secondDice = UnityEngine.Random.Range(1, 7);
        int amount = firstDice + secondDice;
        LastDiceCount = amount;

        OnDice?.Invoke(firstDice, secondDice, data);

        Action callBack = () =>
        {
            var result = map.MakeTurn(data, amount);

            OnFinishTurn?.Invoke(result, data);
            OnUpdateTimer?.Invoke(30);
        };

        view.Move(map.GetPlayerPosition(data), amount, callBack);
    }
}
