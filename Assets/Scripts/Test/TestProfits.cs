using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestProfits : TestBase
{
    protected override void Ontest1(InputAction.CallbackContext context)
    {
        GameManager.Instance.Player.Money += 5002;
        GameManager.Instance.TimeManager.CurrentHour += 1;
        
    }
    protected override void Ontest2(InputAction.CallbackContext context)
    {

    }

    protected override void Ontest3(InputAction.CallbackContext context)
    {


    }

    protected override void Ontest4(InputAction.CallbackContext context)
    {

    }

    protected override void Ontest5(InputAction.CallbackContext context)
    {

    }
}
