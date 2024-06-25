using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;
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
    [SerializeField] private float Speed = 1f;
    [Header("Ȯ�� ����")]
    [SerializeField] float setRandomFloat;
    DogState state;
    private const float GroundLeftX = -25f;
    private const float GroundRightX = 25f;
    private const float GroundMinZ = -8.5f;
    private const float GroundMaxZ = -7.3f;

    private Image thoguhtImage;
    private Sprite questionMark;

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
        state = DogState.Walk;
        ChangeState();
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
    private void GoStore()
    {
        //TODO:: ���� ��ġ��
        dogMoveTransform = storeMoveTransform.GetChild(0);
        transform.SetPositionAndRotation(Vector3.MoveTowards(transform.position, dogMoveTransform.position, Time.deltaTime * Speed), Quaternion.LookRotation(new Vector3(-dogMoveTransform.position.x, 0f, 0f)));
        if (transform.position == dogMoveTransform.position)
        {
            inStore = true;
            StopAllCoroutines();
            StartCoroutine(OrderAndWating());
            Debug.Log("���� ����");
        }

    }

    //�ֹ��ϰ� ��ٸ���
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
                //�ֹ��ϰ�
                orderCount++;
                isOrder = true;
            }
            else
            {
            setRandomFloat = Random.value;
                //���� ��Ű��
                if(setRandomFloat <= 0.3f)
                {
                    orderCount++;
                    isOrder = true;
                }
                //���� ��Ű��
                setRandomFloat = Random.value;
                if(setRandomFloat <= 0.5f) 
                {
                    orderCount++;
                    isOrder = true;
                }
                //����
                setRandomFloat = Random.value;
                if(setRandomFloat <= 0.15f)
                {
                    orderCount++;
                    isOrder = true;
                }
            }//TODO:: ���� ó������ ��緯�³༮�� �ุ������

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
