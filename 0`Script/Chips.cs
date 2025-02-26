using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chips : MonoBehaviour,IInteractable
{
    public Food chips;
    public Texture icon;
    public Transform player;
    public LayerMask playerLayer;
    public float playerDetectRadius = 3f;
    public void Interact()
    {
        if (player != null)
        {
            GameManager.Instance.GameData.inventory.GetItem(chips);
            Debug.Log(GameManager.Instance.GameData.inventory.items[0].itemCount);
            GameManager.Instance.GameDataChanged();
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        chips= new Food();
        chips.healAmount = 20;
        chips.itemDescription = "Potato Chips, Heal 20points";
        chips.itemName = "Chips";
        chips.icon= icon;

        //Debug.Log(GameManager.Instance.GameData.inventory);
        //
        //
        //GameManager.Instance.GameData.inventory.AddItemToList(chips);//error
        GameManager.Instance.OnInteract.AddListener(Interact);
        
        

    }

    // Update is called once per frame
    //void Update()
    //{
    //    
    //}
    private void FixedUpdate()
    {
        DetectPlayer();
    }

    public void DetectPlayer()
    {
        
        Collider[] temp = Physics.OverlapSphere(transform.position, playerDetectRadius, playerLayer);
        ;
        if (temp.Length > 0)
        {
            GameManager.isCanInteract = true;
            
            player = temp[0].transform;
            
        }
        
        if (player == null)
        {
            return;
        }
        if ((player.position - transform.position).magnitude <= playerDetectRadius)
        {
            //Debug.Log((player.position - transform.position).magnitude);
            if (!GameManager.Instance.interactables.Contains(this))
            {
                GameManager.Instance.OnInteract.AddListener(Interact);
                GameManager.Instance.interactables.Add(this);
            }



            GameManager.Instance.DecideInteractBoxShow();
        }
        else
        {
            // Debug.Log((player.position - transform.position).magnitude);
            player = null;
            GameManager.isCanInteract = false;
            if (GameManager.Instance.interactables.Contains(this))
            {
                GameManager.Instance.interactables.Remove(this);
            }

            GameManager.Instance.OnInteract?.RemoveListener(Interact);
            GameManager.Instance.DecideInteractBoxShow();
        }


    }



    private void OnDestroy()
    {
        player = null;
        GameManager.isCanInteract = false;
        if (GameManager.Instance.interactables.Contains(this))
        {
            GameManager.Instance.interactables.Remove(this);
        }
    
        GameManager.Instance.OnInteract?.RemoveListener(Interact);
        GameManager.Instance.DecideInteractBoxShow();
    }

}
