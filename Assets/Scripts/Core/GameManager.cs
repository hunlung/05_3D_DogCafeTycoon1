using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{

    Player player;
    public Player Player => player;
    
    PlayerControll playerControll;
    public PlayerControll PlayerControl => playerControll;

    TimeManager timeManager;
    public TimeManager TimeManager => timeManager;

    ItemShopManager itemShopManager;
    public ItemShopManager ItemShopManager => itemShopManager;

    ItemManager itemManager;
    public ItemManager ItemManager => itemManager;

    CustomerManager customerManager;
    public CustomerManager CustomerManager => customerManager;


    public bool isLastOrder = false;

    public Action onDayEnd;
    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Player>();
        playerControll = player.GetComponent<PlayerControll>();
        timeManager = transform.GetChild(0).GetComponent<TimeManager>();
        itemShopManager = transform.GetChild(1).GetComponent<ItemShopManager>();
        itemManager = transform.GetChild(2).GetComponent<ItemManager>();
        customerManager = transform.GetChild(3).GetComponent<CustomerManager>();
        ItemShopManager.SetupUI();
        timeManager.SetUI();
        ItemShopManager.PrepareStore();


    }


    public void StoreLastOrder()
    {
        isLastOrder = true;
        customerManager.NightCreate();
        customerManager.StartCheckDogs();
    }


    //날이 끝나면 작동
    public void DayEnd()
    {
        
        TimeManager.NextDay();
        TimeManager.CurrentHour = 9;
        TimeManager.CurrentMinute = Random.Range(0, 30);
        itemShopManager.PrepareStore();
        customerManager.StopAllCoroutine();
        onDayEnd?.Invoke();
        if(timeManager.Day == 3)
        {
            GameEnd();
        }
    }
    
    //다음날 영업 시작
    public void DayStart()
    {
        isLastOrder = false;
        player.transform.position = new Vector3(0,player.transform.position.y,0);
        playerControll.enabled = true;
        timeManager.StartTimeCycle();
        customerManager.CreateDog();
        player.ResetProfitRecords();
        player.SetLastRecordMoney();
        player.currentProfitIndex = 0;
        PlayerControl.EnableAction();
    }

    private void GameEnd()
    {
        SceneManager.LoadScene("GameEnd");
    }





}
