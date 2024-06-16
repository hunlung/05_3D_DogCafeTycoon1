using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int previousMoney;
    private int money;
    public int Money
    {
        get { return money; }
        set
        {
            if (money != value)
            {
                previousMoney = money;
                money = value;
                int diff = money - previousMoney; // 돈의 변화량 계산
                OnMoneyChange?.Invoke(money, diff);
            }
        }
    }

    public Action<int,int> OnMoneyChange;

    private void Start()
    {
        Money += 20000;
    }


}





