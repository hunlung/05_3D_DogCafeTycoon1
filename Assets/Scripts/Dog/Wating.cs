using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wating : MonoBehaviour
{
    [SerializeField] private Transform[] waitingPositions;
    private List<DogBase> waitingList = new List<DogBase>();
    private const int MAX_WAITING = 7;  // �ִ� ��� ������ ����

    private DogBase currentOrderingDog = null;
    private object orderLock = new object();

    private void Start()
    {
        StartCheckFrontLine();
    }

    public bool CanStartOrder(DogBase dog)
    {
        lock (orderLock)
        {
            return currentOrderingDog == null || currentOrderingDog == dog;
        }
    }

    public void SetCurrentOrderingDog(DogBase dog)
    {
        lock (orderLock)
        {
            currentOrderingDog = dog;
        }
    }

    private IEnumerator CheckFrontOfLineCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f); // 3�ʸ��� üũ

            lock (waitingList)
            {
                if (waitingList.Count > 0)
                {
                    DogBase frontDog = waitingList[0];
                    if (frontDog == null || (frontDog.isProcessing == false && CanStartOrder(frontDog)))
                    {
                        // �� ���� ���� ���� �ֹ� ������ ������ ���
                        RemoveFromWaitingList(frontDog); // �������� ���� �� ó��
                    }
                }
            }
        }
    }

    public void StartCheckFrontLine()
    {
        StopAllCoroutines();
        StartCoroutine(CheckFrontOfLineCoroutine());
    }
    public void StopCheckFrontLine()
    {
        StopAllCoroutines();
    }

    public void AddToWaitingList(DogBase dog)
    {
        if (waitingList.Count < MAX_WAITING)
        {
            waitingList.Add(dog);
            UpdateWaitingPositions();
        }
        else
        {
            Debug.LogWarning("��⿭�� ���� á���ϴ�. �� �̻� ���� �߰��� �� �����ϴ�.");
        }
    }

    public void RemoveFromWaitingList(DogBase dog)
    {
        waitingList.Remove(dog);
        if (dog == currentOrderingDog)
        {
            currentOrderingDog = null;
        }
        UpdateWaitingPositions();
        ProcessNextInLine();  // ��� ���� �� ó��
    }

    public int GetWaitingPosition(DogBase dog)
    {
        int index = waitingList.IndexOf(dog);
        return index >= 0 && index < MAX_WAITING ? index : -1;
    }

    public bool IsNextInLine(DogBase dog)
    {
        return waitingList.Count > 0 && waitingList[0] == dog;
    }

    public Transform CheckWating(int number)
    {
        if (number >= 0 && number < waitingPositions.Length)
        {
            return waitingPositions[number];
        }
        Debug.LogWarning($"��ȿ���� ���� ��� ��ȣ: {number}");
        return null;
    }

    private void UpdateWaitingPositions()
    {
        for (int i = 0; i < waitingList.Count && i < MAX_WAITING; i++)
        {
            waitingList[i].UpdateWaitingNumber(i);
        }
    }


    public void ProcessNextInLine()
    {
        if (waitingList.Count > 0)
        {
            DogBase nextDog = waitingList[0];
            if (nextDog.isProcessing == false && CanStartOrder(nextDog))
            {
                nextDog.isProcessing = false;
                nextDog.StartOrder();
            }
        }
    }


}