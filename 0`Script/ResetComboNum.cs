using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetComboNum : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
    
    public void ResetCombo()
    {
        animator.SetInteger("ComboNum", 0);
    }
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
}
