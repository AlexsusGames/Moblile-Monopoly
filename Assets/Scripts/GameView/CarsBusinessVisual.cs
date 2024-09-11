using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarsBusinessVisual : BusinessVisual
{
    public override void OnBusinessLevelChange(PlayerData data, int level = 0)
    {
        if (BusinessConfig is BusinessCarsConfig config)
        {
            priceText.text = $"{config.StandartOutcome}$";
            tabColor.color = data.PlayerColor;
        }
        else throw new MissingComponentException("There is wrong config");
    }
}
