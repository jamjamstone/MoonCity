using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum MonsterState
{
    None=0,
    Idle,
    Move,
    Detect,
    Attack,
    Hit,
    Die = 9,

}




[SerializeField]
public class GameData//아이템은 가지고 있게 하자 -> 시작부터 아이템의 종류가 정해저 있으니 이에 따라서 만들자
{
    string description = "GameData";
    public Dictionary<string, bool> gameFlags;// 게임내의 플래그 이름과 활성화 여부
    public Dictionary<string, Item> allItems;//아이템 이름과 아이템 정보
    public Dictionary<string, Monster> monsters;// 몬스터 이름과 몬스터 정보 
    public Inventory inventory;
    public PlayerDataForSave playerData;

    public GameData()
    {
        gameFlags = new Dictionary<string, bool>();
        allItems = new Dictionary<string, Item>();
        monsters = new Dictionary<string, Monster>();
        inventory = new Inventory();
        playerData = new PlayerDataForSave();
    }
    public GameData(GameData cloneData)
    {
        description = cloneData.description;
        //gameFlags = cloneData.gameFlags.Clone()
    }
    //public GameData Clone()// json 파일을 이용한 저장은 복잡한 클래스로는 불가능 -> 플레이어 정보만 저장 가능할듯 
    //{
    //    GameData cloneData = new GameData();
    //    cloneData.description = description;
    //    cloneData.allItems = new Dictionary<string, Item>();
    //    cloneData.gameFlags = new Dictionary<string, bool>();
    //    cloneData.monsters = new Dictionary<string, Monster>();
    //    cloneData.inventory = new Inventory();
    //    cloneData.playerData = new PlayerDataForSave();
    //    //cloneData.allItems
    //
    //    return cloneData;
    //}
    // Start is called before the first frame update
   
}
