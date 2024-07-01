using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FRAMERATE : MonoBehaviour
{
    const int targetFramerate = 60;

    void Awake(){
        Application.targetFrameRate = targetFramerate;
    }
}
