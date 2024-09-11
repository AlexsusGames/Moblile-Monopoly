using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PhotonLobbyCreator : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private TMP_Text errorText;
    [SerializeField] private Toggle[] checkBoxes;
    [SerializeField] private Toggle closedLobbyCheckBox;

    [SerializeField] private LobbyManager playerListUpdater;

    public void CreateRoom()
    {
        if (CheckErrors())
        {
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = CheckPlayerCount();
            options.IsVisible = !closedLobbyCheckBox.isOn;

            PhotonNetwork.CreateRoom(nameField.text, options, TypedLobby.Default);
        }
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room Created. Name - " + PhotonNetwork.CurrentRoom.Name);
        playerListUpdater.CreateLobby(PhotonNetwork.MasterClient);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Error: " + message);
    }

    private bool CheckErrors()
    { 
        if(CheckPlayerCount() == 0)
        {
            errorText.text = "������� ���������� �������!";
            return false;
        }

        if(nameField.text.Length == 0)
        {
            errorText.text = "������� �������� �����!";
            return false;
        }

        if (nameField.text.Length > 20)
        {
            errorText.text = "������� ������� ������� �����!";
            return false;
        }

        if (!PhotonNetwork.IsConnected)
        {
            errorText.text = "������ �����������!";
            return false;
        }

        return true;

    }

    private int CheckPlayerCount()
    {
        for (int i = 0; i < checkBoxes.Length; i++)
        {
            if (checkBoxes[i].isOn) return i + 2;
        }
        return 0;
    }
}
