using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ImprovingBusiness : Business
{
    private BusinessImprovingConfig _config;
    private int level;
    public bool IsFilial { get; set; }
    public int BusinessId { get; }
    public ImprovingBusiness(BusinessConfig config, NotificationSercive service, BusinessVisual view) : base(config, service, view)
    {
        level = 0;
        _config = (BusinessImprovingConfig)config;
        BusinessId = _config.BusinessId;
    }

    protected override int GetRent(PlayerData data)
    {
        if (level == 0 && IsFilial) return _config.FillialOutcome;
        return _config.Outcomes[level];
    }
    public void ImproveFillial()
    {
        var playerWallet = ownerData.PlayerWallet;
        var payment = _config.ImprovingCost;
        if (playerWallet.Has(payment) && IsFilial)
        {
            playerWallet.RemoveMoney(payment);
            level++;
            UpdateView();
        }
    }
    public void SellLevel()
    {
        var playerWallet = ownerData.PlayerWallet;
        var payment = _config.ImprovingCost;
        playerWallet.AddMoney(payment);
        level--;
        UpdateView();
    }

    private void UpdateView()
    {
        int outcome = _config.Outcomes[level];
        if(IsFilial && level == 0)
        {
            outcome = _config.FillialOutcome;
        }
        Visual.UpdateOutcome(outcome);
        Visual.OnBusinessLevelChange(ownerData, level);
    }

    protected override void UpdatePrice(PlayerData data)
    {
        var playerBusiness = finder.FindImprovingById(data.Businesses, BusinessId);
        if(playerBusiness.isFillial)
        {
            for (int i = 0; i < playerBusiness.findingBusiness.Count; i++)
            {
                playerBusiness.findingBusiness[i].IsFilial = true;
                var config = playerBusiness.findingBusiness[i].GetConfig() as BusinessImprovingConfig;
                var level = playerBusiness.findingBusiness[i].level;
                playerBusiness.findingBusiness[i].Visual.UpdateOutcome(config.Outcomes[level]);
            }
        }
    }

    public override BusinessConfig GetConfig()
    {
        return _config;
    }
    public int GetLevel()
    {
        return level;
    }
}
