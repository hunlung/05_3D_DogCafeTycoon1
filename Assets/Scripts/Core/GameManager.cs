using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }


    //날이 끝나면 작동
    public void DayEnd()
    {
        playerControll.enabled = false;
        TimeManager.NextDay();
        TimeManager.CurrentHour = 9;
        TimeManager.CurrentMinute = Random.Range(0, 30);
        itemShopManager.PrepareStore();
        customerManager.StopCreateDogs();
    }
    
    //다음날 영업 시작
    public void DayStart()
    {
        isLastOrder = false;
        player.transform.position = new Vector3(0,player.transform.position.y,0);
        playerControll.enabled = true;
        timeManager.StartTimeCycle();
    }






}
