using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public delegate void GameDataChangeHandler(GameData gamedata);
public delegate void PlayerDataChangeHandler(PlayerDataForSave playerData);
public delegate void LockOnMonsterHandler(Transform Monster);
public delegate void PlayerHitHandler(float playerNowHp);

//public static class GameDataMember
//{
//    public static float PlayerMaxHp=GameManager.Instance.GameData.playerData.playerMaxHp;
//    public static float PlayerNowHp= GameManager.Instance.GameData.playerData.playerNowHp;
//    public static float PlayerStamina= GameManager.Instance.GameData.playerData.playerStamina;
//    public static float StaminaRecoveryTime= GameManager.Instance.GameData.playerData.staminaRecoveryTime;
//    public static float StaminaRecoveryCool= GameManager.Instance.GameData.playerData.staminaRecoveryCool;
//    public static Vector3 PlayerPosition= GameManager.Instance.GameData.playerData.playerPosition;
//    public static Quaternion PlayerRotation= GameManager.Instance.GameData.playerData.playerRotation;
//
//}


public class GameManager : MonoBehaviour
{

    private static GameManager instance;
    private GameData gameData;
    public GameData GameData
    {
        get { return gameData; }
        set { gameData = value; }
    }
    //private PlayerData playerData;
    private string inventoryPath;//= Path.Combine(Application.persistentDataPath, "gameData.json");
    private string playerDataPath;//= Path.Combine(Application.persistentDataPath, "playerData.json");
    public InventoryForJson inventoryForJson;



    public event GameDataChangeHandler GameDataChange;
    public event PlayerDataChangeHandler PlayerDataChange;
    public event LockOnMonsterHandler LockOnMonsterUpdate;
    public event PlayerHitHandler PlayerHPUpdate;
    //public Inventory inventory;
    public List<IInteractable> interactables=new List<IInteractable>();
    

    //public UnityEvent OnLockOnMonsterDead;
    public List<Transform> lockOnMonsterRecord;// 필요 없는듯? 나중에 시간나면 제거


    public UnityEvent OnUseFoodButtonClick;

    public UnityEvent OnOpenInventoryButtonClick;

    public UnityEvent OnLockOnButtonClick;
    //public event EventHandler 

    public UnityEvent MenuBottonPressed;
    public UnityEvent OnSave;
    public UnityEvent OnGameLoad;
    //public event EventHandler 
    public UnityEvent OnPlayerHit;


    public UnityEvent OnInteract;
    public UnityEvent InteractBoxShow;
    public UnityEvent OnInteractBoxHide;




    public UnityEvent OnPlayerDie;

    public float playerDamage = 50f;


    public static bool isInventoryOpen = false;
    public static bool isCanInteract = false;
    public static bool isPlayerHit = false;
    public static bool isDodge = false;
    public static bool isBlock = false;
    public static bool isPlayerAttack = false;
    public static bool isPlayerDeath = false;

    public static bool[] endingFlags;
    public static bool EndingFlag1 = false;
    public static bool EndingFlag2 = false;
    public static bool EndingFlag3 = false;
    public static bool EndingFlag4 = false;


    public static GameManager Instance
    {
        get 
        {
            if (instance == null)
            {
            
                
                //instance = new GameManager();
                //return instance;
                return null;
                
            }
            
            return instance;
        }
    }
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }



        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    public void Test()
    {
        Debug.Log("test");
    }
    void Start()
    {
        inventoryPath= Path.Combine(Application.dataPath+"/Data/", "inventory.json");
        playerDataPath= Path.Combine(Application.dataPath+"/Data/", "playerData.json");
        FirstStartGameDataSet();
        LockOnMonsterUpdate += DeleteMonsterAtLockOnRecord;
        endingFlags=new bool[4];
        
        inventoryForJson = new InventoryForJson();
        //OnLockOnMonsterDead.AddListener(DeleteMonsterAtLockOnRecord());
        //GameDataChangeHandler += GameDataChanged();
    }
    public void FirstStartGameDataSet()
    {
        gameData = new GameData();
        gameData.playerData.playerMaxHp = 100f;
        gameData.playerData.playerNowHp = gameData.playerData.playerMaxHp;
        
    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log("exist");
    }
    IEnumerator Hitduration()
    {
        yield return new WaitForSeconds(1f);
        isPlayerHit= false;


    }

    public void LockOnDead(Transform monster)
    {
        LockOnMonsterUpdate?.Invoke(monster);
    }
    public void RecordLockOnMonster(Transform monster)//record lockonmonster만든 이유가? 뭐였지? 대충 락온했던 몬스터 죽으면 발생할 용도였나?-> 필요 없는거 같은데...
    {
        lockOnMonsterRecord.Add(monster);
    }
    public void DeleteMonsterAtLockOnRecord(Transform monster)
    {
        lockOnMonsterRecord.Remove(monster);
    }
    public void GameDataChanged()
    {
        GameDataChange?.Invoke(gameData);
        PlayerDataChange?.Invoke(gameData.playerData);
    }
    public void PlayerDataChanged()
    {
        PlayerDataChange?.Invoke(gameData.playerData);
    }
    



    public void PlayerDead()
    {
        OnPlayerDie?.Invoke();
    }
    public void InventoryConvert()
    {
        foreach(var item in gameData.inventory.items)
        {
            inventoryForJson.items.Add(item.Value);//error
        }
        foreach (var item in gameData.inventory.questItems)
        {
            inventoryForJson.items.Add(item.Value);
        }
    }
    public void SaveGame()
    {
        Debug.Log("playerdata save");
        //var saveGameData = 
        string json=JsonUtility.ToJson(gameData.playerData,true);
        File.WriteAllText(playerDataPath, json);
        InventoryConvert();
        string json2 = JsonUtility.ToJson(inventoryForJson, true);
        File.WriteAllText(inventoryPath, json2);
        //Debug.Log(json);
        //Debug.Log(json2);
        //Debug.Log(json.ToString()); 
        //Debug.Log(gameData.playerData.playerPosition);
        
    }
    public void LoadGame()
    {
        if (File.Exists(playerDataPath))
        {

            //Debug.Log("savefile exist");
            string json=File.ReadAllText(playerDataPath);
            gameData.playerData = JsonUtility.FromJson<PlayerDataForSave>(json);
            if (isPlayerDeath == true)
            {
                gameData.playerData.playerNowHp = gameData.playerData.playerMaxHp;
                PlayerHPUpdate?.Invoke(gameData.playerData.playerMaxHp);
                
                //gameData.playerData.playerNowHp = ;
                isPlayerDeath = false;
                Time.timeScale = 1;
            }
            //Debug.Log(gameData.playerData.playerPosition);
            GameDataChanged();
            
        }
        else
        {
            Debug.Log("savefile not exist");
        }
    }

    public void DataUpdate(GameData gamedata)
    {
        gameData=gamedata;
        GameDataChanged();
    }
    public void DataUpdate(PlayerDataForSave playerdata)
    {
        gameData.playerData = playerdata;
        PlayerDataChanged();
       
    }
    int hitnum = 0;
    public void PlayerHit(float damage)
    {
        if (isDodge == true)
        {
            Debug.Log("Dodge");
            gameData.playerData.playerNowHp -= 0;//회피중이면 데미지 0
        }
        else if (isBlock==true)
        {
            Debug.Log("Block");
            gameData.playerData.playerNowHp -= damage/2;
        }
        else
        {
            
            gameData.playerData.playerNowHp -= damage;
            OnPlayerHit?.Invoke();
        }
        if(gameData.playerData.playerNowHp < 0)
        {
            isPlayerDeath=true;
            Time.timeScale = 0;
            OnPlayerDie?.Invoke();
            
        }
        //hitnum++;
        //Debug.Log(hitnum);
        StartCoroutine(Hitduration());
        if (gameData == null)
        {
            //Debug.Log("gamedatanull");
        }
        PlayerHPUpdate?.Invoke(gameData.playerData.playerNowHp);

        //OnPlayerHit?.Invoke();


    }
    public void PlayerHpChanged()
    {
        PlayerHPUpdate?.Invoke(gameData.playerData.playerNowHp);
    }

    public void InventoryButtonPressed()
    {
        OnOpenInventoryButtonClick?.Invoke();
    }
    public void DecideInteractBoxShow()
    {
        if (isCanInteract == true)
        {
            InteractBoxShow?.Invoke();
        }
        else
        {
            OnInteractBoxHide?.Invoke();
        }
    }

    public void AddItemToInventory(Food item)
    {
        GameData.inventory.AddItemToList(item);
        GameDataChanged();
    }
    public void AddItemToInventory(QuestItem item)
    {
        GameData.inventory.AddItemToList(item);
        GameDataChanged();
    }

    public void Quit()
    {
        SceneManager.LoadScene("Title");
    }

}
