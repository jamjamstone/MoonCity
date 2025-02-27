using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;



public class InventoryControl : MonoBehaviour// 인벤토리 컨트롤은 인벤토리를 그리는 역활 캔버스에 표시하고 데이터 변화를 저장함 일단 완성 기능 테스트 및 ui 디자인 남음
{
    public List<GameObject> invenItems;
    public Inventory playerInventory;
    //private string filePath; //= Path.Combine(Application.persistentDataPath, "inventory.json");//인벤토리는 게임매니저에서 관리하자
    public GameObject foodPanel;
    public GameObject questPanel;
    public GameObject foodPrefab;
    public TMP_Text flagNum;

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
    //void Update()
    //{
    //
    //}
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
        Debug.Log(GameManager.Instance.GameData.inventory.items[0].itemCount);
        GameManager.Instance.PlayerHpChanged();
        UpdateInventory(GameManager.Instance.GameData);




    }
    public void OpenInventory()
    {
        //Debug.Log("openinven");
       // Debug.Log(playerInventory.items[0].itemCount);
        gameObject.SetActive(true);
        StartCoroutine(ShowFlag());
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
        if (GameManager.isInventoryOpen == true)//인벤이 이미 열려있다면
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
    IEnumerator ShowFlag()
    {
        while (true)
        {
            int num = 0;
            for (int i = 0; i < GameManager.endingFlags.Length; i++)
            {
                if (GameManager.endingFlags[i] == true)
                {
                    num++;
                }
            }
            string text = "Gathering Key " + num;
            flagNum.text = text;

            yield return new WaitForSeconds(1f);   
        }
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
            item.transform.GetComponentInChildren<Text>().text = playerInventory.items[i].itemCount.ToString();//아이템 카운트 표시
            item.transform.GetComponentInChildren<RawImage>().texture = playerInventory.items[i].icon;
            invenItems.Add(item);



        }


            //    foreach (var i in playerInventory?.items)//error
            //{
            //    var item = Instantiate(foodPrefab, foodPanel.transform);//foodpanel 하위 오브젝트로 추가
            //    item.transform.GetComponentInChildren<Text>().text = i.Value.itemCount.ToString();//아이템 카운트 표시
            //    item.transform.GetComponentInChildren<RawImage>().texture = i.Value.icon;//아이콘 표시
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
