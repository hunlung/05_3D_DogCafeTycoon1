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
    private TextMeshPro cleaningText;
    private Slider slider;
    GameObject DogFood;

    [SerializeField] private int level;

    private void Start()
    {
        SetCushionClearTime(level);

        isUsing = false;
        isDirty = false;

        Transform canvasChild = transform.GetChild(0); 
        slider = canvasChild.GetChild(0).GetComponent<Slider>();
        dirtyText = canvasChild.GetChild(1).GetComponent<TextMeshPro>();
        cleaningText = canvasChild.GetChild(2).GetComponent<TextMeshPro>();
        DogFood = canvasChild.GetChild(3).gameObject;
    }

    //������ ���� û�ҽð� ����
    private void SetCushionClearTime(int level)
    {
        switch (level)
        {
            case 1: clearTime = 7; break;
            case 2: clearTime = 5; break;
            case 3: clearTime = 3; break;
        }
    }


    //�մ��� ���� ���, ����
    public void UsingCushion()
    {
        if (!isUsing)
        {
        isUsing = true;
            DogFood.SetActive(true);
        }
    }
    public void LeaveCushion()
    {
        if (!isDirty)
        {
        isDirty = true;
        dirtyText.gameObject.SetActive(true);
        }
    }

    //���� û�� ����, ��

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
        dirtyText.gameObject.SetActive(false);
        cleaningText.gameObject.SetActive(true);
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
        cleaningText.gameObject.SetActive(false);
        DogFood.gameObject.SetActive(false);
    }

    private void ClearingCushionBar(float currentTime, int clearTime)
    {
        float currentValue = currentTime / clearTime;
        slider.value = currentValue;
    }

}