using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class DogBase : RecycleObject
{
    //���� State�� ���� �ִϸ��̼� ��ȭ �ֱ�.PassByDog���� �������� �ٲٱ�
    private enum DogState
    {
        Wait = 0,
        Walk,
        Sit,
        Angry
    }

    [SerializeField] private Item_Dessert likeFood;
    [SerializeField] private Item_Drink likeDrink;
    [SerializeField] private ItemBase[] ChooseItems;
    [SerializeField] private float Speed = 1f;
    [Header("Ȯ�� ����")]
    [SerializeField] float setRandomFloat;
    DogState state;
    private const float GroundLeftX = -25f;
    private const float GroundRightX = 25f;
    private const float GroundMinZ = -8.5f;
    private const float GroundMaxZ = -7.3f;


    //Ȯ����
    [Header("���𰡸� �� Ȯ����")]
    [Range(0f, 1f)]
    [Header("�����ʿ��� �� Ȯ��")]
    [SerializeField] private float goRightProb =0.2f;

    [Header("�������� �� �⺻ Ȯ��")]
    [Range(0f, 1f)]
    [SerializeField] private float goStoreProbBasic = 0.2f;

    [Header("�������� �� ��Ȯ��(�б�����)")]
    [ReadOnly(true)]
    [SerializeField] private float goStoreProb;
    [Header("�����ϴ� ���� ��ų Ȯ��")]
    [Range(0f, 1f)]
    [SerializeField] private float orderLike = 0.4f;


    //private�� �ٲٰ� ���߿� test���� TODO::
    public bool isRight = false;
    public bool goStore = false;
    public bool inStore = false;

    Wating wating;
    Animator animator;
    Player player;
    GameObject store;
    Transform storeMoveTransform;
    Transform dogMoveTransform;

    protected override void OnEnable()
    {
        base.OnEnable();

        GetComponent();
        SetDestination();

    }



    private void GetComponent()
    {
        player = GameManager.Instance.Player;
        animator = GetComponent<Animator>();
        if (store == null)
        {
            store = GameObject.FindWithTag("MergedStore");
            storeMoveTransform = store.transform.GetChild(4);
        }
        wating = FindAnyObjectByType<Wating>();
        ChooseItems = new ItemBase[5];
    }

    private void SetDestination()
    {
        setRandomFloat = Random.value;
        if (setRandomFloat <= goRightProb)
        {
            gameObject.transform.position = new Vector3(GroundRightX - 1, 0.2f, Random.Range(GroundMinZ, GroundMaxZ));
            isRight = true;
            Debug.Log("�����ʽ�ŸƮ");
        }
        //���� �����ʿ��� ���� �����
        else
        {
            gameObject.transform.position = new Vector3(GroundLeftX + 1, 0.2f, Random.Range(GroundMinZ, GroundMaxZ));
            isRight = false;
            Debug.Log("���ʽ�ŸƮ");
        }
        setRandomFloat = Random.value;
        //�α⵵�� ���� Ȯ�� �����ϱ� TODO::�ִ� 40%
        //totalProbability =  player.TotalSatisfaction;
        ChangeState(DogState.Walk);
        if (setRandomFloat >= goStoreProbBasic + goStoreProb)
        {
            goStore = false;
            Debug.Log("����");

        }
        else
        {
            goStore = true;
            Debug.Log("���Է�");
        }
    }



    private void Update()
    {
        if (!inStore)
        {

            switch (goStore)
            {
                case false:
                    PassBy();
                    break;
                case true:
                    GoStore();
                    break;

            }
        }
        else
        {

        }
        //���� ����, ������ ���� ���� ��
        if (transform.position.x <= GroundLeftX || transform.position.x >= GroundRightX)
        {
            gameObject.SetActive(false);
        }
    }
    //�ټ���
    IEnumerator WaitingOrder()
    {
        int watingNumber = 5;
        while(watingNumber >= 1)
        {
   
            transform.position = Vector3.MoveTowards(transform.position, wating.CheckWating().position, Time.deltaTime * Speed);
            watingNumber = wating.CheckWatingNumber();
            Debug.Log($"���� ����ȣ: {watingNumber}");
        yield return new WaitForSeconds(1f);
        }
        Debug.Log("��� ��, �ֹ����� �Ѿ");
        StartCoroutine(OrderAndWating());

    }

    //�ֹ��ϰ� ��ٸ���
    IEnumerator OrderAndWating()
    {
        int randomInt;
        bool isOrder = false;
        int orderCount = 0;
        float thoughtTime = Random.Range(0.2f, 5f);
        Debug.Log("�ֹ� ������");
        yield return new WaitForSeconds(thoughtTime);
        Debug.Log("�ֹ���");
        while (isOrder == false)
        {
            setRandomFloat = Random.value;
            if (setRandomFloat <= orderLike)
            {
                //�����ϴ� ��Ʈ �ֹ��ϰ�
                ChooseItems[0] = likeFood;
                ChooseItems[1] = likeDrink;
                orderCount++;
                isOrder = true;
                Debug.Log($"�����ϴ� ���� �ֹ� �ֹ�����: {ChooseItems[0]},{ChooseItems[1]}");
                //��
                break;
            }
            else
            {
            setRandomFloat = Random.value;
                //���� ��Ű��
                if(setRandomFloat <= 0.3f)
                {
                    randomInt = Random.Range(0,4);
                    ChooseItems[0] = ItemManager.Instance.GetItemByIndex<Item_Dessert>(randomInt);
                    orderCount++;
                    isOrder = true;
                    Debug.Log($"���� �ֹ�: {ChooseItems[0]}");

                }
                //���� ��Ű��
                setRandomFloat = Random.value;
                if(setRandomFloat <= 0.5f) 
                {
                    randomInt = Random.Range(0, 4);
                    ChooseItems[1] = ItemManager.Instance.GetItemByIndex<Item_Drink>(randomInt);
                    orderCount++;
                    isOrder = true;
                    Debug.Log($"���ǰ� �ֹ�: {ChooseItems[1]}");

                }
                //����
                setRandomFloat = Random.value;
                if(setRandomFloat <= 0.15f)
                {
                    ChooseItems[2] = ItemManager.Instance.GetItemByIndex<Item_Goods>(0);
                    orderCount++;
                    isOrder = true;
                    Debug.Log($"���� �ֹ�: {ChooseItems[2]}");
                }
            }//TODO:: ���� ó������ ��緯�³༮�� �ุ������

            onOrder?.Invoke(ChooseItems);
                yield return null;
        }

    }
    public System.Action<ItemBase[]> onOrder;
    //=============================������
    //�մ�
    private void GoStore()
    {
        //TODO:: ���� ��ġ��
        dogMoveTransform = storeMoveTransform.GetChild(0);
        transform.SetPositionAndRotation(Vector3.MoveTowards(transform.position, dogMoveTransform.position, Time.deltaTime * Speed), Quaternion.LookRotation(new Vector3(-dogMoveTransform.position.x, 0f, 0f)));
        if (transform.position == dogMoveTransform.position)
        {
            inStore = true;
            StopAllCoroutines();
            StartCoroutine(WaitingOrder());
            Debug.Log("���� ����");
        }
    }

    //����
    private void PassBy()
    {
        switch (isRight)
        {
            case false:
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(GroundRightX + 1, 0.2f, transform.position.z), Time.deltaTime * Speed);
                transform.rotation = Quaternion.LookRotation(new Vector3(GroundRightX, 0f, 0f));
                break;
            case true:
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(GroundLeftX - 1, 0.2f, transform.position.z), Time.deltaTime * Speed);
                transform.rotation = Quaternion.LookRotation(new Vector3(GroundLeftX, 0f, 0f));
                break;
        }
    }

    private void ChangeState(DogState value)
    {
        switch (value)
        {
            case DogState.Wait:
                state = DogState.Wait;
                animator.SetInteger("AnimationID", 0);
                break;
            case DogState.Walk:
                state = DogState.Walk;
                animator.SetInteger("AnimationID", 1);
                break;
            case DogState.Sit:
                state = DogState.Sit;
                animator.SetInteger("AnimationID", 2);
                break;
            case DogState.Angry:
                state = DogState.Angry;
                animator.SetInteger("AnimationID", 3);
                break;
        }
    }

}
