using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LookPlayer());
    }

    // Update is called once per frame
    //void Update()
    //{
    //    
    //}

    IEnumerator LookPlayer()
    {
        yield return new WaitForSeconds(5f);
        transform.LookAt(player.transform.position);



    }


}
