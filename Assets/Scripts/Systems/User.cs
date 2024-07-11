using System;
using System.Collections.Generic;

public class User
{
    public string username;
    public string date;
    public string contact;
    public int score;
    public bool hidden;
    public string title;

    public User(string username, string contact, string date, int score, bool hidden, string title = "")
    {
        this.username = username;
        this.contact = contact;
        this.score = score;
        this.date = date;
        this.hidden = hidden;
        this.title = title;
    }

    public Dictionary<string, Object> ToDictionary()
    {
        Dictionary<string, Object> result = new Dictionary<string, Object>();

        result["username"] = username;
        result["contact"] = contact;
        result["score"] = score;
        result["date"] = date;
        result["hidden"] = hidden;
        result["title"] = title;
        return result;
    }
}
