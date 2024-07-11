using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{

    Player player;
    public Player Player => player;

    TimeManager timeManager;
    public TimeManager TimeManager => timeManager;

    ItemShopManager itemShopManager;
    public ItemShopManager ItemShopManager => itemShopManager;

    ItemManager itemManager;
    public ItemManager ItemManager => itemManager;

    CustomerManager customerManager;
    public CustomerManager CustomerManager => customerManager;

    PlayerControll playerControll;
    public bool isLastOrder = false;

    public Action onDayEnd;
    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Player>();
        timeManager = transform.GetChild(0).GetComponent<TimeManager>();
        itemShopManager = transform.GetChild(1).GetComponent<ItemShopManager>();
        itemManager = transform.GetChild(2).GetComponent<ItemManager>();
        playerControll = player.GetComponent<PlayerControll>();
        customerManager = transform.GetChild(3).GetComponent<CustomerManager>();
    }

    public void StoreLastOrder()
    {
        isLastOrder = true;
        customerManager.NightCreate();
        customerManager.StartCheckDogs();
    }


    //���� ������ �۵�
    public void DayEnd()
    {
        playerControll.enabled = false;
        TimeManager.NextDay();
        TimeManager.CurrentHour = 9;
        TimeManager.CurrentMinute = Random.Range(0, 30);
        itemShopManager.PrepareStore();
        customerManager.StopAllCoroutine();
        onDayEnd?.Invoke();
    }
    
    //������ ���� ����
    public void DayStart()
    {
        isLastOrder = false;
        player.transform.position = new Vector3(0,player.transform.position.y,0);
        playerControll.enabled = true;
        timeManager.StartTimeCycle();
    }






}
