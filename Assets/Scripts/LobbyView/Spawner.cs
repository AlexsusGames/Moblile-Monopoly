using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : MonoBehaviourPunCallbacks
{
    [SerializeField] protected RectTransform obj;
    [SerializeField] protected RectTransform content;
    [SerializeField] protected float space;
    private float windowWidth = 900f;

    protected void SetContentSize(int count)
    {
        float offset = (obj.sizeDelta.y + space) * count;
        offset = offset > windowWidth ? offset : windowWidth;
        content.sizeDelta = new Vector2(content.sizeDelta.x, offset);
        content.anchoredPosition = new Vector2(content.anchoredPosition.x, content.anchoredPosition.y - content.rect.yMax);
    } 

    protected Vector2 GetFirstPosition()
    {
        Vector2 position = new Vector2(content.anchoredPosition.x, -content.rect.yMin - obj.sizeDelta.y);
        return position;
    }

}
