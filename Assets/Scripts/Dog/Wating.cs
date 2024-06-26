using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wating : MonoBehaviour
{

    private Transform[] watingTransforms;
    private bool[] isUsing;
    private int watingNumber;


    //���� ��ġ �����ϰ� ��ĭ������ ���°� �����ϱ� TODO::
    private void Awake()
    {
        watingTransforms = GetComponentsInChildren<Transform>();
        isUsing = new bool[watingTransforms.Length];
    }

    public Transform CheckWating()
    {
        for (int i = 0; i < watingTransforms.Length; i++)
        {
            if (isUsing[i] == false)
            {
                isUsing[i] = true;
                watingNumber = i;
                return watingTransforms[i].transform;
            }
        }
        Debug.LogWarning("���� �����ϴ�.");
        return null;
    }

    public int CheckWatingNumber()
    {
        return watingNumber;
    }

}
