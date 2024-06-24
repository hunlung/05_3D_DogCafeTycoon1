using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassByDog : RecycleObject
{

    private const float GroundStartX = -24f;
    private const float GroundMinZ = -8.5f;
    private const float GroundMaxZ = -7.3f;
    private const float GroundEndX = 25f;
    public float passBySpeed = 1f;
    Animator animator;
    protected override void OnEnable()
    {
        base.OnEnable();
        
        gameObject.transform.position = new Vector3(GroundStartX, 1f, Random.Range(GroundMinZ, GroundMaxZ));

    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        transform.Rotate(0, 90, 0);
        animator.SetInteger("AnimationID", 1);
    }



    private void Update()
    {
        transform.Translate(Vector3.forward * passBySpeed * Time.deltaTime);
        if(transform.position.x >= GroundEndX)
        {
            gameObject.SetActive(false);
        }
    }




}
