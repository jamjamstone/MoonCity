using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fadein : MonoBehaviour
{
    // Start is called before the first frame update
    public Image image;
    float colorAlpha = 1;
    Color color = new Color();
    public void FadeInStart()
    {
        StartCoroutine(FadeInImage());
    }
    IEnumerator FadeInImage()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.05f);

            colorAlpha -= Time.deltaTime;
            color = Color.white;
            color.a = colorAlpha;
            image.color = color;



        }
    }

}
