using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DevelopersBusiness : Business
{
    private BusinessDevelopersConfig _config;
    public DevelopersBusiness(BusinessConfig config, NotificationSercive service, BusinessVisual view) : base(config, service, view)
    {
        _config = (BusinessDevelopersConfig)config;
    }

    public override BusinessConfig GetConfig()
    {
        return _config;
    }


    protected override int GetRent(PlayerData data)
    {
        return _config.StandartOutcome * TurnMaker.LastDiceCount;
    }

    protected override void UpdatePrice(PlayerData data)
    {
        var playerBusiness = finder.FindByType<DevelopersBusiness>(data.Businesses);
        int count = playerBusiness.Count == 2? 250 : 100;
        for (int i = 0; i < playerBusiness.Count; i++)
        {
            if (playerBusiness[i].GetConfig() is BusinessDevelopersConfig developers)
            {
                developers.StandartOutcome = count;
                playerBusiness[i].Visual.UpdateOutcome(count);
            }
        }
    }
}
