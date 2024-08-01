using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Formulaire : MonoBehaviour
{
    [Header("Formulaire")]
    [SerializeField] GameObject formulaireToToggle;
    public TMP_InputField pseudo_Inputfield;
    public TMP_InputField email_Inputfield;
    public TMP_InputField phone_Inputfield;
    public Toggle hideToggle;
    public Toggle subscribeToggle;

    public Button validateButton;
    [Space]
    [Header("Encore formulaire")]
    public TMP_InputField m_pseudoInputfield;
    public TMP_InputField m_emailInputfield;
    public TMP_InputField m_phoneInputfield;
    public Toggle m_hideToggle;
    [Space]

    public Leaderboard leaderboard;
    public GameObject errorMsg;
    private void Update()
    {
        validateButton.interactable = !string.IsNullOrEmpty(pseudo_Inputfield.text) && !string.IsNullOrEmpty(email_Inputfield.text);
    }

    public void Validate()
    {
        if (RegexUtilities.IsValidEmail(email_Inputfield.text))
        {
            LeaderboardEntry entry = new LeaderboardEntry(pseudo_Inputfield.text, email_Inputfield.text, phone_Inputfield.text, !hideToggle.isOn);
            leaderboard.PushElement(entry);
            formulaireToToggle.SetActive(false);
            if (subscribeToggle.isOn)
            {
                MailChimpNewsletter.AddSubscriber(pseudo_Inputfield.text, email_Inputfield.text);
            }
        }
        else
        {
            ShowErrorMsg();
        }
    }

    public void Remove()
    {
        leaderboard.RemoveElement();
    }

    void ShowErrorMsg()
    {
        errorMsg.SetActive(true);
    }
}

class RegexUtilities
{
    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            // Normalize the domain
            email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                  RegexOptions.None, TimeSpan.FromMilliseconds(200));

            // Examines the domain part of the email and normalizes it.
            string DomainMapper(Match match)
            {
                // Use IdnMapping class to convert Unicode domain names.
                var idn = new IdnMapping();

                // Pull out and process domain name (throws ArgumentException on invalid)
                string domainName = idn.GetAscii(match.Groups[2].Value);

                return match.Groups[1].Value + domainName;
            }
        }
        catch (RegexMatchTimeoutException e)
        {
            return false;
        }
        catch (ArgumentException e)
        {
            return false;
        }

        try
        {
            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }
}
