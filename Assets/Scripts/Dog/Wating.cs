using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wating : MonoBehaviour
{

    [SerializeField] private Transform[] watingTransforms;
    private bool[] isUsing;
    private int watingNumber;


    //���� ��ġ �����ϰ� ��ĭ������ ���°� �����ϱ� TODO::
    private void Awake()
    {
        watingTransforms = GetComponentsInChildren<Transform>();
        isUsing = new bool[watingTransforms.Length - 1];
        for (int i = 0; i < watingTransforms.Length - 1; i++)
        {
            isUsing[i] = false;
        }
    }

    public Transform CheckWating(int number)
    {
        for (int i = 0; i < watingTransforms.Length - 1; i++)
        {
            if (isUsing[i] == false || number == i)
            {
                watingNumber = i;
                isUsing[i] = true;
                return watingTransforms[i + 1].transform;
            }
            
        }
        Debug.LogWarning("���� �����ϴ�.");
        return null;
    }

    public int CheckWatingNumber()
    {
        return watingNumber;
    }
    public void LeaveWating(int number)
    {
        isUsing[number] = false;
    }

}
