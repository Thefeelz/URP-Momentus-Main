using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_AbilityIcon : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ButtonAbilityReadyComplete() { anim.SetBool("AbilityReady", false); }
    public void ButtonAbilityReadyStart() 
    {
        anim.SetBool("AbilityReady", true);
        GetComponent<Image>().fillAmount = 1;
    }
}
