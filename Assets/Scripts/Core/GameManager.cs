using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    Player player;
    public Player Player => player;

    TimeManager timeManager;
    public TimeManager TimeManager => timeManager;

    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Player>();
        timeManager = transform.GetChild(0).GetComponent<TimeManager>();
    }






}
