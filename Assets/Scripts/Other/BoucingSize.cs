using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoucingSize : MonoBehaviour
{
    [SerializeField] Vector3 minSize;
    [SerializeField] float bounceSpeed;
    Vector3 maxSize; //default size of the object
    void Start(){
        maxSize = transform.localScale;
        BounceDown();
    }

    void BounceDown(){
        LeanTween.scale(gameObject, minSize, bounceSpeed).setEaseOutBounce().setOnComplete(BounceUp);
    }

    void BounceUp(){
        LeanTween.scale(gameObject, maxSize, bounceSpeed).setEaseInCirc().setOnComplete(BounceDown);
    }
}
