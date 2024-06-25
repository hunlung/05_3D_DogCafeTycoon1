using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class DogBase : RecycleObject
{
    //도그 State에 따른 애니메이션 변화 주기.PassByDog들을 이쪽으로 바꾸기
    private enum DogState
    {
        Wait = 0,
        Walk,
        Sit,
        Angry
    }

    [SerializeField] private Item_Dessert likeFood;
    [SerializeField] private Item_Drink likeDrink;
    [SerializeField] private float Speed = 1f;
    [Header("확률 보기")]
    [SerializeField] float setRandomFloat;
    DogState state;
    private const float GroundLeftX = -25f;
    private const float GroundRightX = 25f;
    private const float GroundMinZ = -8.5f;
    private const float GroundMaxZ = -7.3f;

    private Image thoguhtImage;
    private Sprite questionMark;

    //확률들
    [Header("무언가를 할 확률들")]
    [Range(0f, 1f)]
    [Header("오른쪽에서 올 확률")]
    [SerializeField] private float goRightProb =0.2f;

    [Header("상점으로 갈 기본 확률")]
    [Range(0f, 1f)]
    [SerializeField] private float goStoreProbBasic = 0.2f;

    [Header("상점으로 갈 총확률(읽기전용)")]
    [ReadOnly(true)]
    [SerializeField] private float goStoreProb;
    [Header("좋아하는 것을 시킬 확률")]
    [Range(0f, 1f)]
    [SerializeField] private float orderLike = 0.4f;


    //private로 바꾸고 나중에 test변경 TODO::
    public bool isRight = false;
    public bool goStore = false;
    public bool inStore = false;

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
        thoguhtImage = transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>();
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
        state = DogState.Walk;
        ChangeState();
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
        else
        {

        }
        //도로 왼쪽, 오른쪽 끝에 도달 시
        if (transform.position.x <= GroundLeftX || transform.position.x >= GroundRightX)
        {
            gameObject.SetActive(false);
        }
    }
    private void GoStore()
    {
        //TODO:: 방향 고치기
        dogMoveTransform = storeMoveTransform.GetChild(0);
        transform.SetPositionAndRotation(Vector3.MoveTowards(transform.position, dogMoveTransform.position, Time.deltaTime * Speed), Quaternion.LookRotation(new Vector3(-dogMoveTransform.position.x, 0f, 0f)));
        if (transform.position == dogMoveTransform.position)
        {
            inStore = true;
            StopAllCoroutines();
            StartCoroutine(OrderAndWating());
            Debug.Log("가게 도착");
        }

    }

    //주문하고 기다리기
    IEnumerator OrderAndWating()
    {
        bool isOrder = false;
        int orderCount = 0;
        float thoughtTime = Random.Range(0.2f, 5f);

        yield return new WaitForSeconds(thoughtTime);

        while (isOrder == false)
        {
            setRandomFloat = Random.value;
            if (setRandomFloat <= orderLike)
            {
                //주문하고
                orderCount++;
                isOrder = true;
            }
            else
            {
            setRandomFloat = Random.value;
                //음식 시키기
                if(setRandomFloat <= 0.3f)
                {
                    orderCount++;
                    isOrder = true;
                }
                //음료 시키기
                setRandomFloat = Random.value;
                if(setRandomFloat <= 0.5f) 
                {
                    orderCount++;
                    isOrder = true;
                }
                //굿즈
                setRandomFloat = Random.value;
                if(setRandomFloat <= 0.15f)
                {
                    orderCount++;
                    isOrder = true;
                }
            }//TODO:: 약은 처음부터 약사러온녀석만 약만구매함

                yield return null;
        }

    }

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

    private void ChangeState()
    {
        switch (state)
        {
            case DogState.Wait:
                animator.SetInteger("AnimationID", 0);
                break;
            case DogState.Walk:
                animator.SetInteger("AnimationID", 1);
                break;
            case DogState.Sit:
                animator.SetInteger("AnimationID", 2);
                break;
            case DogState.Angry:
                animator.SetInteger("AnimationID", 3);
                break;
        }
    }

}
