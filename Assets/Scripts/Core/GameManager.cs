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

    PlayerControll playerControll;

    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Player>();
        timeManager = transform.GetChild(0).GetComponent<TimeManager>();
        itemShopManager = transform.GetChild(1).GetComponent<ItemShopManager>();
         playerControll = player.GetComponent<PlayerControll>();
    }



    //날이 끝나면 작동
    public void DayEnd()
    {
        playerControll.enabled = false;
        TimeManager.CurrentHour = 9;
        TimeManager.CurrentMinute = Random.Range(0, 30);
        itemShopManager.PrepareStore();
    }
    
    //다음날 영업 시작
    public void DayStart()
    {
        player.transform.position = new Vector3(0,player.transform.position.y,0);
        playerControll.enabled = true;
        timeManager.StartTimeCycle();
    }






}
