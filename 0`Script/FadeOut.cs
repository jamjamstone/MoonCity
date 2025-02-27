using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    public Image fadeOut;
    float colorAlpha = 0;
    Color color = new Color();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartFadeOut()
    {
        Debug.Log("Start");
        StartCoroutine(SceneFadeOut());
    }

    IEnumerator SceneFadeOut()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.05f);
            colorAlpha += Time.deltaTime;
            color=Color.white;
            color.a= colorAlpha;
            fadeOut.color= color;





        }
    }



}
