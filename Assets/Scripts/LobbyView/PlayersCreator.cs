using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayersCreator : MonoBehaviour
{
    [SerializeField] private List<Color> colors = new();
    [SerializeField] private List<PlayerVisual> visuals = new();
    [SerializeField] private Sprite standartIcon;
    [SerializeField] private GameObject playerPoint;
    [SerializeField] private Vector2[] positions;
    [SerializeField] private TradeMenu tradeMenu;

    private TurnsManager turns;
    private WayPointsCreator wayPoints;


    public void Init(TurnsManager turns, WayPointsCreator wayPoints)
    {
        this.turns = turns;
        this.wayPoints = wayPoints;
    }

    public void RegisterPlayers(List<PlayerData> players, RectTransform startPos)
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].PlayerColor = colors[i];
            var sprite = players[i].PlayerAvatar == null ? standartIcon : players[i].PlayerAvatar;
            var wallet = players[i].PlayerWallet = new();
            wallet.AddMoney(15000);
            visuals[i].Setup(players[i], sprite);
            players[i].PlayerVisual = visuals[i];

            turns.OnTimerUpdate += visuals[i].UpdateTimer;
            players[i].PlayerWallet.OnSpendMoney += visuals[i].UpdateMoney;

            var obj = Instantiate(playerPoint, startPos);
            obj.TryGetComponent(out PlayerMovement movement);
            movement.Init(positions[i], colors[i], i, wayPoints);
            players[i].PlayerMovement = movement;

            visuals[i].TryGetComponent(out Button button);

            if (players[i] != PhotonPlayerFinder.GetPlayerData(PhotonNetwork.LocalPlayer))
            {
                int index = i;
                button.onClick.AddListener(() =>
                {
                    tradeMenu.SetData(players[index]);
                });
            }
        }
    }
}
