using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestBase : MonoBehaviour
{
    public int seed = -1;
    const int allRandom = -1;
    protected TestInput inputActions;

    private void Awake()
    {
        inputActions = new TestInput();

        if (seed != allRandom) // -1일 때는 완전 랜덤. 그 외에 값은 시드로 설정됨
        {
            UnityEngine.Random.InitState(seed);
        }
    }

    protected virtual void OnEnable()
    {
        inputActions.Test.Enable();
        inputActions.Test._7.performed += Ontest1;
        inputActions.Test._8.performed += Ontest2;
        inputActions.Test._9.performed += Ontest3;
        inputActions.Test._10.performed += Ontest4;
        inputActions.Test._11.performed += Ontest5;
    }

    protected virtual void OnDisable()
    {
        inputActions.Test._11.performed -= Ontest5;
        inputActions.Test._10.performed -= Ontest4;
        inputActions.Test._9.performed -= Ontest3;
        inputActions.Test._8.performed -= Ontest2;
        inputActions.Test._7.performed -= Ontest1;
        inputActions.Test.Disable();
    }


    private void Ontest5(InputAction.CallbackContext context)
    {

    }

    private void Ontest4(InputAction.CallbackContext context)
    {

    }

    private void Ontest3(InputAction.CallbackContext context)
    {

    }

    private void Ontest2(InputAction.CallbackContext context)
    {

    }

    private void Ontest1(InputAction.CallbackContext context)
    {

    }
}
