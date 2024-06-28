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
    private void Start()
    {
        Money += 20000;
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


    //주문한게 둘다 없으면 바로 가게 떠나게 하기TODO::
    public void SellItem(ItemBase[] item, int orderCount)
    {
        int soldOutCount = 0;
        for (int i = 0; i < item.Length; i++)
        {
            if (item[i] != null && item[i].remaining >= 1)
            {
                Money += item[i].sellPrice;
                item[i].remaining--;
                TotalSatisfaction += item[i].satisfaction;
                Debug.Log($"아이템 판매 완료, 남은 {item[i].name}의 개수: {item[i].remaining}개 ");
                
            }
            else if (item[i] == null)
            {
                Debug.Log($"item{i}가 없습니다.");
            }
            else
            {
             Debug.Log($"{item[i].name}이 다 떨어졌습니다.");
                soldOutCount++;
            }
            
        }
        //모든 품목이 품절
        if(soldOutCount == orderCount)
        {
            onSoldOut?.Invoke(orderCount);
        }
        //하나라도 팔았다면
        else
        {
            onSell?.Invoke(orderCount);
        }
    }
    public Action<int> onSell;
    public Action<int> onSoldOut;



}





