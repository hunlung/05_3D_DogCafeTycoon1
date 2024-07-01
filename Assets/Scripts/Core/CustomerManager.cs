using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{

    private float setRandomFloat;

    [Header("Corgi�� ���� Ȯ��(�Ѱ� ��Ű�� �մ�)")]
    [SerializeField] private float perCorgi;
    [Header("Cur�� ���� Ȯ��(���� �մ�)")]
    [SerializeField] private float perCur;
    [Header("���۵尡 ���� Ȯ��(��ѰŽ�Ű�� �մ�)")]
    [SerializeField] private float perShephered;

    public void CreateDog()
    {
        //TODO :: �Ϸ� �ϰ� �ڷ�ƾ���� ��ȯ��Ű��, ���̵Ǹ� �������� �Ѿ�°Ծƴ϶� ���� �� üũ�ؼ� 0�̵Ǹ� �Ѿ���ϱ�
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
