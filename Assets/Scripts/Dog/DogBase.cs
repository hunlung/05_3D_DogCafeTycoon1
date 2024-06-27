using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class DogBase : RecycleObject
{
    //도그 State에 따른 애니메이션 변화 주기.PassByDog들을 이쪽으로 바꾸기
    //TODO:: Dog가 움직이지 않으면 상태 Wait으로 바꾸고, 주문 후 의자에 갔을 때 Sit으로 바꾸기
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
    [Header("확률 보기")]
    [SerializeField] float setRandomFloat;
    DogState state;
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

    int watingNumber = 5;

    //private로 바꾸고 나중에 test변경 TODO::
    public bool isRight = false;
    public bool goStore = false;
    public bool inStore = false;
    public bool isWaiting = false;
    public bool goCushion = false;
    public bool isSit = false;

    ItemShopManager itemshopManager;
    GameObject dogQuestionMark;
    Wating wating;
    Animator animator;
    Player player;
    Cushion[] cushions;
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
        isRight = false;
        goStore = false;
        inStore = false;
        isWaiting = false;
        goCushion = false;
        isSit = false;

        watingNumber = 5;
        if (store == null)
        {
            store = GameObject.FindWithTag("MergedStore");
            storeMoveTransform = store.transform.GetChild(4);
        }
    }

    private void Start()
    {
        player = GameManager.Instance.Player;
        player.onSell += FindCushion;
        likeFood = ItemManager.Instance.GetItemByIndex<Item_Dessert>(likeFood.itemCode);
        likeDrink = ItemManager.Instance.GetItemByIndex<Item_Drink>(likeDrink.itemCode);
        cushions = FindObjectsOfType<Cushion>();
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
            Debug.Log("오른쪽스타트");
        }
        //가끔 오른쪽에서 오게 만들기
        else
        {
            gameObject.transform.position = new Vector3(GroundLeftX + 1, 0.2f, Random.Range(GroundMinZ, GroundMaxZ));
            isRight = false;
            Debug.Log("왼쪽스타트");
        }
        setRandomFloat = Random.value;
        //인기도에 따라 확률 조절하기 TODO::최대 40%
        //totalProbability =  player.TotalSatisfaction;
        ChangeState(DogState.Walk);
        if (setRandomFloat >= goStoreProbBasic + goStoreProb)
        {
            goStore = false;
            Debug.Log("행인");

        }
        else
        {
            goStore = true;
            Debug.Log("가게로");
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
            transform.position = Vector3.MoveTowards(transform.position, wating.CheckWating(watingNumber).position, Time.deltaTime * Speed);
        }
        else if (!goCushion)
        {
            transform.position = Vector3.MoveTowards(transform.position, usingCushion.transform.position, Time.deltaTime * Speed);
            if(transform.position == usingCushion.transform.position)
            {
                goCushion = true;
                StartCoroutine(SitCushion());
            }
        }
        else if (!isSit)
        {

        }
        else
        {
            StopAllCoroutines();

        }

        //도로 왼쪽, 오른쪽 끝에 도달 시
        if (transform.position.x <= GroundLeftX || transform.position.x >= GroundRightX)
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator SitCushion()
    {
        ChangeState(DogState.Sit);
        usingCushion.UsingCushion();
        yield return new WaitForSeconds(Random.Range(7, 20));
        isSit = true;
        usingCushion.LeaveCushion();
    }

    //줄서기
    IEnumerator WaitingOrder()
    {

        while (watingNumber >= 1)
        {


            watingNumber = wating.CheckWatingNumber();
            Debug.Log($"현재 대기번호: {watingNumber}");
            yield return new WaitForSeconds(1f);
        }
        Debug.Log("대기 끝, 주문으로 넘어감");
        StartCoroutine(OrderAndWating());

    }

    //주문하고 기다리기
    IEnumerator OrderAndWating()
    {
        ChangeState(DogState.Wait);
        int randomInt;
        bool isOrder = false;
        float thoughtTime = Random.Range(0.2f, 2f);

        Debug.Log("주문 생각중");
        yield return new WaitForSeconds(thoughtTime);
        Debug.Log("주문중");
        while (isOrder == false)
        {
            setRandomFloat = Random.value;
            if (setRandomFloat <= orderLike)
            {
                //좋아하는 세트 주문하고
                ChooseItems[0] = likeFood;
                ChooseItems[1] = likeDrink;
                isOrder = true;
                Debug.Log($"좋아하는 음식 주문 주문내용: {ChooseItems[0]},{ChooseItems[1]}");
                //끝
                break;
            }
            else
            {
                setRandomFloat = Random.value;
                //음식 시키기
                if (setRandomFloat <= 0.3f)
                {
                    randomInt = Random.Range(0, 4);
                    ChooseItems[0] = ItemManager.Instance.GetItemByIndex<Item_Dessert>(randomInt);
                    if (!ChooseItems[0].itemCantBuy)
                    {
                    isOrder = true;
                    Debug.Log($"음식 주문: {ChooseItems[0]}");
                    }

                }
                //음료 시키기
                setRandomFloat = Random.value;
                if (setRandomFloat <= 0.5f)
                {
                    randomInt = Random.Range(0, 4);
                    ChooseItems[1] = ItemManager.Instance.GetItemByIndex<Item_Drink>(randomInt);
                    if (!ChooseItems[1].itemCantBuy)
                    {
                    isOrder = true;
                    Debug.Log($"마실것 주문: {ChooseItems[1]}");
                    }

                }
                //굿즈
                setRandomFloat = Random.value;
                if (setRandomFloat <= 0.15f)
                {
                    ChooseItems[2] = ItemManager.Instance.GetItemByIndex<Item_Goods>(0);
                    isOrder = true;
                    Debug.Log($"굿즈 주문: {ChooseItems[2]}");
                }
            }//TODO:: 약은 처음부터 약사러온녀석만 약만구매함

        }
        player.SellItem(ChooseItems);
        yield return null;

    }

    //============== 의자찾아서 앉거나 가게 떠나기 TODO::
    private void FindCushion()
    {
        if(!isWaiting && !goCushion)
        {
        isWaiting = true;
        wating.LeaveWating(watingNumber);
        for(int i = 0; i< cushions.Length; i++)
        {
            if (!cushions[i].CheckCushion())
            {
                usingCushion = cushions[i];
            }
        }
        }
    }



    //=============================목적지
    //손님
    private void GoStore()
    {
        //TODO:: 방향 고치기
        dogMoveTransform = storeMoveTransform.GetChild(0);
        transform.SetPositionAndRotation(Vector3.MoveTowards(transform.position, dogMoveTransform.position, Time.deltaTime * Speed), Quaternion.LookRotation(new Vector3(-dogMoveTransform.position.x, 0f, 0f)));
        if (transform.position == dogMoveTransform.position)
        {
            inStore = true;
            StopAllCoroutines();
            StartCoroutine(WaitingOrder());
            Debug.Log("가게 도착");
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
