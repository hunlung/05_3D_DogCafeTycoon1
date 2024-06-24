using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    [SerializeField] private Item_Dessert likeDrink;
    [SerializeField] private float Speed = 1f;
    [SerializeField] private float basicProbability = 0.2f;
    [Header("Ȯ�� ����")]
    [SerializeField] float setDirection;
    DogState state;
    private const float GroundLeftX = -25f;
    private const float GroundRightX = 25f;
    private const float GroundMinZ = -8.5f;
    private const float GroundMaxZ = -7.3f;
    private bool isRight = false;
    private bool goStore = false;
    private float totalProbability;
    Animator animator;
    Player player;

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

    }

    private void SetDestination()
    {
        setDirection = Random.Range(0f, 1f);
        if (setDirection <= 0.8f)
        {
            gameObject.transform.position = new Vector3(GroundLeftX + 1, 1f, Random.Range(GroundMinZ, GroundMaxZ));
            isRight = false;
        }
        //���� �����ʿ��� ���� �����
        else
        {
            gameObject.transform.position = new Vector3(GroundRightX - 1, 1f, Random.Range(GroundMinZ, GroundMaxZ));
            isRight = true;
        }
        setDirection = Random.Range(0f, 1f);
        //�α⵵�� ���� Ȯ�� �����ϱ� TODO::
        //totalProbability =  player.TotalSatisfaction;
        state = DogState.Walk;
        if (setDirection >= basicProbability + totalProbability)
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
        switch (goStore)
        {
            case false:
                PassBy();
                break;
            case true:
                GoStore();
                break;

        }
        //���� ����, ������ ���� ���� ��
        if (transform.position.x <= GroundLeftX || transform.position.x >= GroundRightX)
        {
            gameObject.SetActive(false);
        }
    }
    private void GoStore()
    {

    }

    private void PassBy()
    {
        switch (isRight)
        {
            case false:
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(GroundRightX + 1, 1f, transform.position.z), Time.deltaTime * Speed);
                break;
            case true:
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(GroundLeftX - 1, 1f, transform.position.z), Time.deltaTime * Speed);
                break;
        }
    }


}
