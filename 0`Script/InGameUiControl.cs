using System.Collections;
using System.Collections.Generic;
using MagicPigGames;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUiControl : MonoBehaviour
{
    //public TextMeshPro 
    public TMP_Text interactBox;
    public ProgressBar hpBar;
    public float lastProgress;


    // Start is called before the first frame update
    void Start()
    {
        interactBox.gameObject.SetActive(false);
        GameManager.Instance.InteractBoxShow.AddListener(ShowText);
        GameManager.Instance.OnInteractBoxHide.AddListener(DisableText);
        GameManager.Instance.PlayerDataChange += UpdateHpBar;
        
    }

    // Update is called once per frame
    //void Update()
    //{
    //    
    //}
    public void ShowText()
    {
        interactBox.gameObject.SetActive(true);
    }

    public void DisableText()
    {
        interactBox.gameObject.SetActive(false);
    }
    public void UpdateHpBar(PlayerDataForSave player)
    {
        lastProgress=player.playerNowHp/ player.playerMaxHp ;
        hpBar.SetProgress(lastProgress);
    }





}
