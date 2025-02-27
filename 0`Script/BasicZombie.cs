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
    public AudioSource monsterAudioSource;
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
        monsterAudioSource = GetComponent<AudioSource>();
        monsterHp = 200f;
       // monsterAnimator = GetComponent<Animator>();
        monsterState = MonsterState.None;
        monsterDmg = 10f;
        StartCoroutine(DetectPlayer());
        StartCoroutine(ChangeState());
        StartCoroutine(monsterSound());
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
                    if ((targetPlayer.position-transform.position).magnitude>attackRadius)//�÷��̾ ���� ���� �ٱ��̸�
                    {
                        Debug.Log("monsterdetect");//�÷��̾� Ž����
                        isDetecting = true;
                        monsterAi.isStopped = false;
                        isAttacking = false;
                        monsterState = MonsterState.Detect;
                        monsterAnimator.SetBool("isWalk", true);
                        monsterAnimator.SetBool("isAttack", false);
                    }
                    else//���ݹ��� �����̸�
                    {
                        Debug.Log("monsterattack");
                        isDetecting=false;
                        monsterAi.isStopped = true;
                        isAttacking = true;
                        monsterState= MonsterState.Attack;//������
                        monsterAnimator.SetBool("isAttack", true);
                        monsterAnimator.SetBool("isWalk", false);

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
                monsterAnimator.SetBool("isAttack", false);
                monsterAnimator.SetBool("isWalk", false);
            }
            yield return new WaitForSeconds(0.5f);
        }
        
    }

    IEnumerator monsterSound()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            monsterAudioSource.Play();


        }
    }


    IEnumerator ChangeState()
    {
        while (monsterState!=MonsterState.Die)
        {
            yield return new WaitForSeconds(0.5f);
            //Debug.Log(monsterSelfBody.velocity.magnitude);
            if (monsterSelfBody.velocity.magnitude > 0.01f)
            {
                
                monsterState = MonsterState.Move;
                monsterAnimator.SetBool("isWalk", true);
            }







        }
    }
    public void SetState()
    {

    }
    public void RandomPatrol()
    {
        monsterSelfBody.velocity = new Vector3(UnityEngine.Random.Range(0, 5), 0, UnityEngine.Random.Range(0, 5));
    }

    public void Dead()
    {
        monsterSelfCollider.enabled = false;
        monsterState = MonsterState.Die;
        isDie = true;
        monsterAnimator.SetBool("isAttack", false);
        monsterAnimator.SetBool("isWalk", false);
        monsterAnimator.SetTrigger("Dead");

        GameManager.Instance.LockOnDead(transform);

        Destroy(gameObject,1);

    }
    IEnumerator Hitduration()
    {
        yield return new WaitForSeconds(1f);
        isHitting = false;


    }


    private void OnTriggerEnter(Collider other)
    {
        
        if (isHitting == true)
        {
            return;
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerSword"))
        {
            if (GameManager.isPlayerAttack == true)
            {
                //Debug.Log("monsterhit");
                monsterAnimator.SetTrigger("isHit");
                StartCoroutine(Hitduration());
                monsterHp -= GameManager.Instance.playerDamage;
                isHitting = true;
            }
            if(monsterHp <= 0)
            {
                isDie = true;
                StopAllCoroutines();
                monsterHp = 0;
                monsterState = MonsterState.Die;
                Dead();
            }
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.layer == LayerMask.NameToLayer("PlayerSword"))
    //    {
    //        isHitting = false;
    //    }
    //}


    //private void OnTriggerStay(Collider other)
    //{
    //    
    //}



}
