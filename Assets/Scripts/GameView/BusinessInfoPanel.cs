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
        businessDescription.text = $"{GetBusinessInfo(config)}\n\n������� ���� - <color=black>{config.BusinessPrice}</color>k. " +
            $"\n����� ���� - <color=black>{config.SoldPrise}</color>k.\n����� ���� - <color=black>{config.BuyoutPrice}</color>k.\n{GetBusinessStats(config)}";

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
            default: return "������� �������, ����� ��������� �����.";
            case BusinessCarsConfig: return "����� ������� �� ���������� ��������-���������, �������� �� ��������.";
            case BusinessDevelopersConfig: return "����� ������� �� ����� ����� �� ������� � �� ���������� �������� ���������, �������� �� ��������.";
        }
    }
    private string GetBusinessStats(BusinessConfig config)
    {
        switch (config)
        {
            case BusinessImprovingConfig data: return $"���� ������� - <color=black>{data.ImprovingCost}</color>k\n\n�����:\n�������� - <color=black>{data.Outcomes[0]}</color>k.\n1 ������� - <color=black>{data.Outcomes[1]}</color>k." +
                    $"\n2 ������� - <color=black>{data.Outcomes[2]}</color>k.\n3 ������� - <color=black>{data.Outcomes[3]}</color>k.\n4 ������� - <color=black>{data.Outcomes[4]}</color>k." +
                    $"\n5 ������� - <color=black>{data.Outcomes[5]}</color>k.";
            case BusinessCarsConfig data: return $"\n\n�����:\n1 ���� - <color=black>{data.StandartOutcome}</color>k." +
                    $"\n2 ���� - <color=black>{data.StandartOutcome * 2}</color>k.\n3 ���� - <color=black>{data.StandartOutcome * 4}</color>k.\n4 ���� - <color=black>{data.StandartOutcome * 8}</color>k.";
            case BusinessDevelopersConfig data: return $"\n\n�����:\n1 ���� - <color=black>{data.StandartOutcome}</color>x." +
                    $"\n2 ���� - <color=black>{data.StandartOutcome * 2.5}</color>x.";
        }
        return "";
    }


}
