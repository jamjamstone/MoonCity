using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvasControl : MonoBehaviour
{

    public Canvas menu;
    public Canvas inventory;
    public Canvas inGameUi;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.MenuBottonPressed?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            //GameManager.Instance.InventoryButtonPressed();
        }
        //DecideAction();
    }


    public void DecideAction()
    {
        
    }
}
