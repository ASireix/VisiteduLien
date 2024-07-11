using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardEntry
{
    public string pseudo;
    public string contact;
    public bool hidden;

    public LeaderboardEntry(string pseudo, string contact, bool hidden)
    {
        this.pseudo = pseudo;
        this.contact = contact;
        this.hidden = hidden;
    }
}
