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

    public static bool isCompleted
    {
        get
        {
            return PlayerPrefs.GetInt("isCompleted").Equals(1);
        }
        set
        {
            PlayerPrefs.SetInt("isCompleted", value ? 1 : 0);
        }
    }

    public static int score
    {
        get
        {
            return PlayerPrefs.GetInt("Score");
        }set
        {
            PlayerPrefs.SetInt("Score", value);
        }
    }

    /// <summary>
    /// True if the player has already an entry in the giveaway
    /// </summary>
    public static bool isGiveaway
    {
        get
        {
            return PlayerPrefs.GetInt("isGiveaway").Equals(1);
        }
        set
        {
            PlayerPrefs.SetInt("isGiveaway", value ? 1 : 0);
        }
    }

    public static string playerID
    {
        get
        {
            return PlayerPrefs.GetString("playerID", "");
        }
        set
        {
            PlayerPrefs.SetString("playerID", value);
        }
    }
}
