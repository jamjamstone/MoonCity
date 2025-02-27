using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;


/// 불필요한 start와 업데이트 제거하기 필요할 경우 오브젝트 풀링 사용하기 
/// getcomponent는 업데이트에서 사용하지 않기 
/// GameObject.getcomponent보다는 변수를 별도 할당해서 초기부터 할당하기
/// 



public class PlayerMovement : MonoBehaviour
{
    //PlayerState nowState;// 상태머신은 플레이어에게 적합하지 않음 몬스터에게 사용할 것
    Animator playerAnimator;
    int comboNum=0;
    Transform tr;
    Vector3 moveDir;
    Vector2 mouseDir;
    PlayerInput playerInput;
    public AudioSource playerWalkSound;
    public AudioSource playerAttackSound;
    //InputActionMap mainActionMap;
    //InputAction moveAction;
    //InputAction attackAction;
    //public static float radNum = 180.0f / 4.0f;//예전 방향 정할때 사용했던 변수 지금은 사용하지 않음

    [Header("PlayerData")]
    public float PlayerMaxHp;// = GameManager.Instance.GameData.playerData.playerMaxHp;
    public float PlayerNowHp;// = GameManager.Instance.GameData.playerData.playerNowHp;
    public float PlayerStamina=100f;// = GameManager.Instance.GameData.playerData.playerStamina;
    public float StaminaRecoveryTime=0f;// = GameManager.Instance.GameData.playerData.staminaRecoveryTime;
    public float StaminaRecoveryCool=1f;// = GameManager.Instance.GameData.playerData.staminaRecoveryCool;
    public Vector3 PlayerPosition;// = GameManager.Instance.GameData.playerData.playerPosition;
    public Quaternion PlayerRotation;// = GameManager.Instance.GameData.playerData.playerRotation;

    public CinemachineFreeLook playerCamera;

    //public CinemachineVirtualCamera ;
    public Collider lockOnMonster;
    public Collider[] lockOnTargets;
    //public List<Collider> lockOnRecord;
    public LayerMask monsterLayer;
    public int monsterLayerNum;
    [SerializeField] float detectRadius=11f;


    readonly int hashLockOn = Animator.StringToHash("isLockOn");



    //[Header("플레이어 상태 플래그")]
    bool islockOn = false;
    bool isPray=false;
    bool isBlock=false;
    bool isDodge=false; 
    bool isInteract=false;
    bool isDie = false;
    bool isIdle=false;
    bool isAttack=false;

    // Start is called before the first frame updates
    void Start()
    {
        PlayerMaxHp = 100f;
        playerAnimator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        //playerAudioSource = GetComponent<AudioSource>();
        monsterLayerNum = LayerMask.GetMask("Monster");
        //lockOnRecord = new List<Collider>();
        PlayerNowHp = PlayerMaxHp;
        PlayerStamina = 100f;
        StartCoroutine(PlayerMoveSound());
        GameManager.Instance.LockOnMonsterUpdate += LockOnMonsterDead;
        
        GameManager.Instance.PlayerDataChange += DownPlayerData;
        GameManager.Instance.PlayerHPUpdate += UpdateHp;
        GameManager.Instance.OnPlayerHit.AddListener(PlayerGetDamage);
        
    }
    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LockOnMonsterUpdate -= LockOnMonsterDead;
            GameManager.Instance.PlayerDataChange -= DownPlayerData;
            GameManager.Instance.PlayerHPUpdate -= UpdateHp;
            GameManager.Instance.OnPlayerHit.RemoveListener(PlayerGetDamage);
        }

    }

    // Update is called once per frame
    void Update()
    {
        //attackDelay += Time.deltaTime;
        //var mouseRotation = Input.GetAxis("MouseX");
        //if (staminaRecoveryTime > staminaRecoveryCool)
        //{
        //
        //}
        //if ( mouseRotation> 0.01f)
        //{
        //    transform.rotation=Quaternion.Euler(0,mouseRotation*Time.deltaTime,0);
        //}
        if(Input.GetMouseButton(1))
        {
            playerAnimator.SetBool("Block", true);
            isBlock = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            playerAnimator.SetBool("Block", false);
            isBlock = false;
        }

        if (moveDir != Vector3.zero)
        {
            if (islockOn == false)
            {
                transform.rotation = Quaternion.LookRotation(moveDir);
                //Debug.Log(moveDir);
                transform.Translate(Vector3.forward * Time.deltaTime * 4, Space.Self);//직진중 -> 변경 완료 애니메이션과 연동을 생각해보자
            }
            else
            {
                transform.Translate(moveDir * Time.deltaTime * 4, Space.Self);
                transform.LookAt(lockOnMonster.transform);
            }

            

        }
        else
        {
            //transform.rotation = Quaternion.Euler(mouseDir.x, 0, mouseDir.y);
        }
    }


    private void FixedUpdate()
    {
        StaminaRecoveryTime += Time.fixedDeltaTime;
        if (StaminaRecoveryTime >= StaminaRecoveryCool)
        {
            //playerStamina += Time.fixedDeltaTime * 0.5f;
            //Mathf.Clamp(playerStamina, 0, 100f);
            PlayerStamina=Mathf.Clamp(PlayerStamina += Time.fixedDeltaTime * 50f, 0, 100f);//스테미나는 100이상이 될 수 없다
        }
    }

    //public void SetPlayerState(PlayerState nowState)//플레이어는 즉각적인 반응이 필요하므로 상태 패턴 사용 필요도가 떨어짐
    //{
    //    this.nowState = nowState;
    //}
    //IEnumerator DetectPlayerState()
    //{
    //    
    //}

    IEnumerator PlayerMoveSound()
    {
        while(true)
        {
            if (moveDir != Vector3.zero)
            {
                //Debug.Log("tap");
                playerWalkSound.Play();
            }
            yield return new WaitForSeconds(0.3f);

        }
    }
    
    #region 사용하지 않는
    public void OnDelta(InputValue value)// 마우스가 이동하는 방향으로 플레이어의 로테이션 이동을 생각했지만 다크소울에 가까운 특성상 락온일 경우에 방향을 고정하는 것으로 결정
                                         // 따라서 지금 당장은 사용하지 않음 02/17
    {
        Vector2 playerLook = value.Get<Vector2>();
        mouseDir= new Vector3(playerLook.x,0,playerLook.y);
        //Debug.Log(mouseDir);

    }
    #endregion
    public void PlayerDead()
    {
        //gameObject.
    }

    public void UpdateHp(float nowHp)
    {
        PlayerNowHp = nowHp;
    }
    public void SearchTargetMonster()
    {
        //Ray ray = new Ray(transform.position,transform.forward);
        //RaycastHit hit;
        //Physics.Raycast(ray, out hit,monsterLayer);
        lockOnTargets = Physics.OverlapSphere(transform.position, detectRadius,monsterLayer);

       //LockOnMonster()

    }
    //public void LockOnMonster(Collider[] monsters)// 옵저버 패턴으로 락온몬스터가 죽으면 다음 몬스터로 선택하자
    //{
    //    if(monsters.Length == 0)
    //    {
    //        UnLockOnMonster();
    //    }
    //    if (lockOnMonster == null)
    //    {
    //        lockOnMonster = monsters[0];
    //        islockOn = true;
    //        monsters[0]=null;
    //    }
    //    else
    //    {
    //        foreach(Collider c in monsters)
    //        {
    //            if (c != lockOnMonster&&c!=null)
    //            {
    //                lockOnMonster = c;
    //                islockOn = true;
    //                playerAnimator.SetBool("isLockOn", islockOn);
    //                return;
    //            }
    //        }
    //    }
    //}

    public void LockOnMonster(Collider monster)
    {
        lockOnMonster = monster;
        islockOn = true;
        playerAnimator.SetBool(hashLockOn, islockOn);
        //playerCamera.LookAt = lockOnMonster.transform;
        playerCamera.m_LookAt = lockOnMonster.transform;
        GameManager.Instance.RecordLockOnMonster(lockOnMonster.transform);
        //playerCamera.
    }


    public void UnLockOnMonster()
    {
        lockOnMonster=null;
        islockOn=false;
        playerAnimator.SetBool(hashLockOn,islockOn);
        playerCamera.m_LookAt = null;
    }
    public void LockOnMonsterDead(Transform monster)
    {
        //Collider[] cloneTargets;
       
        //cloneTargets=
        for(int i=0;i<lockOnTargets.Length;i++)//lockontarget중에서
        {
            if (lockOnTargets[i].gameObject.transform == monster)//죽은 몬스터가 존재하면
            {
                lockOnTargets[i] = null;
                if (lockOnMonster.transform == monster)
                {
                    UnLockOnMonster();
                }
                //islockOn = false;
                //lockOnMonster = null;
                //playerAnimator.SetBool(hashLockOn, islockOn);
                
            }
        }
    }
    public void SaveGame()
    {
        PlayerDataForSave tempData= new PlayerDataForSave();
        Vector3 forSavePosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Quaternion forSaveRoatation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z);
        float nowHpForSave = PlayerNowHp;
        float maxHpForSave = PlayerMaxHp;
        //GameManager.Instance.GameData.playerData.playerMaxHp = maxHpForSave;
        //GameManager.Instance.GameData.playerData.playerNowHp = nowHpForSave;
        //GameManager.Instance.GameData.playerData.playerPosition = forSavePosition;
        //GameManager.Instance.GameData.playerData.playerRotation = forSaveRoatation;
        tempData.playerNowHp=nowHpForSave;
        tempData.playerRotation= forSaveRoatation;
        tempData.playerPosition = forSavePosition;
        tempData.playerMaxHp=maxHpForSave;
        GameManager.Instance.DataUpdate(tempData);
        
    }
    public void DownPlayerData(PlayerDataForSave playerData)
    {
        PlayerNowHp=playerData.playerNowHp;
        PlayerMaxHp=playerData.playerMaxHp;
        transform.position=playerData.playerPosition;
        transform.rotation=playerData.playerRotation;
        //Debug.Log(playerData.playerPosition+"from"+position)
    }
    public void ChangeLockOnMonster(Collider monster)
    {
        for (int i = 0; i < lockOnTargets.Length; i++)
        {
            if (lockOnMonster != lockOnTargets[i])
            {
                LockOnMonster(lockOnTargets[i]);
                playerCamera.m_LookAt = lockOnMonster.transform;
                GameManager.Instance.RecordLockOnMonster(lockOnMonster.transform);
                return;
            }
        }


        //bool isDuplicate = false;
        //for (int i = 0; i < lockOnTargets.Length; i++)//lockontarget중에서
        //{
        //    isDuplicate = false;
        //    for(int j = 0; j < GameManager.Instance.lockOnMonsterRecord.Count; j++)
        //    {
        //        if (lockOnTargets[i].gameObject.transform == GameManager.Instance.lockOnMonsterRecord[j])
        //        {
        //            isDuplicate = true;
        //            break;
        //        }
        //        
        //    }
        //    if (isDuplicate == false)
        //    {
        //        lockOnMonster = lockOnTargets[i];
        //    }
        //
        //}
    }

    public void SetRunMotion(Vector2 v2)
    {
        float angle = Vector2.Angle(Vector2.up, v2);
        //Debug.Log(angle);
        //angle = angle * Mathf.Deg2Rad;
        if (angle<=45-15f )
        {
            playerAnimator.SetInteger("RunDirection",8);
        }else if (45-15f< angle && angle <= 45+15f)
        {
            if (v2.x > 0)
            {
                playerAnimator.SetInteger("RunDirection", 9);
            }
            else if (v2.x < 0)
            {
                playerAnimator.SetInteger("RunDirection", 7);
            }
        }else if (45+15f < angle && angle <= 135-15f)
        {
            if (v2.x > 0)
            {
                playerAnimator.SetInteger("RunDirection", 6);
            }
            else if (v2.x < 0)
            {
                playerAnimator.SetInteger("RunDirection", 4);
            }
        }
        else if(135-15f < angle && angle <= 135+15f)
        {
            if (v2.x > 0)
            {
                playerAnimator.SetInteger("RunDirection", 3);
            }
            else if (v2.x < 0)
            {
                playerAnimator.SetInteger("RunDirection", 1);
            }
        }
        else if(135+15f < angle && angle <= 180f)
        {
            playerAnimator.SetInteger("RunDirection", 2);
        }

    }
    void ResetComboNum()
    {
        comboNum = 0;
    }
    public void BlockEnd()
    {
        isBlock = false;
    }
   
    public void DodgeEnd()
    {
        isDodge = false;
    }
    #region 인풋시스템
    public void OnMove(InputValue value)//이동 각도는 벡터의 길이가 0.9, 0.4일때 변화
    {
        Vector2 dir = value.Get<Vector2>();
        moveDir = new Vector3(dir.x, 0, dir.y);
        //playerAudioSource.Play();
        playerAnimator.SetFloat("Move", dir.magnitude);//move는 이동량? 이동의 세기 정도 
        SetRunMotion(dir);


    }

    public void OnOpenInventory()
    {
        //Debug.Log("invenopen");
        //GameManager.Instance.Test();
        if (GameManager.Instance != null)
        {
            GameManager.Instance.InventoryButtonPressed();
        }
    }
    public void OnLockOn()
    {       
       SearchTargetMonster();
        if (lockOnTargets.Length == 0)
        {
            islockOn=false;
            return;
        }
        else
        {
            LockOnMonster(lockOnTargets[0]);   
        }
    }
    public void OnInteract()
    {
        
        Vector3 forSavePosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Quaternion forSaveRoatation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z);
        float nowHpForSave = PlayerNowHp;
        float maxHpForSave = PlayerMaxHp;
        GameManager.Instance.GameData.playerData.playerMaxHp = maxHpForSave;
        GameManager.Instance.GameData.playerData.playerNowHp = nowHpForSave;
        GameManager.Instance.GameData.playerData.playerPosition = forSavePosition;
        GameManager.Instance.GameData.playerData.playerRotation = forSaveRoatation;
        GameManager.Instance.OnInteract?.Invoke();
    }
    public void OnUseItem()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnUseFoodButtonClick.Invoke();
        }
    }

    public void OnChangeLockOn()
    {
        //Debug.Log("lockchange");
        if (islockOn == false)
        {
            return;
        }
        if(lockOnTargets.Length <= 1) 
        {
            return;
        }
        //Debug.Log("lockchange");
        ChangeLockOnMonster(lockOnMonster);


    }
    //public void OnInteract()
    //{
    //    if (true)//save 기능 지금은 시간상 신상에 상호작용만 해도 저장되는걸로 -> 신상에 회복기능을 넣을까?-> 시간상 저장만
    //    {
    //        Vector3 forSavePosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    //        Quaternion forSaveRoatation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z);
    //        float nowHpForSave=PlayerNowHp;
    //        float maxHpForSave = PlayerMaxHp;
    //        GameManager.Instance.GameData.playerData.playerMaxHp = maxHpForSave;
    //        GameManager.Instance.GameData.playerData.playerNowHp = nowHpForSave;
    //        GameManager.Instance.GameData.playerData.playerPosition = forSavePosition; 
    //        GameManager.Instance.GameData.playerData.playerRotation = forSaveRoatation;
    //        //GameManager.Instance.
    //    }
    //
    //}




    public void OnFire()
    {
        if (PlayerStamina <= 0)
        {
            return;
        }
        playerAttackSound.Play();
        playerAnimator.SetTrigger("Attack");
        playerAnimator.SetInteger("ComboNum", ++comboNum);
        PlayerStamina -= 20;
        PlayerStamina= Mathf.Clamp(PlayerStamina, 0, 100);
        StaminaRecoveryTime = 0;
        GameManager.isPlayerAttack = true;
    }
    
    public void OnBlock()
    {
        //Debug.Log(value);
        //Debug.Log(value.)
        //playerAnimator.SetBool("Block",true);
        //
        //isBlock = true;

        DecideBlock();

    }
    public void DecideBlock()
    {
        if (GameManager.isBlock == true)
        {
            playerAnimator.SetBool("Block", false);
            GameManager.isBlock = false;
        }
        else
        {
            playerAnimator.SetBool("Block", true);
            GameManager.isBlock= true;
        }
    }
    //public void EndBlock()
    //{
    //    playerAnimator.SetBool("Block", false);
    //    isBlock = false;
    //}
   
    public void OnDodge()
    {
        playerAnimator.SetTrigger("Dodge");
        isDodge = true;
    }
    
    public void PlayerGetDamage()
    {
        playerAnimator.SetTrigger("Hit");
    }


    #endregion
    //private void OnCollisionEnter(Collision collision)//피격 판정 및 계산은 몬스터에 달아놓은 특수 콜라이더에서 실행하는 것이 좋아 보임
    //{
    //    
    //    if (collision.gameObject.layer ==LayerMask.NameToLayer("Monster"))
    //    {
    //        Debug.Log("monsterhit");
    //        if (isDodge == false)//회피중이 아니고
    //        {
    //            if (isBlock == false)//방어중이 아니면
    //            {
    //                PlayerNowHp -=collision.gameObject.GetComponent<Monster>().monsterDmg;
    //                playerAnimator.SetTrigger("Hit");//피격모션 재생
    //                
    //            }
    //            else//방어중이면
    //            {
    //               PlayerNowHp -= collision.gameObject.GetComponent<Monster>().monsterDmg/5;
    //                
    //            }
    //        }
    //        else//회피중이면
    //        {
    //            //피해 없음
    //        }
    //    }
    //    else
    //    {
    //        //몬스터에 충돌한게 아니면 피해 없음
    //    }
    //    if (PlayerNowHp <= 0)
    //    {
    //        GameManager.Instance.PlayerDead();
    //    }
    //
    //
    //}





}
