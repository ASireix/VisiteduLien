using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] LeaderboardElement elementPrefab;
    [SerializeField] int maxElements = 7;
    [Space]
    [SerializeField] Dialogue echecDialogue;
    [SerializeField] Dialogue successDialogue;

    [System.NonSerialized] public UnityEvent onDBUpdates = new UnityEvent();
    [Space]
    [SerializeField] TextAsset titleJson;
    string[] titles;

    LeaderboardElement[] elements;
    private void Awake()
    {
        elements = new LeaderboardElement[maxElements];
        titles = JsonUtility.FromJson<TitleData>(titleJson.text).titles;
    }

    public void UpdateLeaderboard(List<User> users)
    {
        int i = 0;
        users.Reverse();
        while (i < users.Count && i < maxElements - 1)
        {
            if (elements[i] == null)
            {
                elements[i] = Instantiate(elementPrefab,transform,false);
            }
            elements[i].SetLeaderboardElementValues(users[i] == null ? "" : users[i].username,
                                                    users[i] == null ? "" : users[i].title);
            i++;
        }
    }

    public async void PushElement(LeaderboardEntry entry)
    {
        var writeTask = FirebaseStartupManager.instance.
            WriteUser(entry.pseudo, $"Email = {entry.email}; Téléphone = {entry.phone}", SETTINGS.score, entry.hidden, true, SETTINGS.playerID, GetRndTitle());
        if (await Task.WhenAny(writeTask,Task.Delay(2000)) == writeTask){
            successDialogue.TriggerDialogue();
            SETTINGS.isGiveaway = true;
            onDBUpdates?.Invoke();
        }
        else
        {
            echecDialogue.TriggerDialogue();
        }
    }

    public async void RemoveElement()
    {
        var removing = FirebaseStartupManager.instance.GetGiveawayDB().Child(SETTINGS.playerID).RemoveValueAsync();
        await removing;
        if (removing.IsCompleted)
        {
            onDBUpdates?.Invoke();
            SETTINGS.isGiveaway = false;
        }
    }

    string GetRndTitle()
    {
        return titles[Random.Range(0, titles.Length)];
    }
}
