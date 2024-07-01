using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARSessionManager : MonoBehaviour
{
    [SerializeField] ARSession session;
    [SerializeField] QRScanner scanner;
    [SerializeField] GameObject background;

    public void EnterARState(){
        session.enabled = true;
        scanner.ShowScanner();
        background.SetActive(false);
    }

    public void ExitARSate(){
        session.enabled = false;
        scanner.HideScanner();
        background.SetActive(true);
    }
}
