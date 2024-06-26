using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wating : MonoBehaviour
{

    private Transform[] watingTransforms;
    private bool[] isUsing;
    private int watingNumber;


    //현재 위치 저장하고 한칸앞으로 가는것 구현하기 TODO::
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
        Debug.LogWarning("줄이 없습니다.");
        return null;
    }

    public int CheckWatingNumber()
    {
        return watingNumber;
    }

}
