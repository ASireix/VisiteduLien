using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Formulaire : MonoBehaviour
{
    [Header("Formulaire")]
    public TMP_InputField pseudo_Inputfield;
    public TMP_InputField contact_Inputfield;
    public Toggle hideToggle;

    public Button validateButton;

    [Space]
    [Header("Encore formulaire")]
    public TMP_InputField m_pseudoInputfield;
    public TMP_InputField m_contactInputfield;
    public Toggle m_hideToggle;
    [Space]

    public Leaderboard leaderboard;
    private void Update()
    {
        validateButton.interactable = !string.IsNullOrEmpty(pseudo_Inputfield.text) && !string.IsNullOrEmpty(contact_Inputfield.text);
    }

    public void Validate()
    {
        LeaderboardEntry entry = new LeaderboardEntry(pseudo_Inputfield.text, contact_Inputfield.text, !hideToggle.isOn);
        leaderboard.PushElement(entry);
    }

    public void Remove()
    {
        leaderboard.RemoveElement();
    }
}
