using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SETTINGS
{
    public static bool isTutorialCompleted
    {
        get
        {
            return PlayerPrefs.GetInt("isTutorialCompleted").Equals(1);
        }
        set
        {
            PlayerPrefs.SetInt("isTutorialCompleted", value ? 1 : 0);
        }
    }

    public static bool isGuidee
    {
        get
        {
            return PlayerPrefs.GetInt("isGuidee").Equals(1);
        }
        set
        {
            PlayerPrefs.SetInt("isGuidee", value ? 1 : 0);
        }
    }
}
