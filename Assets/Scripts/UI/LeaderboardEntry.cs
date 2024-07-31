using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardEntry
{
    public string pseudo;
    public string email;
    public string phone;
    public bool hidden;

    public LeaderboardEntry(string pseudo, string email, string phone, bool hidden)
    {
        this.pseudo = pseudo;
        this.email = email;
        this.phone = phone;
        this.hidden = hidden;
    }
}
