using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Patrick : MonoBehaviour
{
    [SerializeField] Animator patrickAnim;

    AnimatorOverrideController aoc;
    protected AnimationClipOverrides clipOverrides;
    [SerializeField] UnityEvent onStartEvent;

    private void Start()
    {
        //StartWave();
        aoc = new AnimatorOverrideController(patrickAnim.runtimeAnimatorController);
        patrickAnim.runtimeAnimatorController = aoc;

        clipOverrides = new AnimationClipOverrides(aoc.overridesCount);
        aoc.GetOverrides(clipOverrides);

        onStartEvent?.Invoke();
    }

    public void LaunchCustomAnimationClip(AnimationClip clip)
    {
        clipOverrides["Custom Animation"] = clip;
        aoc.ApplyOverrides(clipOverrides);
        patrickAnim.SetTrigger("CustomTrigger");
    }

    #region Launch Animations


    /// <summary>
    /// Launch animation using Trigger
    /// </summary>
    /// <param name="animation_Name"></param>
    public void LaunchAnimation(string animation_Name)
    {
        patrickAnim.SetTrigger(animation_Name);
        patrickAnim.SetBool("Hold", true);
    }

    public void LaunchAnimationSingle(string animation_Name)
    {
        patrickAnim.SetTrigger(animation_Name);
        patrickAnim.SetBool("Hold", false);
    }

    public void ResetPatrickToidle()
    {
        patrickAnim.SetBool("Hold", false);
    }

    /// <summary>
    /// Launch Animation using float and value
    /// </summary>
    /// <param name="animation_Name"></param>
    /// <param name="state"></param>
    public void LaunchAnimation(string animation_Name, float state)
    {
        patrickAnim.SetFloat(animation_Name, state);
    }

    public void ActivateAnimation(string bool_Name)
    {
        patrickAnim.SetBool(bool_Name, true);
    }

    public void DeactivateAnimation(string bool_Name)
    {
        patrickAnim.SetBool(bool_Name, false);
    }
    #endregion
    #region Animations Trigger

    public void StartWave()
    {
        patrickAnim.SetBool("isWaving", true);
    }

    public void StopWave()
    {
        patrickAnim.SetBool("isWaving", false);
    }


    public void TriggerPoint()
    {
        patrickAnim.SetTrigger("Point");
    }

    public void TriggerClap()
    {
        patrickAnim.SetTrigger("Clap");
    }

    public void StartJul()
    {
        patrickAnim.SetBool("isJul", true);
    }

    public void StopJul()
    {
        patrickAnim.SetBool("isJul", false);
    }
    #endregion
}


