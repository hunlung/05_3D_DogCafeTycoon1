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
    [Header("확률 보기")]
    [SerializeField] float setRandomFloat;
    DogAnimation state;
    private const float GroundLeftX = -25f;
    private const float GroundRightX = 25f;
    private const float GroundMinZ = -8.5f;
    private const float GroundMaxZ = -7.3f;


    //확률들
    [Header("무언가를 할 확률들")]
    [Range(0f, 1f)]
    [Header("오른쪽에서 올 확률")]
    [SerializeField] private float goRightProb = 0.2f;

    [Header("상점으로 갈 기본 확률")]
    [Range(0f, 1f)]
    [SerializeField] private float goStoreProbBasic = 0.2f;

    [Header("상점으로 갈 총확률(읽기전용)")]
    [ReadOnly(true)]
    [SerializeField] private float goStoreProb;
    [Header("좋아하는 것을 시킬 확률")]
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

    //초기화 작업
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
        //가끔 오른쪽에서 오게 만들기
        else
        {
            gameObject.transform.position = new Vector3(GroundLeftX + 1, 0.2f, Random.Range(GroundMinZ, GroundMaxZ));
            isRight = false;
        }
        setRandomFloat = Random.value;
        ChangeState(DogAnimation.Walk);
        //만족도 200일때 최대치(최대 20%+)
        if (player != null)
        {
            goStoreProb = player.TotalSatisfaction * 0.001f;
        }
        else
        {
            goStoreProb = 0.05f;
        }
        Mathf.Min(goStoreProb, 0.2f);
        //확률적이거나 라스트오더 이후 가게로 향하지않음.
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



    //줄서기
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
                Debug.LogError($"잘못된 대기 위치입니다. 대기 번호: {watingNumber}");
                SetLeaveStore();
                yield break;
            }

            if (watingNumber == -1)
            {
                Debug.LogWarning($"{gameObject.name}이(가) 대기열에서 제거되었습니다.");
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
                    Debug.Log($"주문시작 :{gameObject.name}");
                    isProcessing = false;
                    endWating = true;
                    StartCoroutine(OrderAndWating());
                    yield break;
                }
            }
            //오류나거나 너무 오래기다릴시 떠나기
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

    //주문하고 기다리기
    IEnumerator OrderAndWating()
    {
        int retryCount = 0;

        Debug.Log($"오더코루틴시작:{gameObject.name}");
        if (isProcessing)
        {
            Debug.Log($"오더코루틴 브레이크:{gameObject.name}");
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
            Debug.Log($"오더코루틴 오버트라이 브레이크:{gameObject.name}");
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
                //좋아하는 세트 주문하고
                ChooseItems[0] = likeFood;
                ChooseItems[1] = likeDrink;
                if (!ChooseItems[0].itemCantBuy && ChooseItems[0].remaining >= 1
                    && !ChooseItems[1].itemCantBuy && ChooseItems[1].remaining >= 1)
                {
                    isOrder = true;
                    orderCount = 2;
                    //끝
                    break;
                }
            }
            else
            {
                setRandomFloat = Random.value;
                //음식 시키기
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
                //음료 시키기
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
                //굿즈
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

            //아무것도 안삿다면 약
            if (!isOrder && recursionCount > 2)
            {
                ChooseItems[3] = ItemManager.Instance.GetItemByIndex<Item_Medicine>(0);
                if (ChooseItems[3].remaining >= 1)
                {
                    isOrder = true;
                    orderCount++;
                    Debug.Log("약 주문");
                }
            }

            //재귀
            recursionCount++;
        }
        //다 품절이면
        if (!isOrder && recursionCount > 3)
        {
            Debug.Log("품절,가게를떠납니다.");
            wating.RemoveFromWaitingList(this);
            isProcessing = false;
            SetLeaveStore();
            yield break; // 코루틴 종료
        }
        else
        {
            //주문나오는 시간 대기
            yield return new WaitForSeconds(2.0f);
            Debug.Log($"주문한 개:{gameObject.name}");
            player.SellItem(ChooseItems, orderCount);

            //오류등의 이유로 반응 없을시 떠나기
            while (!isWaiting && !goCushion && !isLeave)
            {
                yield return new WaitForSeconds(leaveTime);
                if (!isWaiting && !goCushion && !isLeave) // 조건 체크 추가
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

    //============== 의자찾아서 앉거나 가게 떠나기 및 줄변경
    //TODO:: 현재 개풀이 한바퀴 돌아 1번개부터 다시시작할 시 가게 도착하자마자 쿠션으로 갈 확률이 있는 오류 존재
    private void FindCushionOrWatingChange(int number)
    {
        if (isProcessing) // 실행 중인지 확인
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

    //카페 나가기
    IEnumerator LeaveStore()
    {
        if (!inStore)
        {
            Debug.Log($"instore에서 떠남{gameObject.name}");
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
            //쿠션 팁
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

        //사라질 방향 정하기
        Vector3 exitTarget = isRight ? new Vector3(GroundLeftX - 1, 0.2f, transform.position.z) : new Vector3(GroundRightX + 1, 0.2f, transform.position.z);


        while (Vector3.Distance(transform.position, exitTarget) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, exitTarget, Time.deltaTime * Speed);
            RotateTowardsTarget(exitTarget);
            yield return null;
        }

    }



    //=============================목적지
    //손님
    private void GoStore()
    {
        dogMoveTransform = storeMoveTransform.GetChild(0);
        Vector3 directionToStore = dogMoveTransform.position - transform.position;
        directionToStore.y = 0; // y축 회전을 방지하기 위해

        transform.position = Vector3.MoveTowards(transform.position, dogMoveTransform.position, Time.deltaTime * Speed);
        transform.rotation = Quaternion.LookRotation(directionToStore);

        if (transform.position == dogMoveTransform.position)
        {
            inStore = true;
            StopAllCoroutines();
            StartCoroutine(WaitingOrder());
        }
    }

    //행인
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

    //애니메이션 변경용
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
