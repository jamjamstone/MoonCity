using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBox : MonoBehaviour
{

    LayerMask player; 
    public Monster monster;
    bool isTouched = false;
    // Start is called before the first frame update
    void Start()
    {
        //monster=transform.root.GetComponent<Monster>();
        player = LayerMask.NameToLayer("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("hit");
    //    if (collision.gameObject.layer == player)
    //    {
    //        GameManager.Instance.PlayerHit(monster.monsterDmg);
    //    }
    //}
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("hit");
        if (other.gameObject.layer == player)
        {
            if (GameManager.isPlayerHit == false)
            {
                GameManager.Instance.PlayerHit(monster.monsterDmg);
                isTouched = true;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == player)
        {
            //GameManager.Instance.PlayerHit(monster.monsterDmg);
            isTouched = false;
        }
    }
}
