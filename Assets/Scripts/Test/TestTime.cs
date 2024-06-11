using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestTime : TestBase
{

    TimeManager timeManager;
    Player player;



    private void Start()
    {
        timeManager = FindAnyObjectByType<TimeManager>();
        player = GameManager.Instance.Player;
    }

    protected override void Ontest1(InputAction.CallbackContext context)
    {
        timeManager.StartTimeCycle();
    }
    protected override void Ontest2(InputAction.CallbackContext context)
    {
        timeManager.CurrentHour += 1;
    }

    protected override void Ontest3(InputAction.CallbackContext context)
    {
        timeManager.CurrentMinute += 5;

    }

    protected override void Ontest4(InputAction.CallbackContext context)
    {
        player.Money += 500;
    }

}
