using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusinessFinder 
{
    public List<BusinessConfig> GetPlayersBusinesses(List<Business> names)
    {
        List<BusinessConfig> list = new();
        for (int i = 0;i < names.Count;i++)
        {
            list.Add(names[i].GetConfig());
        }
        return list;
    }
    public List<T> FindByType<T>(List<Business> businesses)
    {
        List<T> list = new();
        for(int i = 0;i < businesses.Count; i++)
        {
            if (businesses[i] is T business)
            {
                list.Add(business);
            }
        }
        return list;
    }
    public (List<ImprovingBusiness> findingBusiness, bool isFillial) FindImprovingById(List<Business> businesses,int id)
    {
        List<ImprovingBusiness> list = new();
        for (int i = 0; i < businesses.Count; i++)
        {
            if (businesses[i] is ImprovingBusiness business) 
            {
                if(business.BusinessId == id)
                    list.Add(business);
            }
        }
        return (list,CheckFillial(id,list.Count));
    }
    private bool CheckFillial(int id, int count)
    { 
        if (id == 0 && count == 2) return true;
        if (id == 1 && count == 3) return true;
        if (id == 2 && count == 3) return true;
        if (id == 3 && count == 3) return true;
        if (id == 4 && count == 3) return true;
        if (id == 5 && count == 3) return true;
        if (id == 6 && count == 3) return true;
        if (id == 7 && count == 2) return true;
        return false;
    }
}
