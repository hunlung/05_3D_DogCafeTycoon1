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
                int diff = money - previousMoney; // ���� ��ȭ�� ���
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
                Debug.Log($"������ �Ǹ� �Ϸ�, ���� {item[i].name}�� ����: {item[i].remaining}�� ");
                
            }
            else if (item[i] == null)
            {
                Debug.Log($"item{i}�� �����ϴ�.");
            }
            else
            {
             Debug.Log($"{item[i].name}�� �� ���������ϴ�.");   
            }


        }
    }
    public Action onSell;




}





