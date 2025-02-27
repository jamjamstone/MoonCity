using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;


/// ���ʿ��� start�� ������Ʈ �����ϱ� �ʿ��� ��� ������Ʈ Ǯ�� ����ϱ� 
/// getcomponent�� ������Ʈ���� ������� �ʱ� 
/// GameObject.getcomponent���ٴ� ������ ���� �Ҵ��ؼ� �ʱ���� �Ҵ��ϱ�
/// 



public class PlayerMovement : MonoBehaviour
{
    //PlayerState nowState;// ���¸ӽ��� �÷��̾�� �������� ���� ���Ϳ��� ����� ��
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
    //public static float radNum = 180.0f / 4.0f;//���� ���� ���Ҷ� ����ߴ� ���� ������ ������� ����

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



    //[Header("�÷��̾� ���� �÷���")]
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
                transform.Translate(Vector3.forward * Time.deltaTime * 4, Space.Self);//������ -> ���� �Ϸ� �ִϸ��̼ǰ� ������ �����غ���
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
            PlayerStamina=Mathf.Clamp(PlayerStamina += Time.fixedDeltaTime * 50f, 0, 100f);//���׹̳��� 100�̻��� �� �� ����
        }
    }

    //public void SetPlayerState(PlayerState nowState)//�÷��̾�� �ﰢ���� ������ �ʿ��ϹǷ� ���� ���� ��� �ʿ䵵�� ������
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
    
    #region ������� �ʴ�
    public void OnDelta(InputValue value)// ���콺�� �̵��ϴ� �������� �÷��̾��� �����̼� �̵��� ���������� ��ũ�ҿ￡ ����� Ư���� ������ ��쿡 ������ �����ϴ� ������ ����
                                         // ���� ���� ������ ������� ���� 02/17
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
    //public void LockOnMonster(Collider[] monsters)// ������ �������� ���¸��Ͱ� ������ ���� ���ͷ� ��������
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
        for(int i=0;i<lockOnTargets.Length;i++)//lockontarget�߿���
        {
            if (lockOnTargets[i].gameObject.transform == monster)//���� ���Ͱ� �����ϸ�
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
        //for (int i = 0; i < lockOnTargets.Length; i++)//lockontarget�߿���
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
    #region ��ǲ�ý���
    public void OnMove(InputValue value)//�̵� ������ ������ ���̰� 0.9, 0.4�϶� ��ȭ
    {
        Vector2 dir = value.Get<Vector2>();
        moveDir = new Vector3(dir.x, 0, dir.y);
        //playerAudioSource.Play();
        playerAnimator.SetFloat("Move", dir.magnitude);//move�� �̵���? �̵��� ���� ���� 
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
    //    if (true)//save ��� ������ �ð��� �Ż� ��ȣ�ۿ븸 �ص� ����Ǵ°ɷ� -> �Ż� ȸ������� ������?-> �ð��� ���常
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
    //private void OnCollisionEnter(Collision collision)//�ǰ� ���� �� ����� ���Ϳ� �޾Ƴ��� Ư�� �ݶ��̴����� �����ϴ� ���� ���� ����
    //{
    //    
    //    if (collision.gameObject.layer ==LayerMask.NameToLayer("Monster"))
    //    {
    //        Debug.Log("monsterhit");
    //        if (isDodge == false)//ȸ������ �ƴϰ�
    //        {
    //            if (isBlock == false)//������� �ƴϸ�
    //            {
    //                PlayerNowHp -=collision.gameObject.GetComponent<Monster>().monsterDmg;
    //                playerAnimator.SetTrigger("Hit");//�ǰݸ�� ���
    //                
    //            }
    //            else//������̸�
    //            {
    //               PlayerNowHp -= collision.gameObject.GetComponent<Monster>().monsterDmg/5;
    //                
    //            }
    //        }
    //        else//ȸ�����̸�
    //        {
    //            //���� ����
    //        }
    //    }
    //    else
    //    {
    //        //���Ϳ� �浹�Ѱ� �ƴϸ� ���� ����
    //    }
    //    if (PlayerNowHp <= 0)
    //    {
    //        GameManager.Instance.PlayerDead();
    //    }
    //
    //
    //}





}
