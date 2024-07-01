using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cadenas : MonoBehaviour
{
    [SerializeField] GameObject cadenas;
    [SerializeField] Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        Evenement.onEventCompleted.AddListener(ShowCadenasAnim);
    }

    void ShowCadenasAnim(EventData eventData){
        animator.SetTrigger("Unlock");
    }
}
