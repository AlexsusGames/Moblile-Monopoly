using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class DevelopersBusinessVisual : BusinessVisual
{
    public override void OnBusinessLevelChange(PlayerData data, int level = 0)
    {
        if (BusinessConfig is BusinessDevelopersConfig config)
        {
            priceText.text = $"{config.StandartOutcome}x";
            tabColor.color = data.PlayerColor;
        }
        else throw new MissingComponentException("There is wrong config");
    }
    public override void UpdateOutcome(int newOutcome)
    {
        priceText.text = $"{newOutcome}x";
    }
}
