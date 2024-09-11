using System.Collections.Generic;
using UnityEngine;

public class MonopolyMapCreator : MonoBehaviour
{
    [SerializeField] private StandartBusinesesConfig standartBusinesesConfig;
    [SerializeField] private NotificationSercive notifications;
    [SerializeField] private List<BusinessVisual> visuals;
    private MonopolyMap monopolyMap;

    private void Init()
    {
        monopolyMap = new(notifications, standartBusinesesConfig.config, visuals);
    }

    public MonopolyMap GetMonopolyMap()
    {
        if (monopolyMap == null) Init();
        return monopolyMap;
    }
}
