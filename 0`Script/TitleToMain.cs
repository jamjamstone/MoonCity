using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleToMain : MonoBehaviour
{
    public TMP_Text text;
    public AudioSource audioSource;
    // Start is called before the first frame update
    
    public void LoadingNextScene()
    {
        audioSource.Play();
        StartCoroutine(Loading());
    }
    IEnumerator Loading()
    {
        var oper = SceneManager.LoadSceneAsync("MainStage");//비동기 방식 -> 로딩할 때는 이게 더 좋을거 같음
        oper.allowSceneActivation = false;
        
        while (oper.isDone == false)
        {
            // Debug.Log(oper.progress);
            if (oper.progress < 0.9f)//0.9넘어가면 활성화
            {
                //loadingBar.value= oper.progress;
            }
            else//로딩이 다 되면
            {
                text.gameObject.SetActive(true);

                if (Input.anyKey)
                {
                    //loading.gameObject.SetActive(false);
                    oper.allowSceneActivation = true;
                }


            }
            yield return null;

        }

        

        oper.allowSceneActivation = true;
        


        //yield return null;

    }
    // Update is called once per frame
    

    
}
