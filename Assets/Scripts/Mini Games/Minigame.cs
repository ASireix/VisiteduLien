using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Minigame : MonoBehaviour
{
    public UnityEvent onMiniGameFinished = new UnityEvent();

    public abstract void StartMiniGame();

    protected virtual void EndMiniGame()
    {
        onMiniGameFinished?.Invoke();
    }

    
}
