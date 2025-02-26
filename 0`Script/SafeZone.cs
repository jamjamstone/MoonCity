using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SafeZone : MonoBehaviour, IInteractable
{


    [SerializeField] float playerDetectRadius = 7f;//감지 기능 어떻게 할까 
    public Transform player;
    public LayerMask playerLayer;
    Vector3 playerPosition;
    public void Interact()
    {
        //PlayerDataForSave temp= new PlayerDataForSave();
        //var playerdata = player.gameObject.GetComponent<PlayerMovement>();
        //temp.playerRotation = playerdata.PlayerRotation;
        //temp.playerPosition=playerdata.PlayerPosition;
        //temp.playerNowHp=playerdata.PlayerNowHp;
        //temp.playerMaxHp=playerdata.PlayerMaxHp;


        //GameManager.Instance.DataUpdate(temp);
        GameManager.Instance.SaveGame();
    }
    //private void OnBecameVisible()
    //{
    //    //StartCoroutine(DetectPlayer());
    //    gameObject.SetActive(true);
    //}
    //private void OnBecameInvisible()
    //{
    //    //StopAllCoroutines();
    //    gameObject.SetActive(false);
    //}
    // Start is called before the first frame update
    //void Start()
    //{
    //
    //
    //}
    private void FixedUpdate()
    {
        DetectPlayer();
    }
    // Update is called once per frame 
    //void Update()
    //{
    //
    //}




    public void DetectPlayer()
    {
        //while (true)
        //{

        //Debug.Log("detecting");
        //Collider[] temp;
        
        Collider[] temp= Physics.OverlapSphere(transform.position, playerDetectRadius, playerLayer);
        //Debug.Log(temp.Length);
        if (temp.Length>0)
        {
            GameManager.isCanInteract = true;
            //playerPosition = new Vector3(temp[0].transform.position.x, temp[0].transform.position.y, temp[0].transform.position.z);
            //player.position = playerPosition;
            //Debug.Log(temp[0].gameObject);
            player = temp[0].transform;
            //if (!GameManager.Instance.interactables.Contains(this))
            //{
            //    GameManager.Instance.OnInteract.AddListener(Interact);
            //    GameManager.Instance.interactables.Add(this);
            //}
            //
            //
            //
            //GameManager.Instance.DecideInteractBoxShow();
        }
        //else if (temp.Length == 0)
        //{
        //    //Debug.Log("temp null");
        //
        //    //Debug.Log(temp[0].gameObject);
        //    player = null;
        //    GameManager.isCanInteract = false;
        //    if (GameManager.Instance.interactables.Contains(this))
        //    {
        //        GameManager.Instance.interactables.Remove(this);
        //    }
        //
        //    GameManager.Instance.OnInteract?.RemoveListener(Interact);
        //    GameManager.Instance.DecideInteractBoxShow();
        //}
        if (player == null)
        {
            return;
        }
        if ((player.position - transform.position).magnitude<=playerDetectRadius)
        {
            //Debug.Log((player.position - transform.position).magnitude);
            if (!GameManager.Instance.interactables.Contains(this))
            {
                GameManager.Instance.OnInteract.AddListener(Interact);
                GameManager.Instance.interactables.Add(this);
            }



            GameManager.Instance.DecideInteractBoxShow();
        }else
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
}


