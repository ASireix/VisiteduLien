using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardElement : MonoBehaviour
{
    public TextMeshProUGUI pseudo;
    public TextMeshProUGUI title;

    public void SetLeaderboardElementValues(string pseudo, string title)
    {
        this.pseudo.text = pseudo;
        this.title.text = title;
    }
}
