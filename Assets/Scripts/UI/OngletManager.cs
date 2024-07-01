using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OngletManager : MonoBehaviour
{
    Onglet[] onglets;
    [SerializeField] GameObject swipeTutorial;

    void Start()
    {
        onglets = GetComponentsInChildren<Onglet>();

        for (int i = 0; i < onglets.Length; i++)
        {
            onglets[i].onOngletOpened.AddListener(OpenOnglet);
        }
    }

    void OpenOnglet(bool open, Onglet onglet)
    {
        swipeTutorial.SetActive(false);
        
        if (open)
        {
            for (int i = 0; i < onglets.Length; i++)
            {
                if (onglets[i] != onglet)
                {
                    onglets[i].leanTweenTransition.TriggerTransition(TransitionDirection.Left);
                }
            }
        }
        else
        {
            for (int i = 0; i < onglets.Length; i++)
            {
                if (onglets[i] != onglet)
                {
                    onglets[i].leanTweenTransition.TriggerTransition(TransitionDirection.Right);
                }
            }
        }
    }
}
