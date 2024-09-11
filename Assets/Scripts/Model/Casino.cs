using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Casino : MonoBehaviour
{
    [SerializeField] private CasinoView casinoView;
    [SerializeField] private Button button;
    private PhotonView photonView;
    private int[] combination = new int[3];
    private PlayerData cachedData;

    public static Casino Instance;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        Instance = this;
    }
    public void SetData(PlayerData player, UnityAction action)
    {
        cachedData = player;

        photonView.RPC(nameof(RPC_SetData), RpcTarget.All);

        if (PhotonNetwork.LocalPlayer == PhotonPlayerFinder.GetPlayer(player))
        {
            button.onClick.AddListener(() => photonView.RPC(nameof(RPC_PlayAnim), RpcTarget.All));
            casinoView.OnAnimPlayed += action;
            casinoView.OnAnimPlayed += GetPrise;
        }
    }
    [PunRPC]
    public void RPC_SetData()
    {
        casinoView.gameObject.SetActive(true);
        button.onClick.RemoveAllListeners();
        casinoView.OnAnimPlayed += () => casinoView.gameObject.SetActive(false);

        combination[0] = Random.Range(0, 2);
        combination[1] = Random.Range(0, 2);
        combination[2] = Random.Range(0, 2);

        casinoView.SetData(combination);
    }
    [PunRPC]
    private void RPC_PlayAnim()
    {
        casinoView.PlayAnim();
    }
    private void GetPrise()
    {
        int bet = -1000;
        if (combination[0] == 0 && combination[1] == 0 && combination[2] == 0)
        {
            bet = 3000;
            ChatLog.instance.AddMessage(cachedData, "выиграл в казино 3000$");
        }
        else if (combination[0] == 1 && combination[1] == 1 && combination[2] == 1)
        {
            bet = 6000;
            ChatLog.instance.AddMessage(cachedData, "выиграл в казино 6000$");
        }
        else ChatLog.instance.AddMessage(cachedData, "проиграл в казино 1000$");
        PhotonDataUpdater.Instance.ChangePlayerMoney(cachedData, bet);
    }
}
