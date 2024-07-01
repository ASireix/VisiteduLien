using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(Animator))]
public abstract class Evenement : MonoBehaviour
{
    public EventData eventData;
    protected Animator eventAnimator;
    [SerializeField] protected Patrick patrick;
    [System.NonSerialized]
    public static UnityEvent<EventData> onEventCompleted = new UnityEvent<EventData>();
    [System.NonSerialized]
    public static UnityEvent<EventData> onEventStarted = new UnityEvent<EventData>();

    protected AnimatorOverrideController aoc;
    protected AnimationClipOverrides clipOverrides;
    bool init;

    void Awake()
    {
        eventAnimator = GetComponent<Animator>();
        aoc = new AnimatorOverrideController(eventAnimator.runtimeAnimatorController);
        eventAnimator.runtimeAnimatorController = aoc;

        clipOverrides = new AnimationClipOverrides(aoc.overridesCount);
        aoc.GetOverrides(clipOverrides);
        OnStart();
        init = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Start function is called in OnEnabled
        //LaunchStart();
    }

    void LaunchStart()
    {
        onEventStarted?.Invoke(eventData);
        if (eventData.isCompleted)
        {
            CompletedStart();
            return;
        }
        if (SETTINGS.isGuidee)
        {
            GuideeStart();
        }
        else
        {
            JoueeStart();
        }
    }

    protected virtual void OnStart() { }

    public abstract void GuideeStart();

    public abstract void JoueeStart();

    public abstract void CompletedStart();

    public virtual void CompleteEvent()
    {
        eventData.isCompleted = true;
        try
        {
            SaveManager.instance.saveSystem.Save();
        }
        catch
        {
            Debug.Log("Save mananager is missing");
        }
        
        onEventCompleted?.Invoke(eventData);
        if (transform.parent.gameObject.name == gameObject.name){
            transform.parent.gameObject.SetActive(false);
        }else{
            gameObject.SetActive(false);
        }
        
        EnterARState();
    }

    protected virtual void Reset()
    {
        Debug.Log("Empty reset");
    }

    void OnEnable()
    {
        if (init)
        {
            LaunchStart();
        }
    }


    public void EnterARState()
    {
        ARSessionManager ar = FindObjectOfType<ARSessionManager>();
        ar.EnterARState();
    }
}
