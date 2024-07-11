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
            Debug.Log("��üũ����");

            isCheckedDogs = true;
            yield return new WaitForSeconds(2f);

            //���� �Ǹ� ���� ������ ã�� ����Ʈ�� �ֱ�
            List<DogBase> dogs = new List<DogBase>(FindObjectsOfType<DogBase>());
            Debug.Log($"üũ ī��Ʈ:{dogs.Count}");
            while (true)
            {
                //����Ʈ�� ���� ������ ����üũ(isnight = true��� ���Կ� �鸮�� �ʴ´ٴ� ��)
                for (int i = dogs.Count - 1; i >= 0; i--)
                {
                    DogBase dog = dogs[i];
                    //���� null�̰ų� ������Ʈ�� �������°ų� isnight�� true�� ���¶��
                    if (dog == null || !dog.gameObject.activeSelf || dog.isNight)
                    {
                        //����Ʈ���� ����
                        Debug.Log($"���� ����Ʈ���� ���ŵǾ����ϴ�.{dog.name}");
                        dogs.RemoveAt(i);
                    }
                }
                //��� ���� ���ŵǸ�
                if (dogs.Count <= 0)
                { // �������� �̵�
                    Debug.Log($"�������� �̵��մϴ�{dogs.Count}");
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
