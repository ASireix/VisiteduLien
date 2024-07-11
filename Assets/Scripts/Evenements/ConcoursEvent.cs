using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConcoursEvent : Evenement
{
    [SerializeField] Dialogue explication;
    [SerializeField] Dialogue encore;

    [Header("Match Informations")]
    public Formulaire formulaire;
    public Leaderboard leaderboard;

    [SerializeField] SceneController sceneController;

    protected override void OnStart()
    {
        base.OnStart();
        leaderboard.onDBUpdates.AddListener(UpdateInfoAsync);
    }


    public override void CompletedStart()
    {
        StartEvent();
    }

    public override void GuideeStart()
    {
        StartEvent();
    }

    public override void JoueeStart()
    {
        StartEvent();
    }

    public override void CompleteEvent()
    {
        try
        {
            SaveManager.instance.saveSystem.Save();
        }
        catch
        {
            Debug.Log("Save mananager is missing");
        }
        sceneController.SwitchSceneTo("MainMenu");
    }

    void StartEvent()
    {
        UpdateInfoAsync();
        if (SETTINGS.isGiveaway)
        {
            encore.TriggerDialogue();
        }
        else
        {
            explication.TriggerDialogue();
        }
    }

    async void UpdateInfoAsync()
    {
        await FirebaseStartupManager.instance.GetGiveawayDB()
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot dataSnapshot = task.Result;

                    if (dataSnapshot.Exists)
                    {
                        List<User> users = new List<User>();
                        foreach (var userInDB in dataSnapshot.Children)
                        {
                            User user = JsonUtility.FromJson<User>(userInDB.GetRawJsonValue());
                            users.Add(user);
                            if (userInDB.Key == SETTINGS.playerID)
                            {
                                formulaire.m_pseudoInputfield.text = user.username;
                                formulaire.m_contactInputfield.text = user.contact;
                                formulaire.m_hideToggle.isOn = !user.hidden;
                            }
                        }
                        leaderboard.UpdateLeaderboard(users);
                    }
                }
            });
    }
}
