using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{

    private float setRandomFloat;

    [Header("Corgi의 생성 시간")]
    [SerializeField] private float secCorgi;
    [Header("Cur의 생성 시간")]
    [SerializeField] private float secCur;
    [Header("셰퍼드의 생성 시간")]
    [SerializeField] private float secShephered;
    //밤의 생성속도 변경을 위한 진짜 생성시간
    private float realTimeCorgi;
    private float realTimeCur;
    private float realTimeShephered;

    private bool isCheckedDogs;

    public void CreateDog()
    {
        StartCoroutine(MakeDogs());
    }
    IEnumerator MakeDogs()
    {
        realTimeCorgi = secCorgi;
        realTimeCur = secCur;
        realTimeShephered = secShephered;
        float corgiTime = 0f;
        float curTime = 0f;
        float shepTime = 0f;
        while (true)
        {
            corgiTime += Time.deltaTime;
            curTime += Time.deltaTime;
            shepTime += Time.deltaTime;

            if (corgiTime >= realTimeCorgi)
            {
                Factory.Instance.GetCorgi();
                corgiTime = Random.Range(-0.3f, 0.3f);
            }

            if (curTime >= realTimeCur)
            {
                Factory.Instance.GetCur();
                curTime = Random.Range(-0.3f, 0.3f);
            }
            if (shepTime >= realTimeShephered)
            {
                Factory.Instance.GetShephered();
                shepTime = Random.Range(-0.3f, 0.3f);
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }

    }

    public void NightCreate()
    {
        realTimeCorgi = secCorgi * 2;
        realTimeCur = secCur * 2;
        realTimeShephered = secShephered * 2;
        StartCoroutine(CheckDogs());
    }
    public void StopAllCoroutine()
    {

        StopAllCoroutines();
        isCheckedDogs = false;
    }

    public void StartCheckDogs()
    {
        StartCoroutine(CheckDogs());
    }


    IEnumerator CheckDogs()
    {
        if (!isCheckedDogs)
        {
            Debug.Log("개체크시작");

            isCheckedDogs = true;
            yield return new WaitForSeconds(2f);

            //밤이 되면 씬의 개들을 찾아 리스트에 넣기
            List<DogBase> dogs = new List<DogBase>(FindObjectsOfType<DogBase>());
            Debug.Log($"체크 카운트:{dogs.Count}");
            while (true)
            {
                //리스트에 넣은 개들의 상태체크(isnight = true라면 가게에 들리지 않는다는 뜻)
                for (int i = dogs.Count - 1; i >= 0; i--)
                {
                    DogBase dog = dogs[i];
                    //개가 null이거나 오브젝트가 꺼진상태거나 isnight가 true인 상태라면
                    if (dog == null || !dog.gameObject.activeSelf || dog.isNight)
                    {
                        //리스트에서 제거
                        Debug.Log($"개가 리스트에서 제거되었습니다.{dog.name}");
                        dogs.RemoveAt(i);
                    }
                }
                //모든 개가 제거되면
                if (dogs.Count <= 0)
                { // 다음날로 이동
                    Debug.Log($"다음날로 이동합니다{dogs.Count}");
                    GameManager.Instance.DayEnd();
                    yield break;
                }

                yield return new WaitForSeconds(1f);
            }
        }
    }


        public void TestDogset()
    {
        Factory.Instance.GetCorgi();

        Factory.Instance.GetCur();

        Factory.Instance.GetShephered();
    }

}
