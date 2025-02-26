using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class OcultMonster : MonoBehaviour
{
    public Transform targetPlayer;
    public LayerMask playerLayer;
    MonsterState monsterState;
    NavMeshAgent monsterAi;
    Rigidbody monsterSelfBody;
    Collider monsterSelfCollider;

    bool isDetecting = false;
    bool isDie = false;
    bool isAttacking = false;
    bool isHitting = false;



    [Serialize] float detectRadius = 5f;
    [Serialize] float attackRadius = 2f;
    [Serialize] float signalRadius = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
