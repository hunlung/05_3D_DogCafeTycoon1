using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wating : MonoBehaviour
{

    [SerializeField] private Transform[] watingTransforms;
    private bool[] isUsing;
    private int watingNumber;


    //현재 위치 저장하고 한칸앞으로 가는것 구현하기 TODO::
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
        Debug.LogWarning("줄이 없습니다.");
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
