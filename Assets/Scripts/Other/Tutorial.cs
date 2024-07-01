using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.UI;
using UnityEngine.XR.ARCore;
using UnityEngine.XR.ARFoundation;

public class Tutorial : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] bool enableTurorial = true;
    [SerializeField] bool forceTutorial = true;
    [SerializeField] float waitingTime = 3f;
    [SerializeField] int blinkAmount = 10;
    [Range(0f,2f)]
    [SerializeField] float blinkSpeed = 0.5f;

    [Header("Objects to handle")]
    [SerializeField] GameObject mapButton;
    [SerializeField] GameObject ARsession;

    [SerializeField] GameObject coworkingBackground;

    [SerializeField] Dialogue dialogueTutorial;

    [SerializeField] Patrick PatrickInScene;

    // Start is called before the first frame update
    void Start()
    {
        if ((!SETTINGS.isTutorialCompleted && enableTurorial) || forceTutorial)
        {
            mapButton.SetActive(false);
            ARsession.GetComponent<ARSession>().enabled = false;
            ARsession.GetComponent<QRScanner>().HideScanner();
            
            coworkingBackground.SetActive(true);
            PatrickInScene.gameObject.SetActive(true);

            dialogueTutorial.onDialogueComplete.AddListener(OnFirstDialogueComplete);

            StartCoroutine(DelayStart());
            PatrickInScene.StartWave();
        }else{
            OnFirstDialogueComplete();
        }
    }


    IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(waitingTime);

        dialogueTutorial.TriggerDialogue();
        PatrickInScene.StopWave();
    }

    void OnFirstDialogueComplete()
    {
        mapButton.SetActive(true);
        ARsession.GetComponent<QRScanner>().ShowScanner();
        ARsession.GetComponent<ARSession>().enabled = true;
        coworkingBackground.SetActive(false);
        PatrickInScene.gameObject.SetActive(false);
        SETTINGS.isTutorialCompleted = true;
    }

    public void ShowMapButton()
    {
        mapButton.SetActive(true);
        StartCoroutine(BlinkGameObject(blinkAmount,blinkSpeed,mapButton,mapButton.GetComponent<Button>()));
    }

    IEnumerator BlinkGameObject(int amount, float speed, GameObject blinkingObject, Button buttonComponent = null){
        bool defaultState = blinkingObject.activeInHierarchy;
        if (buttonComponent) buttonComponent.interactable = false;
        for(int i = 0; i<amount; i++){            
            yield return new WaitForSeconds(speed);
            blinkingObject.SetActive(!blinkingObject.activeInHierarchy);
        }

        blinkingObject.SetActive(defaultState);
        if (buttonComponent) buttonComponent.interactable = true;
    }
}
