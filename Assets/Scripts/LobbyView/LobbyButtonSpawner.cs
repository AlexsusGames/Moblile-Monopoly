using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class LobbyButtonSpawner : Spawner
{
    private List<RectTransform> buttons = new();

    public void UpdateList(List<RoomInfo> info)
    {
        DestroyButtons();
        Spawn(info);
    }

    private void Spawn(List<RoomInfo> info)
    {
        SetContentSize(info.Count);
        Vector2 pos = GetFirstPosition();

        for (int i = 0; i < info.Count; i++)
        {
            if (info[i].PlayerCount != 0)
            {
                int index = i;
                var obj = Instantiate(base.obj, content);
                obj.TryGetComponent(out LobbyButtonView view);
                obj.TryGetComponent(out Button button);
                view.SetView(info[i]);
                buttons.Add(obj);
                obj.anchoredPosition = pos;
                pos.y -= obj.rect.yMax + space;
                button.onClick.AddListener(() => PhotonNetwork.JoinRoom(info[index].Name));
            }
        }
    }
    private void DestroyButtons()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            Destroy(buttons[i].gameObject);
        }
        buttons.Clear();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("RoomList is updated");
        UpdateList(roomList);
    }
}
