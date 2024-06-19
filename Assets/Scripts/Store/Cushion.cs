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
    [SerializeField] private AnimationCurve ColorAnimation;
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
            StartCoroutine(DirtyTextRainbow());
        }
    }
    IEnumerator DirtyTextRainbow()
    {
        dirtyText.gameObject.SetActive(true);
        float curTime = 0;
        int SetColorNum = 0;
        Color color = new Color(0f, 0f, 0f, 1f);
        while (true)
        {
            curTime += Time.unscaledDeltaTime;
            if (SetColorNum == 0)
            {
                color.r = ColorAnimation.Evaluate(curTime);
                dirtyText.color = color;
                if (color.r >= 0.9f)
                {
                    
                    SetColorNum += 1;
                    color.g = Random.Range(0, 1f);
                    curTime = 0;
                }
            }
            else if (SetColorNum == 1)
            {
                color.b = ColorAnimation.Evaluate(curTime);
                dirtyText.color = color;
                if (color.b >= 0.9f)
                {
                    SetColorNum += 1;
                    color.r = Random.Range(0, 1f);
                    curTime = 0;
                }
            }
            else
            {
                color.g = ColorAnimation.Evaluate(curTime);
                dirtyText.color = color;
                if (color.g >= 0.9f)
                {
                    SetColorNum = 0;
                    color.b = Random.Range(0, 1f);
                    curTime = 0;
                }
            }
            

            yield return new WaitForSeconds(Time.unscaledDeltaTime);
        }

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
        dirtyText.gameObject.SetActive(false);
        cleaningText.gameObject.SetActive(true);
        float currentTime = 0f;
        float clearingTextChnageTime = 0f;
        int clearingTextChanger = 0;
        isClearing = true;
        while (currentTime < clearTime)
        {
            currentTime += Time.deltaTime;
            ClearingCushionBar(currentTime, clearTime);
            clearingTextChnageTime += Time.deltaTime;
            if (clearingTextChnageTime > 1f)
            {
                clearingTextChanger++;
                clearingTextChnageTime = 0f;
            }
            switch (clearingTextChanger)
            {
                case 0: cleaningText.text = "청소중."; break;
                case 1: cleaningText.text = "청소중.."; break;
                case 2: cleaningText.text = "청소중..."; break;
            }
            clearingTextChanger %= 3;
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
