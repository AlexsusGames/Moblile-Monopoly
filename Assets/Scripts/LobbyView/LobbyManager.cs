using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LobbyManager : Spawner
{
    [SerializeField] private List<Color> colorList = new();
    [SerializeField] private Button prepButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button startButton;
    [SerializeField] private TMP_Text countOfPlayers;
    [SerializeField] private TMP_Text errorInfo;

    [SerializeField] private UnityEvent onLeaveRoom;

    private Vector2 lastPosition;
    private float SpaceBetweenButtons => obj.sizeDelta.y + space;

    private List<GameObject> list = new();

    private void Awake()
    {
        exitButton.onClick.AddListener(LeaveRoom);
        startButton.onClick.AddListener(StartGame);
        prepButton.onClick.AddListener(() => photonView.RPC(nameof(RPC_UpdatePlayerPrepareness), RpcTarget.All, PhotonNetwork.LocalPlayer));

        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        onLeaveRoom?.Invoke();
    }

    public void CreateLobby(Player creator)
    {
        Debug.Log("Lobby Created " + creator.NickName);
        photonView.RPC(nameof(RPC_UpdatePlayerList), RpcTarget.All);
        
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("New player joined");

        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC(nameof(RPC_UpdatePlayerList), RpcTarget.All);
        }
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC(nameof(RPC_UpdatePlayerList), RpcTarget.All);
        }
        Debug.Log("Player left room");
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
        Debug.Log($"New master - {newMasterClient}");
        StartButtonAvailavle(PhotonNetwork.IsMasterClient);
    }
    public override void OnJoinedRoom()
    {
        StartButtonAvailavle(PhotonNetwork.IsMasterClient);
    }

    public void UpdateList()
    {
        List<Player> lobbyPlayers = PhotonNetwork.PlayerList.ToList();

        UpdateCount(lobbyPlayers.Count);
        DeleteList();
        SpawnButton(lobbyPlayers);
    }

    private void DeleteList()
    {
        for (int i = 0; i < list.Count; i++)
        {
            Destroy(list[i]);
        }
        list.Clear();
    }


    private void SpawnButton(List<Player> players)
    {
        lastPosition = GetFirstPosition();

        for (int i = 0; i < players.Count; i++)
        {
            var rect = Instantiate(obj, content);
            rect.TryGetComponent(out PlayerInLobbyView view);
            rect.SetParent(content);
            rect.anchoredPosition = lastPosition;
            view.SetView(players[i], colorList[i]);
            lastPosition.y -= SpaceBetweenButtons;
            list.Add(rect.gameObject);
        }
    }

    [PunRPC]
    private void RPC_UpdatePlayerList()
    {
        UpdateList();
    }
    [PunRPC]
    private void RPC_UpdatePlayerPrepareness(Player player)
    {
        foreach (var item in list)
        {
            item.TryGetComponent(out PlayerInLobbyView view);

            if(view.PlayerId == player.ActorNumber)
            {
                view.ChangePreparedness();
            }
        }
    }

    private void UpdateCount(int count)
    {
        countOfPlayers.text = count.ToString() + "/" + PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
    }
    private void StartButtonAvailavle(bool value)
    {
        startButton.gameObject.SetActive(value);
    }
    private void ShowError(string error)
    {
        errorInfo.gameObject.SetActive(false);
        errorInfo.gameObject.SetActive(true);
        errorInfo.text = error;
    }

    public void StartGame()
    {
        if(list.Count != PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            ShowError("Недостаточно игроков!");
            return;
        }

        for (int i = 0; i < list.Count; i++)
        {
            list[i].TryGetComponent(out PlayerInLobbyView view);
            if (!view.IsPrepared)
            {
                ShowError("Не все игроки готовы!");
                return;
            }
        }

        PhotonNetwork.LoadLevel("Game");
    }
}
