using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class BusinessDataChanger : MonoBehaviour
{
    [SerializeField] private Button improveButton;
    [SerializeField] private Button sellButton;
    private TMP_Text improveText;
    private TMP_Text sellText;

    private PhotonDataUpdater photonDataUpdater;
    private TurnsManager turnsManager;

    public void Setup(PhotonDataUpdater dataUpdater, TurnsManager turnManager)
    {
        photonDataUpdater = dataUpdater;
        turnsManager = turnManager;
        improveText = improveButton.GetComponentInChildren<TMP_Text>();
        sellText = sellButton.GetComponentInChildren<TMP_Text>();
    }

    public void OpenDataChanger(Business business)
    {
        gameObject.SetActive(true);
        AddListeners(business);
    }

    private void AddListeners(Business business)
    {

        if (business.GetOwnerData() != PhotonPlayerFinder.GetPlayerData(PhotonNetwork.LocalPlayer))
        {
            SetButtonsState(false, false);
            return;
        }

        RemoveOldListeners();

        var businessOwner = business.GetOwnerData();

        if (business.isPawned)
        {
            SetButtonsState(true, false, "Выкупить");
            improveButton.onClick.AddListener(() => photonDataUpdater.UnpawnBusiness(business));
            return;
        }


        if (business is ImprovingBusiness improvingBusiness)
        {
            if(improvingBusiness.GetLevel() == 0 && improvingBusiness.IsFilial)
            {
                SetButtonsState(true, true);
                improveButton.onClick.AddListener(() =>
                {
                    if (turnsManager.RemainedActions > 0)
                    {
                        photonDataUpdater.ImproveBusiness(improvingBusiness);
                        turnsManager.RemainedActions--;

                        ErrorLog.instance.ShowError("Вы уже улучшали поле за этот ход!");
                    }
                });
                sellButton.onClick.AddListener(() => photonDataUpdater.PawnBusiness(business));
                return;
            }
            if(improvingBusiness.GetLevel() > 0)
            {
                SetButtonsState(true, true);

                if(improvingBusiness.GetLevel() == 5) improveButton.gameObject.SetActive(false); 

                improveButton.onClick.AddListener(() =>
                {
                    if (turnsManager.RemainedActions > 0)
                    {
                        photonDataUpdater.ImproveBusiness(improvingBusiness);
                        turnsManager.RemainedActions--;

                        ErrorLog.instance.ShowError("Вы уже улучшали поле за этот ход!");
                    }
                });
                sellButton.onClick.AddListener(() => photonDataUpdater.SellLevel(improvingBusiness));
                return;
            }
        }

        SetButtonsState(false, true);
        sellButton.onClick.AddListener(() => photonDataUpdater.PawnBusiness(business));
    }
    private void SetButtonsState(bool value1, bool value2, string improveButtonName = "Улучшить", string sellButtonName = "Заложить")
    {
        improveButton.gameObject.SetActive(value1);
        sellButton.gameObject.SetActive(value2);
        improveText.text = improveButtonName;
        sellText.text = sellButtonName;
    }
    private void RemoveOldListeners()
    {
        improveButton.onClick.RemoveAllListeners();
        sellButton.onClick.RemoveAllListeners();
    }
}
