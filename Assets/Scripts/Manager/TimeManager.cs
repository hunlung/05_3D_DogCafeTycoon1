using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    [SerializeField] private Material skybox;

    private int hour = 0;
    public int Hour
    {
        get { return hour; }
        set
        {
            if (hour != value)
            {
                hour = value;
                if (hour >= nightTime && isNight == false)
                {
                    RenderSettings.skybox = skybox;
                    isNight = true;
                }
                if(hour >= endTime)
                {
                    //영업시간이 끝났음을 알리기
                }

            }
        }
    }
    private int startTime = 12;
    private int endTime = 20;
    int nightTime;

    bool isNight = false;

    private int minit;
    const int second = 1;

    private void Awake()
    {
        nightTime = endTime - 2;
    }

    public void TimeChanging()
    {
        StopAllCoroutines();
        StartCoroutine(TimeCoroutine());
    }

    IEnumerator TimeCoroutine()
    {

        Hour = startTime;
        minit = 0;
        while (Hour <= endTime)
        {
            minit += second;

            if (minit >= 60)
            {
                minit = 0;
                Hour += 1;
            }
            yield return new WaitForSeconds(1f);

        }

    }



}
