using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct TransitionAction{
    public string name;
    public MonoBehaviour monoBehaviour;
    public string methodName;
    public bool isCoroutine;

    public TransitionAction(string _name, MonoBehaviour _monoBehavior, string _methodName, bool _isCoroutine){
        name = _name;
        monoBehaviour = _monoBehavior;
        methodName = _methodName;
        isCoroutine = _isCoroutine;
    }

    public IEnumerator Execute(){
        if (isCoroutine){
            yield return monoBehaviour.StartCoroutine(methodName);
        }else{
            monoBehaviour.Invoke(methodName,0f);
            yield return null;
        }
    }
}
public class TransitionManager : MonoBehaviour
{
    [SerializeField] List<TransitionAction> transitionActions;
    [SerializeField] float intractLength;

    IEnumerator StartTransitions(){
        foreach(var action in transitionActions){
            yield return StartCoroutine(action.Execute());
            yield return new WaitForSeconds(intractLength);
        }
    }

    public void StartTransition(){
        StartCoroutine(StartTransitions());
    }
}

