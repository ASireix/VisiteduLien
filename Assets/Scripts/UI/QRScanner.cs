using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QRScanner : MonoBehaviour
{
    public CanvasGroup qrcanvas;
    [SerializeField] ImageTracking imageTracking;
    [SerializeField] Image progressBar;
    [SerializeField] float fadeSpeed;

    void Awake(){
        Evenement.onEventCompleted.AddListener(ShowScanner);
        Evenement.onEventStarted.AddListener(HideScanner);
        imageTracking.onScanProgress.AddListener(UpdateScanProgress);
    }

    public void HideScanner(EventData a = null){
        if (!qrcanvas.gameObject.activeInHierarchy && qrcanvas.alpha == 0) {return;}
        LeanTween.value(1f,0f,fadeSpeed).setOnUpdate((float value)=>{
            qrcanvas.alpha = value;
        }).setOnComplete(()=>{
            qrcanvas.gameObject.SetActive(false);
        });
    }

    public void ShowScanner(EventData a = null){
        if (qrcanvas.gameObject.activeInHierarchy && qrcanvas.alpha == 1) {return;}
        LeanTween.value(0f,1f,fadeSpeed).setOnUpdate((float value)=>{
            qrcanvas.alpha = value;
        }).setOnComplete(()=>{
            qrcanvas.gameObject.SetActive(true);
        });
    }

    public void UpdateScanProgress(float amount){
        progressBar.fillAmount = amount;
    }
}
