using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Wallet 
{
    private int money;

    public event Action<int> OnSpendMoney;

    public bool Has(int money)
    {
        if (this.money >= money) return true;
        return false;
    }
    public void AddMoney(int money)
    {
        this.money += money;

        if (this.money < 0)
        {
            this.money = 0;
        }

        OnSpendMoney?.Invoke(this.money);
    }
    public void RemoveMoney(int money)
    {
        this.money -= money;

        OnSpendMoney?.Invoke(this.money);

        if(money < 0)
        {
            throw new InvalidExpressionException();
        }
    }
    public int GetMoney()
    {
        return this.money;
    }
}
