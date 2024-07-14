using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int[] profits = new int[5];
    public int[] recorededTime = new int[5];
    private int lastRecordedMoney = 0;
    public int currentProfitIndex = 0;


    private int previousMoney;
    private int doggumSellCount;
    //���� 50�� �ȸ� Ȳ�ݰ��� �ر�
    private int DogGumSellCount
    {
        get { return doggumSellCount; }
        set
        {
            if (doggumSellCount != value)
            {
                doggumSellCount = value;
                if (DogGumSellCount >= 50)
                {
                    ItemManager.Instance.GetItemByIndex<Item_Dessert>(3).itemCantBuy = false;
                    GameManager.Instance.ItemShopManager.GoldDogGumActivate();
                }
            }

        }
    }
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
    private void Start()
    {
        Money += 20000;
        DontDestroyOnLoad(gameObject);
        GameManager.Instance.TimeManager.onHourChanged += RecordProfit;
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
                if (item[i].ItemType == ItemType.dessert && item[i].itemCode == 2)
                {
                    DogGumSellCount++;
                    Debug.Log($"���� �Ǹ� +1,���ǸŰ���{DogGumSellCount}");
                }
                Debug.Log($"������ �Ǹ� �Ϸ�, ���� {item[i].name}�� ����: {item[i].remaining}�� ");

            }
            else if (item[i] == null)
            {

            }
            else
            {
                Debug.Log($"{item[i].name}�� �� ���������ϴ�.");
                soldOutCount++;
            }

        }

        onSell?.Invoke(orderCount);
    }

    public Action<int> onSell;
    public Action<int> onSoldOut;
    public Action<int> onRecorded;

    //�ð��� ���� ���
    public void RecordProfit()
    {
        int currentProfit = Money - lastRecordedMoney;
        profits[currentProfitIndex] = currentProfit;
        recorededTime[currentProfitIndex] = GameManager.Instance.TimeManager.CurrentHour;
        currentProfitIndex = (currentProfitIndex + 1) % 5;

        lastRecordedMoney = Money; // ���� ���� ������ ��ϵ� ������ ������Ʈ
        onRecorded?.Invoke(currentProfit);
    }
    //�� ���۽� ����
    public void SetLastRecordMoney()
    {
        lastRecordedMoney = Money;
    }
    //�� ����� �ʱ�ȭ
    public void ResetProfitRecords()
    {
        for (int i = 0; i < profits.Length; i++)
        {
            profits[i] = 0;
        }
        currentProfitIndex = 0;
        lastRecordedMoney = Money;

    }


}





