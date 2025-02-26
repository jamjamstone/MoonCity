using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[SerializeField]
public class Item 
{
    public string itemName;
    public string itemDescription;
    public int itemCount;//���� ������ �κ��丮�� �Ҵ��ϴ� ���� ���ƺ��� -> ���ǻ� �������� �����°� ���غ���

}

public class InventoryForJson
{
    public List<Item> items;
}

[SerializeField]
public class Food : Item
{
    public float healAmount;
    public Texture icon;
    public void UseFood(ref float playerHP)
    {
        
        playerHP += healAmount;
        if(playerHP > GameManager.Instance.GameData.playerData.playerMaxHp)
        {
            playerHP = GameManager.Instance.GameData.playerData.playerMaxHp;
        }
    }
}

[SerializeField]
public class QuestItem:Item
{
    public string flagName;
    //public bool flagSet;
}

[SerializeField]
public class Inventory//�����͸� ������ ���� 
{
    public Dictionary<int, Food> items;
    public Dictionary<int, QuestItem> questItems;
    
    public Inventory()
    {
        items = new Dictionary<int, Food>();
        questItems = new Dictionary<int, QuestItem>();
    }

    public void AddItemToList(Food item)//�������� �κ��丮�� �߰�
    {
        
        for(int i=0;i<items.Count;i++)
        {
            if(items[i].itemName==item.itemName)
            {
                
                items[i].itemCount++;
                return;
            }
        }
        items.Add(items.Count, item);



        //if (!items.ContainsValue(item))
        //{
        //    items.Add(items.Count, item);
        //}
        //else
        //{
        //    return;
        //}
    }
    public void AddItemToList(QuestItem item)//����Ʈ �������� �κ��丮�� �߰�
    {
        for (int i = 0; i < questItems.Count; i++)
        {
            if (questItems[i].itemName == item.itemName)
            {
                questItems[i].itemCount++;

                return;
            }
        }
        questItems.Add(questItems.Count, item);
    }
    public void RemoveItemFromList(Food item)//�������� �κ��丮���� ����
    {

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemName == item.itemName)
            {
                items[i]=null;

                return;
            }
        }
        
    }
    public void RemoveItemFromList(QuestItem item)//�������� �κ��丮���� ����
    {

        for (int i = 0; i < questItems.Count; i++)
        {
            if (questItems[i].itemName == item.itemName)
            {
                questItems[i] = null;

                return;
            }
        }
        
    }
    public void UseItem(int itemNumber)//������ ���
    {
        Food useditem=new Food();
        items.TryGetValue(itemNumber, out useditem);
        if (useditem != null)
        {
            if (useditem is Food)
            {
                if (useditem.itemCount > 0)
                {
                    (useditem as Food).UseFood(ref GameManager.Instance.GameData.playerData.playerNowHp);
                    useditem.itemCount -= 1;
                }
                else
                {
                    return;
                }
            }
        }
    }
    public void GetItem(Food item)
    {
        for (int i = 0; i < questItems.Count; i++)
        {
            if (questItems[i].itemName == item.itemName)
            {
                questItems[i].itemCount++;

                return;
            }
        }
        AddItemToList(item);
        //if (items.ContainsValue(item))
        //{
        //    Food temp = new Food();
        //    items.TryGetValue(GetIndex(item), out temp);
        //    temp.itemCount++;
        //}
        //else
        //{
        //    AddItemToList(item);
        //}
    }
    public void GetItem(QuestItem item)
    {
        for (int i = 0; i < questItems.Count; i++)
        {
            if (questItems[i].itemName == item.itemName)
            {
                questItems[i].itemCount++;

                return;
            }
        }
        AddItemToList(item);
    }
    public int GetIndex(Food item)
    {
        Food temp = new Food();
        if (items.ContainsValue(item))
        {
            for (int i = 0; i < items.Count; i++)
            {
                items.TryGetValue(i, out temp);
                if (temp == item)
                {
                    
                    return i;
                }
                
            }
            return -1;
        }
        else
        {
            return -1;
        }
    }
    public int GetIndex(QuestItem item)
    {
        QuestItem temp = new QuestItem();
        if (questItems.ContainsValue(item))
        {
            for (int i = 0; i < questItems.Count; i++)
            {
                questItems.TryGetValue(i, out temp);
                if (temp == item)
                {

                    return i;
                }

            }
            return -1;
        }
        else
        {
            return -1;
        }
    }
}

