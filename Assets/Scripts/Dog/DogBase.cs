using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DogBase : RecycleObject
{





    private enum DogAnimation
    {
        Wait = 0,
        Walk,
        Sit
    }

    [SerializeField] private Item_Dessert likeFood;
    [SerializeField] private Item_Drink likeDrink;
    [SerializeField] private ItemBase[] ChooseItems;
    [SerializeField] private float Speed = 1f;
    [Header("Ȯ�� ����")]
    [SerializeField] float setRandomFloat;
    DogAnimation state;
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

    private bool isRight = false;
    private bool goStore = false;
    private bool inStore = false;
    private bool isWaiting = false;
    private bool endWating = false;
    private bool goCushion = false;
    private bool isLeave = false;
    private bool outStore = false;
    public bool isProcessing = false;

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
        cushions = FindObjectsOfType<Cushion>();
        isRight = false;
        goStore = false;
        inStore = false;
        isWaiting = false;
        endWating = false;
        goCushion = false;
        isLeave = false;
        outStore = false;
        usedCushion = false;
        isNight = false;
        isProcessing = false;
        dogQuestionMark.SetActive(false);
        watingNumber = 5;
        orderCount = 0;
        usingCushion = null;

        StopAllCoroutines();
        ChangeState(DogAnimation.Wait);

        for (int i = 0; i < ChooseItems.Length; i++)
        {
            ChooseItems[i] = null;
        }


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



    protected override void OnDisable()
    {
        
        isProcessing = false;
        base.OnDisable();
    }

    private void Start()
    {
        player = GameManager.Instance.Player;
        player.onSell += FindCushionOrWatingChange;
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
        ChangeState(DogAnimation.Walk);
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

    private void RotateTowardsTarget(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }



    private void Update()
    {
        HandleMovement();
        CheckBoundaries();
    }

    private void HandleMovement()
    {
        if (!inStore)
        {
            HandleOutsideStore();
        }
        else if (!isWaiting)
        {
            if (dogMoveTransform != null)
            {
                MoveTowards(dogMoveTransform.position);
            }
            else
            {
                SetLeaveStore();
            }
        }
        else if (!goCushion)
        {
            MoveTowardsCushion();
        }
        else if (isLeave)
        {
            SetLeaveStore();
        }
    }

    private void HandleOutsideStore()
    {
        if (goStore)
        {
            GoStore();
        }
        else
        {
            PassBy();
        }
    }

    private void MoveTowards(Vector3 target)
    {
        Vector3 newPosition = Vector3.MoveTowards(transform.position, target, Time.deltaTime * Speed);
        RotateTowardsTarget(target);
        transform.position = newPosition;
    }

    private void MoveTowardsCushion()
    {
        MoveTowards(usingCushion.transform.position);
        StartCoroutine(SitCushion());
    }

    private void CheckBoundaries()
    {
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
            ChangeState(DogAnimation.Sit);
            goCushion = true;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            yield return new WaitForSeconds(Random.Range(7, 20));
            isLeave = true;
            usingCushion.LeaveCushion();
        }

    }



    //�ټ���
    IEnumerator WaitingOrder()
    {

        if (GameManager.Instance.isLastOrder)
        {
            SetLeaveStore();
            yield break;
        }

        if (isProcessing) yield break;
        isProcessing = true;

        ChangeState(DogAnimation.Wait);
        wating.AddToWaitingList(this);
        float waittime = 0f;
        while (true)
        {
            waittime += 0.1f;
            watingNumber = wating.GetWaitingPosition(this);
            dogMoveTransform = wating.CheckWating(watingNumber);
            if (dogMoveTransform == null)
            {
                Debug.LogError($"�߸��� ��� ��ġ�Դϴ�. ��� ��ȣ: {watingNumber}");
                SetLeaveStore();
                yield break;
            }

            if (watingNumber == -1)
            {
                Debug.LogWarning($"{gameObject.name}��(��) ��⿭���� ���ŵǾ����ϴ�.");
                SetLeaveStore();
                yield break;
            }



            float distance = Vector3.Distance(transform.position, dogMoveTransform.position);
            if (distance > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, dogMoveTransform.position, Time.deltaTime * Speed);
            }
            else
            {
                if (watingNumber == 0 && wating.IsNextInLine(this) && !endWating)
                {
                    Debug.Log($"�ֹ����� :{gameObject.name}");
                    isProcessing = false;
                    endWating = true;
                    StartCoroutine(OrderAndWating());
                    yield break;
                }
            }
            //�������ų� �ʹ� ������ٸ��� ������
            if (waittime > 50f)
            {

                wating.RemoveFromWaitingList(this);
                isProcessing = false;
                SetLeaveStore();
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void UpdateWaitingNumber(int newNumber)
    {
        watingNumber = newNumber;
        if (watingNumber == 0 && !isWaiting)
        {
            StartCoroutine(WaitingOrder());
        }
    }

    public void StartOrder()
    {
        StartCoroutine(OrderAndWating());
    }

    //�ֹ��ϰ� ��ٸ���
    IEnumerator OrderAndWating()
    {
        int retryCount = 0;

        Debug.Log($"�����ڷ�ƾ����:{gameObject.name}");
        if (isProcessing)
        {
            Debug.Log($"�����ڷ�ƾ �극��ũ:{gameObject.name}");
            yield break;
        }
        isProcessing = true;

        while (!wating.CanStartOrder(this) && retryCount < 3)
        {
            yield return new WaitForSeconds(0.5f);
            retryCount++;
        }

        if (!wating.CanStartOrder(this))
        {
            Debug.Log($"�����ڷ�ƾ ����Ʈ���� �극��ũ:{gameObject.name}");
            wating.RemoveFromWaitingList(this);
            SetLeaveStore();
            yield break;
        }

        wating.SetCurrentOrderingDog(this);

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
            wating.RemoveFromWaitingList(this);
            isProcessing = false;
            SetLeaveStore();
            yield break; // �ڷ�ƾ ����
        }
        else
        {
            //�ֹ������� �ð� ���
            yield return new WaitForSeconds(2.0f);
            Debug.Log($"�ֹ��� ��:{gameObject.name}");
            player.SellItem(ChooseItems, orderCount);

            //�������� ������ ���� ������ ������
            while (!isWaiting && !goCushion && !isLeave)
            {
                yield return new WaitForSeconds(leaveTime);
                if (!isWaiting && !goCushion && !isLeave) // ���� üũ �߰�
                {
                    wating.RemoveFromWaitingList(this);
                    isProcessing = false;
                    SetLeaveStore();
                }
            }
        }

    }

    private void SetLeaveStore()
    {
        StopAllCoroutines();
        StartCoroutine(LeaveStore());
    }

    //============== ����ã�Ƽ� �ɰų� ���� ������ �� �ٺ���
    //TODO:: ���� ��Ǯ�� �ѹ��� ���� 1�������� �ٽý����� �� ���� �������ڸ��� ������� �� Ȯ���� �ִ� ���� ����
    private void FindCushionOrWatingChange(int number)
    {
        if (isProcessing) // ���� ������ Ȯ��
        {
            StopCoroutine(OrderAndWating());
        }

        bool isCushion = false;

        if (!isWaiting && !goCushion && orderCount == number)
        {
            isWaiting = true;
            wating.RemoveFromWaitingList(this);
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
            if (isCushion == false)
            {
                goCushion = true;
                isLeave = true;
            }
        }
    }

    //ī�� ������
    IEnumerator LeaveStore()
    {
        if (!inStore)
        {
            Debug.Log($"instore���� ����{gameObject.name}");
        }

        if (!outStore)
        {

            ChangeState(DogAnimation.Walk);
            dogQuestionMark.SetActive(false);
            isRight = true;
            goStore = true;
            inStore = true;
            isWaiting = true;
            goCushion = true;
            isLeave = true;
            isProcessing = false;
            //��� ��
            if (usedCushion == true)
            {
                usedCushion = false;
                int tip = Random.Range(2, 11) * 100;
                player.Money += tip;
            }
            dogMoveTransform = storeMoveTransform.GetChild(0);

            while (Vector3.Distance(transform.position, dogMoveTransform.position) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, dogMoveTransform.position, Time.deltaTime * Speed);
                RotateTowardsTarget(dogMoveTransform.position);
                yield return null;
            }


            if (!outStore)
            {
                setRandomFloat = Random.value;
                isRight = setRandomFloat >= 0.5f;
                outStore = true;
            }
        }

        //����� ���� ���ϱ�
        Vector3 exitTarget = isRight ? new Vector3(GroundLeftX - 1, 0.2f, transform.position.z) : new Vector3(GroundRightX + 1, 0.2f, transform.position.z);


        while (Vector3.Distance(transform.position, exitTarget) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, exitTarget, Time.deltaTime * Speed);
            RotateTowardsTarget(exitTarget);
            yield return null;
        }

    }



    //=============================������
    //�մ�
    private void GoStore()
    {
        dogMoveTransform = storeMoveTransform.GetChild(0);
        Vector3 directionToStore = dogMoveTransform.position - transform.position;
        directionToStore.y = 0; // y�� ȸ���� �����ϱ� ����

        transform.position = Vector3.MoveTowards(transform.position, dogMoveTransform.position, Time.deltaTime * Speed);
        transform.rotation = Quaternion.LookRotation(directionToStore);

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

    //�ִϸ��̼� �����
    private void ChangeState(DogAnimation value)
    {
        switch (value)
        {
            case DogAnimation.Wait:
                state = DogAnimation.Wait;
                animator.SetInteger("AnimationID", 0);
                break;
            case DogAnimation.Walk:
                state = DogAnimation.Walk;
                animator.SetInteger("AnimationID", 1);
                break;
            case DogAnimation.Sit:
                state = DogAnimation.Sit;
                animator.SetInteger("AnimationID", 2);
                break;

        }
    }

}
