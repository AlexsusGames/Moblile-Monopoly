using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WayPointsCreator : MonoBehaviour
{
    [SerializeField] private RectTransform wayPoint;
    [SerializeField] private RectTransform[] mapObjects;
    private List<RectTransform> wayPoints = new();
    public int CountOfWays => wayPoints.Count;

    private void Awake()
    {
        CreateWays();
    }
    private void CreateWays()
    {
        for (int i = 0; i < mapObjects.Length; i++)
        {
            var obj = Instantiate(wayPoint,transform);
            wayPoints.Add(obj);
        }

        for (int i = 0; i < wayPoints.Count; i++)
        {
            wayPoints[i].anchoredPosition = mapObjects[i].anchoredPosition;
        }
    }
    public RectTransform GetMapPoint(int index)
    {
        return wayPoints[index];
    }
}
