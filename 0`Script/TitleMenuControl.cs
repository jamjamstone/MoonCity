using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMenuControl : MonoBehaviour
{

    public AudioSource audio;




    public void QuitGame()
    {
        audio.Play();
        Application.Quit();
    }
    
}
