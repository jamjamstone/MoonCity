using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{

    public Button[] buttons;
    bool isEnable=false;
    public TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        buttons = new Button[transform.childCount];
        for(int i = 0; i < transform.childCount; i++)
        {
            buttons[i]=transform.GetChild(i).GetComponent<Button>();
        }

       
        GameManager.Instance.MenuBottonPressed.AddListener(DecideMenuOpen);
        GameManager.Instance.OnPlayerDie.AddListener(ShowYouDie);
        
        gameObject.SetActive(false);
    }


    private void FixedUpdate()
    {
        
    }
    public void ShowYouDie()
    {
        isEnable = true;
        gameObject.SetActive(true);
        text.gameObject.SetActive(true);//error
    }
    // Update is called once per frame
    //void Update()
    //{
    //    
    //}
    
    public void DecideMenuOpen()
    {
        if (isEnable == true)//메뉴가 켜져 있다면
        {

            gameObject.SetActive(false);//메뉴를 끄고
            text.gameObject.SetActive(false);
            isEnable = false;//플래그 끄기
        }
        else//메뉴가 켜져있지 않다면
        {
            isEnable = true;
            gameObject.SetActive(true);
            text.gameObject.SetActive(false);
        }
    }
    IEnumerator PressMenu()
    {
        yield return null;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.MenuBottonPressed.RemoveAllListeners();
        }
    }



}
