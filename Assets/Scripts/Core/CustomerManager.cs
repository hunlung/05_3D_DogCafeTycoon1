using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{

    private float setRandomFloat;

    [Header("Corgi가 나올 확률(싼거 시키는 손님)")]
    [SerializeField] private float perCorgi;
    [Header("Cur가 나올 확률(보통 손님)")]
    [SerializeField] private float perCur;
    [Header("셰퍼드가 나올 확률(비싼거시키는 손님)")]
    [SerializeField] private float perShephered;

    public void CreateDog()
    {
        //TODO :: 하루 일과 코루틴으로 변환시키기, 밤이되면 다음날로 넘어가는게아니라 남은 개 체크해서 0이되면 넘어가게하기
        setRandomFloat = Random.value;
        if (setRandomFloat > 0.4)
        {
            Factory.Instance.GetCorgi();
        }
        else
        {
            Factory.Instance.GetCur();
        }
        if (setRandomFloat > 0.7)
        {
            Factory.Instance.GetShephered();
        }
    }

}
