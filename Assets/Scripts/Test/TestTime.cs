using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestTime : TestBase
{

    TimeManager timeManager;

    private void Start()
    {
        timeManager = FindAnyObjectByType<TimeManager>();
    }

    protected override void Ontest1(InputAction.CallbackContext context)
    {
        timeManager.TimeChanging();
    }
    protected override void Ontest2(InputAction.CallbackContext context)
    {
        timeManager.Hour += 1;
    }

}
