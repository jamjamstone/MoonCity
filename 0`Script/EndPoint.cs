using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPoint : MonoBehaviour
{
    // Start is called before the first frame update
    
    public void Loading()
    {
         SceneManager.LoadScene("EndingFront");//�񵿱� ��� -> �ε��� ���� �̰� �� ������ ����
        
            



        

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
