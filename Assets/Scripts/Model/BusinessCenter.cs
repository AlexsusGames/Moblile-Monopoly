using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BusinessCenter 
{
    private Dictionary<Business, int> pawnedBusinesses;
    private int pawnTerm = 15;

    public BusinessCenter()
    {
        pawnedBusinesses = new Dictionary<Business, int>();
    }

    public void PawnBusiness(Business business)
    {
        var businessOwner = business.GetOwnerData();
        if (businessOwner != null)
        {
            pawnedBusinesses[business] = pawnTerm;
            business.isPawned = true;
            business.Visual.SetPawn(pawnTerm);
            businessOwner.PlayerWallet.AddMoney(business.GetConfig().SoldPrise);
            ChatLog.instance.AddMessage(businessOwner, $"заложил поле: '{business.GetConfig().BusinessName}'");
        }
        else throw new System.Exception("This business doens't have owner");
    }
    public void UnpawnBusiness(Business business)
    {
        var businessOwner = business.GetOwnerData();
        var playersWallet = businessOwner.PlayerWallet;
        var payment = business.GetConfig().BuyoutPrice;
        if (playersWallet.Has(payment))
        {
            business.isPawned = false;
            playersWallet.RemoveMoney(payment);
            pawnedBusinesses.Remove(business);
            business.Visual.SetPawn(0);
            ChatLog.instance.AddMessage(businessOwner, $"выкупил поле: '{business.GetConfig().BusinessName}'");
        }
        else ErrorLog.instance.ShowError("Не хватает денег!");
    }

    public void OnCircleComplated()
    {
        List<Business> list = new();
        List<Business> keyCopy = pawnedBusinesses.Keys.ToList();
        foreach(var business in keyCopy)
        {
            if (business.isPawned)
            {
                pawnedBusinesses[business] -= 1;
                int term = pawnedBusinesses[business];
                if (term == 0)
                {
                    ConfiscateBusiness(business);
                    list.Add(business);
                }
                business.Visual.SetPawn(term);
            }
        }
        for (int i = 0; i < list.Count; i++)
        {
            pawnedBusinesses.Remove(list[i]);
        }
    }
    private void ConfiscateBusiness(Business business)
    {
        var businessOwner = business.GetOwnerData();
        var businessName = business.GetConfig().BusinessName;

        ChatLog.instance.AddMessage(businessOwner,$"Поле '{businessName}' конфисковано!");
        business.SetStandart();
    }
}
