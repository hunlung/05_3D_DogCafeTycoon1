using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestDogs : TestBase
{

    DogBase dog;
   
    private void Start()
    {
        dog = FindAnyObjectByType<DogBase>();
    }

    protected override void Ontest1(InputAction.CallbackContext context)
    {
        dog.goStore = true;
        Debug.Log("���Է� ������");
    }

    protected override void Ontest2(InputAction.CallbackContext context)
    {
        dog.goStore = false;
        Debug.Log("�������� ������");
    }

    protected override void Ontest3(InputAction.CallbackContext context)
    {
        dog.isRight = true;
        Debug.Log("������ ��ŸƮ�� �����");
    }

    protected override void Ontest4(InputAction.CallbackContext context)
    {
        dog.isRight = false;
        Debug.Log("���� ��ŸƮ�� �����");

    }

    protected override void Ontest5(InputAction.CallbackContext context)
    {
        dog.inStore = false;
        Debug.Log("���Ե�������");
    }

}
