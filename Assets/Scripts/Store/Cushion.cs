using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cushion : MonoBehaviour
{
    private bool isUsing;
    private bool isDirty;
    private bool isClearing;

    private int clearTime;


    private TextMeshPro dirtyText;
    private Slider slider;

    [SerializeField] private int level;

    private void Start()
    {
        SetCushionClearTime(level);

        isUsing = false;
        isDirty = false;

        Transform canvasChild = transform.GetChild(0); 
        slider = canvasChild.GetChild(0).GetComponent<Slider>();
        dirtyText = canvasChild.GetChild(1).GetComponent<TextMeshPro>();
    }

    //레벨에 따른 청소시간 감소
    private void SetCushionClearTime(int level)
    {
        switch (level)
        {
            case 1: clearTime = 7; break;
            case 2: clearTime = 5; break;
            case 3: clearTime = 3; break;
        }
    }


    //손님이 쿠션 사용, 떠남
    public void UsingCushion()
    {
        isUsing = true;
    }
    public void LeaveCushion()
    {
        isDirty = true;
        dirtyText.gameObject.SetActive(true);
    }

    //쿠션 청소 시작, 끝

    public void ClearingStart()
    {
        if (isUsing && isDirty && !isClearing)
        {
            StopAllCoroutines();
            StartCoroutine(ClearingCushion());
        }
    }
    IEnumerator ClearingCushion()
    {
        slider.gameObject.SetActive(true);
        float currentTime = 0f;
        isClearing = true;
        while (currentTime < clearTime)
        {
            currentTime += Time.deltaTime;
            ClearingCushionBar(currentTime, clearTime);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        ClearingEnd();
    }

    private void ClearingEnd()
    {
        isUsing = false;
        isDirty = false;
        isClearing = false;
        slider.gameObject.SetActive(false);
        dirtyText.gameObject.SetActive(false);
    }

    private void ClearingCushionBar(float currentTime, int clearTime)
    {
        float currentValue = currentTime / clearTime;
        slider.value = currentValue;
    }

}
