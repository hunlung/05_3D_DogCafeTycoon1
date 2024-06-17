using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestCushion : TestBase
{
public  Cushion[] cushion;

    private void Start()
    {
        cushion = new Cushion[10];
        cushion = FindObjectsOfType<Cushion>();
    }

    protected override void Ontest1(InputAction.CallbackContext context)
    {
        for(int i =0; i < cushion.Length; i++)
        {
            cushion[i].UsingCushion();
            Debug.Log($"{i + 1}번째 쿠션 사용함");
        }
        
    }
    protected override void Ontest2(InputAction.CallbackContext context)
    {
        for (int i = 0; i < cushion.Length; i++)
        {
            cushion[i].LeaveCushion();
            Debug.Log($"{i + 1}번째 쿠션 떠남");

        }
    }
    protected override void Ontest3(InputAction.CallbackContext context)
    {
        for (int i = 0; i < cushion.Length; i++)
        {
            cushion[i].ClearingStart();
            Debug.Log($"{i + 1}번째 쿠션 청소시작");

        }
    }

}
