using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{

    private float setRandomFloat;

    [Header("Corgi�� ���� �ð�")]
    [SerializeField] private float secCorgi;
    [Header("Cur�� ���� �ð�")]
    [SerializeField] private float secCur;
    [Header("���۵��� ���� �ð�")]
    [SerializeField] private float secShephered;
    //���� �����ӵ� ������ ���� ��¥ �����ð�
    private float realTimeCorgi;
    private float realTimeCur;
    private float realTimeShephered;

    public void CreateDog()
    {
        StartCoroutine(MakeDogs());
    }
    IEnumerator MakeDogs()
    {
        realTimeCorgi = secCorgi;
        realTimeCur = secCur;
        realTimeShephered = secShephered;
        float corgiTime =0f;
        float curTime =0f;
        float shepTime = 0f;
        while (true)
        {
            corgiTime += Time.deltaTime;
            curTime += Time.deltaTime;
            shepTime += Time.deltaTime;

            if(corgiTime >= realTimeCorgi)
            {
                Factory.Instance.GetCorgi();
                corgiTime = Random.Range(-0.3f,0.3f);
            }
            
            if(curTime >= realTimeCur)
            {
                Factory.Instance.GetCur();
                curTime = Random.Range(-0.3f,0.3f);
            }
            if(shepTime >= realTimeShephered)
            {
                Factory.Instance.GetShephered();
                shepTime = Random.Range(-0.3f,0.3f);
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }

    }

    public void NightCreate()
    {
        realTimeCorgi = secCorgi *2;
        realTimeCur = secCur *2;
        realTimeShephered = secShephered * 2;
        StartCoroutine(CheckDogs());
    }
    public void StopCreateDogs()
    {
        StopAllCoroutines();
    }

    //TODO:: isNight�� false�� ������ ���� �ʹ����� ������� �������� �̵��ؾ���.
    IEnumerator CheckDogs()
    {
        yield return new WaitForSeconds(2f);
        DogBase[] dogs;
        
    }


    public void TestDogset()
    {
        Factory.Instance.GetCorgi();

        Factory.Instance.GetCur();

        Factory.Instance.GetShephered();
    }

}
