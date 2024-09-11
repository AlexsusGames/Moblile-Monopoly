using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float spead;
    [SerializeField] private Image image;

    private RectTransform rect;
    private Vector2 standartPosition;
    public bool _moveToStandart;

    public PhotonView View { get; private set;}
    private WayPointsCreator wayPoints;

    public void Init(Vector2 position, Color color, int index, WayPointsCreator wayPoints)
    {
        View = GetComponent<PhotonView>();
        View.ViewID = 100 + index;
        standartPosition = position;
        image.color = color;
        this.wayPoints = wayPoints;
    }
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        _moveToStandart = true;
    }

    public void MoveToParent(int transformIndex ,bool moveToStandart)
    {
        View.RPC(nameof(RPC_MoveToParent), RpcTarget.All, transformIndex, moveToStandart);
    }

    [PunRPC]
    public void RPC_MoveToParent(int transformIndex, bool moveToStandart)
    {
        rect.SetParent(wayPoints.GetMapPoint(transformIndex));
        _moveToStandart = moveToStandart;
    }

    private void FixedUpdate()
    {
        Vector2 position = _moveToStandart ? standartPosition : Vector2.zero;
        if (rect.anchoredPosition != position)
        {
            var pos = Vector3.MoveTowards(rect.anchoredPosition, position, spead * Time.fixedDeltaTime);
            rect.anchoredPosition = pos;
        }
    }
    public void Die()
    {
        Destroy(gameObject);
    }
}
