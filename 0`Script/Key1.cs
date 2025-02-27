using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key1 : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            for(int i=0;i<GameManager.endingFlags.Length;i++)
            {
                if (GameManager.endingFlags[i] == false)
                {
                    GameManager.endingFlags[i] = true;
                    //Debug.Log(GameManager.endingFlags[i]);
                    Destroy(gameObject);
                    return;
                }
            }
        }
        
    }
}
