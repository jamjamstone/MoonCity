using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{

    public Button[] buttons;
    bool isEnable=false;

    // Start is called before the first frame update
    void Start()
    {
        buttons = new Button[transform.childCount];
        for(int i = 0; i < transform.childCount; i++)
        {
            buttons[i]=transform.GetChild(i).GetComponent<Button>();
        }

       
        GameManager.Instance.MenuBottonPressed.AddListener(DecideMenuOpen);
        gameObject.SetActive(false);
    }


    private void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    //void Update()
    //{
    //    
    //}
    
    public void DecideMenuOpen()
    {
        if (isEnable == true)//�޴��� ���� �ִٸ�
        {

            gameObject.SetActive(false);//�޴��� ����
            isEnable = false;//�÷��� ����
        }
        else//�޴��� �������� �ʴٸ�
        {
            isEnable = true;
            gameObject.SetActive(true);
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
            GameManager.Instance.MenuBottonPressed.RemoveListener(DecideMenuOpen);
        }
    }



}
