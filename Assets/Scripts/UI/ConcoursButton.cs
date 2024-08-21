using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcoursButton : MonoBehaviour
{
    /// <summary>
    /// This script hides the button in mini game list if the user 
    /// doesnt have registered for the giveaway
    /// </summary>

    private void OnEnable()
    {
        if (!SETTINGS.isGiveaway)
        {
            gameObject.SetActive(false);
        }
    }
}
