using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestDogs : TestBase
{

    CustomerManager customerManager;
   
    private void Start()
    {
        customerManager = GameManager.Instance.CustomerManager;
    }

    protected override void Ontest1(InputAction.CallbackContext context)
    {
        customerManager.CreateDog();
        GameManager.Instance.TimeManager.StartTimeCycle();
    }



}
