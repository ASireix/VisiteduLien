using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenSize : MonoBehaviour
{
    [SerializeField] Vector3 startSize = Vector3.one;
    [SerializeField] Vector3 minimalSize;

    void Start(){
        
    }
    void OnEnable(){
        transform.localScale = minimalSize;
        LeanTween.scale(gameObject,startSize, 1.7f).setEaseOutBounce();
    }

}
