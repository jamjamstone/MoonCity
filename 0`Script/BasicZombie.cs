using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class BasicZombie : Monster //hp,dmg,name,animator
{
    public Transform targetPlayer;
    public LayerMask playerLayer;
    MonsterState monsterState;
    NavMeshAgent monsterAi;
    Rigidbody monsterSelfBody;
    Collider monsterSelfCollider;

    bool isDetecting=false;
    bool isDie = false;
    bool isAttacking = false;
    bool isHitting = false;



    [Serialize] float detectRadius = 5f;
    [Serialize] float attackRadius = 2f;
    [Serialize] float signalRadius = 10f;





    // Start is called before the first frame update
    void Start()
    {
        monsterAi = GetComponent<NavMeshAgent>();
        monsterSelfBody = GetComponent<Rigidbody>();
        monsterSelfCollider = GetComponent<Collider>();
        monsterHp = 200f;
       // monsterAnimator = GetComponent<Animator>();
        monsterState = MonsterState.None;
        monsterDmg = 10f;
        StartCoroutine(DetectPlayer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        //DetectPlayer();
        
    }
    IEnumerator DetectPlayer()
    {
        while (monsterState != MonsterState.Die)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, detectRadius, playerLayer);
            if (colliders.Length > 0)//Ž���� ��ü�� ������
            {
                foreach (Collider collider in colliders)
                {
                    targetPlayer = collider.transform;
                    if ((targetPlayer.position-transform.position).magnitude>attackRadius)
                    {
                        //Debug.Log("monsterstart");
                        isDetecting = true;
                        monsterAi.isStopped = false;
                        isAttacking = false;
                        monsterState = MonsterState.Detect;
                    }
                    else
                    {
                        //Debug.Log("monsterstop");
                        isDetecting=false;
                        monsterAi.isStopped = true;
                        isAttacking = true;
                        monsterState= MonsterState.Attack;

                    }
                    //isDetecting = true;
                    //monsterAi.isStopped = false;

                    monsterAi.SetDestination(targetPlayer.position);
                    //Debug.Log(targetPlayer.position);
                }


            }
            else
            {
                isDetecting = false;
                isAttacking= false;
                monsterAi.isStopped = true;// ó�� ������ �� ���ߴ� �κ��� �����ؼ� �̸� �ٽ� �ذ������� �ʾ� �߰��� ������ ���ߴ� ���� �߻� -> �ذ�!
            }
            yield return new WaitForSeconds(0.5f);
        }
        
    }




    IEnumerator ChangeState()
    {
        while (monsterState!=MonsterState.Die)
        {
            yield return new WaitForSeconds(0.5f);
            







        }
    }



    public void Dead()
    {
        monsterSelfCollider.enabled = false;
        monsterAnimator.SetTrigger("Dead");
        GameManager.Instance.LockOnDead(transform);

        Destroy(gameObject);

    }



    private void OnTriggerEnter(Collider other)
    {
        
        if (isHitting == true)
        {
            return;
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerSword"))
        {
            Debug.Log("monsterhit");
            monsterHp-=GameManager.Instance.playerDamage;
            if(monsterHp <= 0)
            {
                isHitting = true;
                monsterHp = 0;
                monsterState = MonsterState.Die;
                Dead();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerSword"))
        {
            isHitting = false;
        }
    }


    //private void OnTriggerStay(Collider other)
    //{
    //    
    //}



}
