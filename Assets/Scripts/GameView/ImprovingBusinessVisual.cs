using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImprovingBusinessVisual : BusinessVisual
{
    [SerializeField] private Image startImage;
    [SerializeField] private List<Sprite> starsSprites = new();
    public override void OnBusinessLevelChange(PlayerData data, int level = 0)
    {
        if(level > 0)
        {
            startImage.gameObject.SetActive(true);
            startImage.sprite = starsSprites[level - 1];
        }
        else
        {
            startImage.gameObject.SetActive(false);
        }
        if(BusinessConfig is BusinessImprovingConfig config)
        {
            priceText.text = $"{config.Outcomes[level]}$"; 
            tabColor.color = data.PlayerColor;
        }
        else throw new MissingComponentException("There is wrong config");
    }
}
