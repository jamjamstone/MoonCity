using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPoint : MonoBehaviour
{
    // Start is called before the first frame update
    
    public void Loading()
    {
         SceneManager.LoadScene("EndingFront");//비동기 방식 -> 로딩할 때는 이게 더 좋을거 같음
        
            



        

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log(GameManager.endingFlags[3]);
            if (GameManager.endingFlags[3] == true)
            {
                Loading();
            }
        }
    }



}
