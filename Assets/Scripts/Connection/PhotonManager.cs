using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private string region;
    [SerializeField] private UnityEvent onJoinRoom;

    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion(region);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Is connecting to " + PhotonNetwork.CloudRegion);

        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Is disconnecting. Cause:{cause}");
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Room joined. Name - " + PhotonNetwork.CurrentRoom.Name);
        onJoinRoom?.Invoke();
    }
}
