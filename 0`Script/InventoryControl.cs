using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine;
using System.IO;
using UnityEngine.UI;



public class InventoryControl : MonoBehaviour// �κ��丮 ��Ʈ���� �κ��丮�� �׸��� ��Ȱ ĵ������ ǥ���ϰ� ������ ��ȭ�� ������ �ϴ� �ϼ� ��� �׽�Ʈ �� ui ������ ����
{
    public List<GameObject> invenItems;
    public Inventory playerInventory;
    //private string filePath; //= Path.Combine(Application.persistentDataPath, "inventory.json");//�κ��丮�� ���ӸŴ������� ��������
    public GameObject foodPanel;
    public GameObject questPanel;
    public GameObject foodPrefab;
    //bool isInvnetoryOpen
    // Start is called before the first frame update
    void Start()
    {
        invenItems = new List<GameObject>();
        gameObject.SetActive(false);
        //GameManager.Instance.Test();
        GameManager.Instance?.OnOpenInventoryButtonClick?.AddListener(DecideOpen);
        //GameManager.Instance?.OnOpenInventoryButtonClick?.AddListener(DrawInventory);
        GameManager.Instance.GameDataChange += UpdateInventory;
        GameManager.Instance.OnUseFoodButtonClick.AddListener(UseFood);

        //GameManager.Instance?.OnOpenInventoryButtonClick?.AddListener(DecideOpen);

        playerInventory = new Inventory();

        //playerInventory =GameManager.Instance.GameData.inventory;//error

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void UpdateInventory(GameData gamedata)
    {
        Debug.Log("upadate inven");
        playerInventory = gamedata.inventory;
        DrawInventory();
    }
    public void UseFood()
    {
        //playerInventory.UseItem(0);
        GameManager.Instance.GameData.inventory.UseItem(0);
        GameManager.Instance.GameDataChanged();




    }
    public void OpenInventory()
    {
        //Debug.Log("openinven");
        Debug.Log(playerInventory.items[0].itemCount);
        gameObject.SetActive(true);
    }
    public void SaveInventoryToJson()
    {
        string json = JsonUtility.ToJson(playerInventory);
    }
    public void CloseInventory()
    {
        //Debug.Log("closeinven");
        gameObject.SetActive(false);
    }

    public void DecideOpen()
    {
        if (GameManager.isInventoryOpen == true)//�κ��� �̹� �����ִٸ�
        {
            CloseInventory();
            GameManager.isInventoryOpen = false;
        }
        else
        {
            OpenInventory();
            GameManager.isInventoryOpen = true;
        }
        //Debug.Log("decide");



    }
    

    public void ClearInventory()
    {
        for (int i = 0; i < foodPanel.transform.childCount; i++)
        {
            Destroy(foodPanel.transform.GetChild(i).gameObject);
        }
    }
    public void DrawInventory()
    {
        Debug.Log("draw method work");
        ClearInventory();
        //Debug.Log("draw");
        for (int i = 0; i < playerInventory.items.Count; i++)
        {
            var item = Instantiate(foodPrefab, foodPanel.transform);
            //if (invenItems.Contains(item))
            //{
            //    Debug.Log("Detect clone");
            //    Destroy(item);
            //    continue;
            //}
            Debug.Log(playerInventory.items[i].itemCount);
            item.transform.GetComponentInChildren<Text>().text = playerInventory.items[i].itemCount.ToString();//������ ī��Ʈ ǥ��
            item.transform.GetComponentInChildren<RawImage>().texture = playerInventory.items[i].icon;
            invenItems.Add(item);



        }


            //    foreach (var i in playerInventory?.items)//error
            //{
            //    var item = Instantiate(foodPrefab, foodPanel.transform);//foodpanel ���� ������Ʈ�� �߰�
            //    item.transform.GetComponentInChildren<Text>().text = i.Value.itemCount.ToString();//������ ī��Ʈ ǥ��
            //    item.transform.GetComponentInChildren<RawImage>().texture = i.Value.icon;//������ ǥ��
            //
            //
            //    //foodPanel.transform.GetChild()
            //    //i.Value.itemCount;
            //}
        //}
        
    }




    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnOpenInventoryButtonClick.RemoveListener(DecideOpen);
        }
        //string json= JsonUtility.ToJson(playerInventory);
    }
}
