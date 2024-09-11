using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TurnsManager : MonoBehaviourPunCallbacks
{
    private int waitingTime = 0;
    private int index = 0;

    private Dictionary<PlayerData, bool> finishedTurns = new();
    private List<PlayerData> players = new();
    private TurnMaker turnMaker;
    private PhotonView view;
    private NotificationSercive service;

    private PlayerData cathedData;

    public event Action<PlayerData> OnPlayerTurn;
    public event Action<PlayerData, int> OnTimerUpdate;
    public event Action OnCircleComplated;
    public event Action OnChangePlayer;

    [SerializeField] private UnityEvent onGameEnd;
    [SerializeField] private WinTab winTab;
    [SerializeField] private Button quitButton;

    public int RemainedActions { get; set; }

    public void SetService(NotificationSercive service)
    {
        this.service = service;
        view = GetComponent<PhotonView>();
    }

    public void Setup(List<PlayerData> players, TurnMaker turnMaker)
    {
        this.players = players;
        this.turnMaker = turnMaker;
        MakeTurn(false);
        StartGame();

        PhotonDataUpdater.Instance.OnGiveUp += DeletePlayer;
    }

    private async void StartGame()
    {
        while(players.Count > 1)
        {
            ResetTime(50);
            RemainedActions = 1;
            var player = players[index];
            Debug.Log(player.PlayerName + index);
            if(player.restTurns > 0)
            {
                PhotonDataUpdater.Instance.HaveRest(player);
                index++;

                if (index >= players.Count)
                {
                    index = 0;
                    OnCircleComplated?.Invoke();
                }

                MakeTurn(false, player);
                continue;
            }

            cathedData = player;
            OnPlayerTurn?.Invoke(player);
            NotificationData data = new(true);

            UnityAction diceAction = () => turnMaker.MakeTurn(player);

            if(player == PhotonPlayerFinder.GetPlayerData(PhotonNetwork.LocalPlayer))
            {
                service.Open(data, diceAction);
            }

            while (waitingTime > 0)
            {
                if (CheckTurn(player))
                {
                    break;
                }

                OnTimerUpdate?.Invoke(player, waitingTime);
                await Task.Delay(1000);
                waitingTime--;
            }

            OnChangePlayer?.Invoke();

            if (waitingTime == 0)
            {
                PhotonDataUpdater.Instance.GiveUp(player);
                RemovePlayer(player);

                if (players.Count == 1)
                {
                    onGameEnd?.Invoke();
                }

                if (index >= players.Count)
                {
                    index = 0;
                    OnCircleComplated?.Invoke();
                }
                continue;
            }

            index++;
            if (index >= players.Count)
            {
                index = 0;
                OnCircleComplated?.Invoke();
            }
        }
    }
    private void DeletePlayer(PlayerData playerData)
    {
        if (playerData == cathedData)
        {
            ResetTime(1);
        }
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        var playerData = PhotonPlayerFinder.GetPlayerData(otherPlayer);
        if (playerData == cathedData)
        {
            ResetTime(1);
        }
        else
        {
            PhotonDataUpdater.Instance?.GiveUp(playerData);
            RemovePlayer(playerData);
            if(players.Count == 1)
            {
                onGameEnd?.Invoke();
            }
        }
    }

    public void ResetTime(int time)
    {
        view.RPC(nameof(RPC_ResetTime), RpcTarget.All, time);
    }
    [PunRPC]
    public void RPC_ResetTime(int time)
    {
        waitingTime = time;
    }

    public void RemovePlayer(PlayerData player)
    {
        view.RPC(nameof(RPC_RemovePlayer), RpcTarget.All, PhotonPlayerFinder.GetPlayer(player).ActorNumber);
    }
    [PunRPC]
    public void RPC_RemovePlayer(int index)
    {
        var player = PhotonPlayerFinder.GetPlayerData(index);
        players.Remove(player);

        if (player == PhotonPlayerFinder.GetPlayerData(PhotonNetwork.LocalPlayer))
        {
            quitButton.gameObject.SetActive(true);
        }
    }

    private bool CheckTurn(PlayerData data)
    {
        return finishedTurns[data];
    }
    public void MakeTurn(bool result, PlayerData data = null)
    {
        var player = PhotonPlayerFinder.GetPlayer(data);
        int number = player == null ? -1 : player.ActorNumber;
        view.RPC(nameof(RPC_MakeTurn), RpcTarget.All, result, number);
    }
    [PunRPC]
    public void RPC_MakeTurn(bool result, int numberOfPlayer)
    {
        var playerData = PhotonPlayerFinder.GetPlayerData(numberOfPlayer);

        if (result == false)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i] == playerData)
                {
                    finishedTurns[players[i]] = true;
                }
                else finishedTurns[players[i]] = false;
            }
        }
    }
    public void Win()
    {
        winTab.SetData(players[0]);
    }

}
