using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarsBusiness : Business
{
    private BusinessCarsConfig _config;
    public CarsBusiness(BusinessConfig config, NotificationSercive service, BusinessVisual view) : base(config, service, view)
    {
        _config = (BusinessCarsConfig)config;
    }

    public override BusinessConfig GetConfig()
    {
        return _config;
    }

    protected override int GetRent(PlayerData data)
    {
        return _config.StandartOutcome;
    }

    protected override void UpdatePrice(PlayerData data)
    {
        var playerBusiness = finder.FindByType<CarsBusiness>(data.Businesses);
        int count = 0;

        if (playerBusiness.Count > 0) count = 250;
        if (playerBusiness.Count > 1) count = 500;
        if (playerBusiness.Count > 2) count = 1000;
        if (playerBusiness.Count > 3) count = 2000;

        for (int i = 0; i < playerBusiness.Count; i++)
        {
            playerBusiness[i]._config.StandartOutcome = count;
            playerBusiness[i].Visual.UpdateOutcome(count);
        }
    }
}
