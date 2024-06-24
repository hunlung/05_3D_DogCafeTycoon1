using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestPassBy : TestBase
{
    PasserByManager passerByManager;
    private void Start()
    {
        passerByManager = FindAnyObjectByType<PasserByManager>();
    }

    protected override void Ontest1(InputAction.CallbackContext context)
    {
        passerByManager.MakePassbyDog();
    }
}
