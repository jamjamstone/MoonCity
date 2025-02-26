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
public class GameData//�������� ������ �ְ� ���� -> ���ۺ��� �������� ������ ������ ������ �̿� ���� ������
{
    string description = "GameData";
    public Dictionary<string, bool> gameFlags;// ���ӳ��� �÷��� �̸��� Ȱ��ȭ ����
    public Dictionary<string, Item> allItems;//������ �̸��� ������ ����
    public Dictionary<string, Monster> monsters;// ���� �̸��� ���� ���� 
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
    //public GameData Clone()// json ������ �̿��� ������ ������ Ŭ�����δ� �Ұ��� -> �÷��̾� ������ ���� �����ҵ� 
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
