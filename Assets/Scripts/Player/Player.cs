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

    public Action<int, int> OnMoneyChange;
    DogBase customer;
    private void Start()
    {
        Money += 20000;
        customer.onOrder += SellItem;
    }

    private float totalSatisfaction;
    public float TotalSatisfaction
    {
        get { return totalSatisfaction; }
        set
        {
            if (totalSatisfaction != value)
            {
                totalSatisfaction = value;
            }
        }
    }


    private void SellItem(ItemBase[] item)
    {
        for (int i = 0; i < item.Length; i++)
        {
            if (item[i] != null && item[i].remaining >= 1)
            {
                Money += item[i].sellPrice;
                item[i].remaining--;
                TotalSatisfaction += item[i].satisfaction;
                onSell?.Invoke();
                Debug.Log($"아이템 판매 완료, 남은 {item[i].name}의 개수: {item[i].remaining}개 ");
                
            }
            else if (item[i] == null)
            {
                Debug.Log($"item{i}가 없습니다.");
            }
            else
            {
             Debug.Log($"{item[i].name}이 다 떨어졌습니다.");   
            }


        }
    }
    public Action onSell;




}





