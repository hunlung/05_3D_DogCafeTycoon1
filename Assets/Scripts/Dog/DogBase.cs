using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DogBase : RecycleObject
{
    //���� State�� ���� �ִϸ��̼� ��ȭ �ֱ�.PassByDog���� �������� �ٲٱ�
    //TODO:: Dog�� �������� ������ ���� Wait���� �ٲٰ�, �����̸� ���� move�� �ٲٱ�
    //TODO:: �� ���� �ø���, ���� ������ �������ϱ�
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
    [SerializeField] private float goRightProb = 0.2f;

    [Header("�������� �� �⺻ Ȯ��")]
    [Range(0f, 1f)]
    [SerializeField] private float goStoreProbBasic = 0.2f;

    [Header("�������� �� ��Ȯ��(�б�����)")]
    [ReadOnly(true)]
    [SerializeField] private float goStoreProb;
    [Header("�����ϴ� ���� ��ų Ȯ��")]
    [Range(0f, 1f)]
    [SerializeField] private float orderLike = 0.4f;

    int orderCount = 0;
    int watingNumber = 5;

    //private�� �ٲٰ� ���߿� test���� TODO::
    private bool isRight = false;
    private bool goStore = false;
    private bool inStore = false;
    private bool isWaiting = false;
    private bool goCushion = false;
    private bool isLeave = false;
    private bool outStore = false;

    private bool usedCushion = false;
    public bool isNight = false;

    ItemShopManager itemshopManager;
    GameObject dogQuestionMark;
    Wating wating;
    Animator animator;
    Player player;
    public Cushion[] cushions;
    Cushion usingCushion;
    GameObject store;
    Transform storeMoveTransform;
    Transform dogMoveTransform;

    private void Awake()
    {
        GetComponents();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Initialize();


        SetDestination();
    }

    //�ʱ�ȭ �۾�
    private void Initialize()
    {
        isRight = false;
        goStore = false;
        inStore = false;
        isWaiting = false;
        goCushion = false;
        isLeave = false;
        outStore = false;
        usedCushion = false;
        isNight = false;
        StopAllCoroutines();
        dogQuestionMark.SetActive(false);
        watingNumber = 5;
        if (store == null)
        {
            store = GameObject.FindWithTag("MergedStore");
            storeMoveTransform = store.transform.GetChild(4);
        }
        if (GameManager.Instance.isLastOrder)
        {
            isNight = true;
        }
    }

    private void Start()
    {
        player = GameManager.Instance.Player;
        player.onSell += FindCushion;
        cushions = FindObjectsOfType<Cushion>();
        likeFood = ItemManager.Instance.GetItemByIndex<Item_Dessert>(likeFood.itemCode);
        likeDrink = ItemManager.Instance.GetItemByIndex<Item_Drink>(likeDrink.itemCode);
    }


    private void GetComponents()
    {
        animator = GetComponent<Animator>();
        wating = FindAnyObjectByType<Wating>();
        ChooseItems = new ItemBase[5];
        dogQuestionMark = transform.GetChild(2).GetChild(0).gameObject;
    }

    private void SetDestination()
    {
        setRandomFloat = Random.value;
        if (setRandomFloat <= goRightProb)
        {
            gameObject.transform.position = new Vector3(GroundRightX - 1, 0.2f, Random.Range(GroundMinZ, GroundMaxZ));
            isRight = true;
        }
        //���� �����ʿ��� ���� �����
        else
        {
            gameObject.transform.position = new Vector3(GroundLeftX + 1, 0.2f, Random.Range(GroundMinZ, GroundMaxZ));
            isRight = false;
        }
        setRandomFloat = Random.value;
        ChangeState(DogState.Walk);
        //������ 200�϶� �ִ�ġ(�ִ� 20%+)
        if (player != null)
        {
            goStoreProb = player.TotalSatisfaction * 0.001f;
        }
        else
        {
            goStoreProb = 0.05f;
        }
        Mathf.Min(goStoreProb, 0.2f);
        //Ȯ�����̰ų� ��Ʈ���� ���� ���Է� ����������.
        if (setRandomFloat >= goStoreProbBasic + goStoreProb || isNight)
        {
            goStore = false;

        }
        else
        {
            goStore = true;
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
        else if (!isWaiting)
        {
            transform.position = Vector3.MoveTowards(transform.position, dogMoveTransform.position, Time.deltaTime * Speed);
        }
        else if (!goCushion)
        {
            transform.position = Vector3.MoveTowards(transform.position, usingCushion.transform.position, Time.deltaTime * Speed);
            StartCoroutine(SitCushion());
        }
        else if (isLeave)
        {
            StartCoroutine(LeaveStore());
        }
        else
        {

        }

        //���� ����, ������ ���� ���� ��
        if (transform.position.x <= GroundLeftX || transform.position.x >= GroundRightX)
        {
            StopAllCoroutines();
            gameObject.SetActive(false);
        }
    }

    IEnumerator SitCushion()
    {
        usingCushion.UsingCushion();
        if (transform.position == usingCushion.transform.position)
        {
            ChangeState(DogState.Sit);
            goCushion = true;
            yield return new WaitForSeconds(Random.Range(7, 20));
            isLeave = true;
            usingCushion.LeaveCushion();
        }

    }



    //�ټ���
    IEnumerator WaitingOrder()
    {
        //��Ʈ���� ���Ŀ� ���� �ֹ��� ��������.
        if (GameManager.Instance.isLastOrder)
        {
            LeaveStore();
        }
        ChangeState(DogState.Wait);
        dogMoveTransform = wating.CheckWating(watingNumber);
        watingNumber = wating.CheckWatingNumber();
        while (watingNumber >= 1 || transform.position != dogMoveTransform.position)
        {

            yield return new WaitForSeconds(1f);
        }
        StartCoroutine(OrderAndWating());

    }

    //�ֹ��ϰ� ��ٸ���
    IEnumerator OrderAndWating()
    {
        int randomInt;
        bool isOrder = false;
        float thoughtTime = Random.Range(0.2f, 2f);
        int recursionCount = 0;
        float leaveTime = 8f;
        orderCount = 0;
        dogQuestionMark.SetActive(true);
        yield return new WaitForSeconds(thoughtTime);
        dogQuestionMark.SetActive(false);

        while (isOrder == false && recursionCount < 4)
        {
            setRandomFloat = Random.value;
            if (setRandomFloat <= orderLike)
            {
                //�����ϴ� ��Ʈ �ֹ��ϰ�
                ChooseItems[0] = likeFood;
                ChooseItems[1] = likeDrink;
                if (!ChooseItems[0].itemCantBuy && ChooseItems[0].remaining >= 1
                    && !ChooseItems[1].itemCantBuy && ChooseItems[1].remaining >= 1)
                {
                    isOrder = true;
                    orderCount = 2;
                    //��
                    break;
                }
            }
            else
            {
                setRandomFloat = Random.value;
                //���� ��Ű��
                if (setRandomFloat <= 0.3f)
                {
                    randomInt = Random.Range(0, 4);
                    ChooseItems[0] = ItemManager.Instance.GetItemByIndex<Item_Dessert>(randomInt);
                    if (!ChooseItems[0].itemCantBuy && ChooseItems[0].remaining >= 1)
                    {
                        isOrder = true;
                        orderCount++;
                    }

                }
                //���� ��Ű��
                setRandomFloat = Random.value;
                if (setRandomFloat <= 0.5f)
                {
                    randomInt = Random.Range(0, 4);
                    ChooseItems[1] = ItemManager.Instance.GetItemByIndex<Item_Drink>(randomInt);
                    if (!ChooseItems[1].itemCantBuy && ChooseItems[1].remaining >= 1)
                    {
                        isOrder = true;
                        orderCount++;
                    }

                }
                //����
                setRandomFloat = Random.value;
                if (setRandomFloat <= 0.15f)
                {
                    ChooseItems[2] = ItemManager.Instance.GetItemByIndex<Item_Goods>(0);
                    if (ChooseItems[2].remaining >= 1)
                    {
                        isOrder = true;
                        orderCount++;
                    }
                }

            }

            //�ƹ��͵� �Ȼ�ٸ� ��
            if (!isOrder && recursionCount > 2)
            {
                ChooseItems[3] = ItemManager.Instance.GetItemByIndex<Item_Medicine>(0);
                if (ChooseItems[3].remaining >= 1)
                {
                    isOrder = true;
                    orderCount++;
                    Debug.Log("�� �ֹ�");
                }
            }

            //���
            recursionCount++;
        }
        //�� ǰ���̸�
        if (!isOrder && recursionCount > 3)
        {
            Debug.Log("ǰ��,���Ը������ϴ�.");
            LeaveStore();

        }
        else
        {
            //�ֹ������� �ð� ���
            yield return new WaitForSeconds(2.0f);
            player.SellItem(ChooseItems, orderCount);

            //�������� ������ ���� ������ ������
            yield return new WaitForSeconds(leaveTime);
            LeaveStore();
        }

    }

    //============== ����ã�Ƽ� �ɰų� ���� ������ TODO::
    private void FindCushion(int number)
    {
        StopCoroutine(OrderAndWating());
        Debug.Log("���ã�� ����");
        bool isCushion = false;
        if (!isWaiting && !goCushion && orderCount == number)
        {
            isWaiting = true;
            wating.LeaveWating(0);
            for (int i = 0; i < cushions.Length; i++)
            {
                if (!cushions[i].CheckCushion())
                {
                    usingCushion = cushions[i];
                    isCushion = true;
                    usedCushion = true;
                    break;
                }
            }
            //�� ����� ������ ������.
            if (isCushion == false)
            {
                goCushion = true;
                isLeave = true;
            }
        }
        else if (!isCushion && !goCushion && inStore)
        {
            wating.LeaveWating(watingNumber);
            watingNumber -= 1;
            if (watingNumber < 0)
            {
                watingNumber = 0;
            }
            dogMoveTransform = wating.CheckWating(watingNumber);
            Debug.Log($"���� {gameObject.name}�� ����ȣ: {watingNumber}");


        }
    }

    IEnumerator LeaveStore()
    {
        if (!outStore)
        {

            ChangeState(DogState.Walk);
            isRight = true;
            goStore = true;
            inStore = true;
            isWaiting = true;
            goCushion = true;
            isLeave = true;
            if(usedCushion == true)
            {
                usedCushion = false;
                //TODO:: 100������ ����
                player.Money += Random.Range(200,1000);
                Debug.Log("���������");
            }
            dogMoveTransform = storeMoveTransform.GetChild(0);
            transform.SetPositionAndRotation(Vector3.MoveTowards(transform.position, dogMoveTransform.position, Time.deltaTime * Speed), Quaternion.LookRotation(new Vector3(-dogMoveTransform.position.x, 0f, 0f)));
            if (transform.position == dogMoveTransform.position && outStore == false)
            {
                setRandomFloat = Random.value;
                if (setRandomFloat >= 0.5)
                {
                    isRight = true;
                    outStore = true;
                }
                else
                {
                    isRight = false;
                    outStore = true;
                }
            }
            while (outStore == true)
            {
                PassBy();
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        yield return null;
    }



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
