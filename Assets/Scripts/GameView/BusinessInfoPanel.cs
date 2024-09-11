using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BusinessInfoPanel : MonoBehaviour
{
    [SerializeField] private Image color;
    [SerializeField] private TMP_Text businessName;
    [SerializeField] private TMP_Text businessDescription;
    [SerializeField] private GameObject panel;
    [SerializeField] private BusinessDataChanger businessDataChanger;

    private PlayerData player;
    public void ChangePlayer(PlayerData player)
    {
        this.player = player;
    }
    public void SetupBusinessCenter(PhotonDataUpdater dataUpdater, TurnsManager turns)
    {
        businessDataChanger.Setup(dataUpdater, turns);
    }

    public void OpenInfoPanel(Business business, Color color)
    {
        var config = business.GetConfig();
        panel.SetActive(false);
        panel.SetActive(true);
        this.color.color = color;
        businessName.text = config.BusinessName;
        businessDescription.text = $"{GetBusinessInfo(config)}\n\nПокупка поля - <color=black>{config.BusinessPrice}</color>k. " +
            $"\nЗалог поля - <color=black>{config.SoldPrise}</color>k.\nВыкуп поля - <color=black>{config.BuyoutPrice}</color>k.\n{GetBusinessStats(config)}";

        if(player != null && business.GetOwnerData() == player)
        {
            businessDataChanger.OpenDataChanger(business);
        }
        else businessDataChanger.gameObject.SetActive(false);
    }
    public void HideWindow()
    {
        panel.SetActive(false);
    }
    private string GetBusinessInfo(BusinessConfig config)
    {
        switch (config)
        {
            default: return "Стройте филиалы, чтобы увеличить ренту.";
            case BusinessCarsConfig: return "Рента зависит от количества интернет-магазинов, которыми вы владеете.";
            case BusinessDevelopersConfig: return "Рента зависит от суммы чисел на кубиках и от количества почтовых отделений, которыми вы владеете.";
        }
    }
    private string GetBusinessStats(BusinessConfig config)
    {
        switch (config)
        {
            case BusinessImprovingConfig data: return $"Цена филиала - <color=black>{data.ImprovingCost}</color>k\n\nРента:\nСтандарт - <color=black>{data.Outcomes[0]}</color>k.\n1 Уровень - <color=black>{data.Outcomes[1]}</color>k." +
                    $"\n2 Уровень - <color=black>{data.Outcomes[2]}</color>k.\n3 Уровень - <color=black>{data.Outcomes[3]}</color>k.\n4 Уровень - <color=black>{data.Outcomes[4]}</color>k." +
                    $"\n5 Уровень - <color=black>{data.Outcomes[5]}</color>k.";
            case BusinessCarsConfig data: return $"\n\nРента:\n1 Поле - <color=black>{data.StandartOutcome}</color>k." +
                    $"\n2 Поля - <color=black>{data.StandartOutcome * 2}</color>k.\n3 Поля - <color=black>{data.StandartOutcome * 4}</color>k.\n4 Поля - <color=black>{data.StandartOutcome * 8}</color>k.";
            case BusinessDevelopersConfig data: return $"\n\nРента:\n1 Поле - <color=black>{data.StandartOutcome}</color>x." +
                    $"\n2 Поля - <color=black>{data.StandartOutcome * 2.5}</color>x.";
        }
        return "";
    }


}
