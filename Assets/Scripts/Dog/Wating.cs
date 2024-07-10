using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wating : MonoBehaviour
{
    [SerializeField] private Transform[] waitingPositions;
    private List<DogBase> waitingList = new List<DogBase>();
    private const int MAX_WAITING = 7;  // 최대 대기 가능한 개수

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
            yield return new WaitForSeconds(3f); // 3초마다 체크

            lock (waitingList)
            {
                if (waitingList.Count > 0)
                {
                    DogBase frontDog = waitingList[0];
                    if (frontDog == null || (frontDog.isProcessing == false && CanStartOrder(frontDog)))
                    {
                        // 맨 앞의 개가 없고 주문 가능한 상태인 경우
                        RemoveFromWaitingList(frontDog); // 빼버리고 다음 개 처리
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
            Debug.LogWarning("대기열이 가득 찼습니다. 더 이상 개를 추가할 수 없습니다.");
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
        ProcessNextInLine();  // 즉시 다음 개 처리
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
        Debug.LogWarning($"유효하지 않은 대기 번호: {number}");
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