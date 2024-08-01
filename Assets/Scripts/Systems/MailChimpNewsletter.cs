using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

public class MailChimpNewsletter : MonoBehaviour
{
    private const string dataCenter = "us18"; // At the end of the API Key
    private const string apiKey = "74ca732aa50e8aa93aeb10d512a2ef4c-us18";
    private const string listId = "71edcd6438"; // Found in audience settings

    // Utility method to add a subscriber
    public static void AddSubscriber(string name, string email)
    {
        // I don't know how else to make this pretty. This is just harcoded JSON.
        string requestJson = "{\"email_address\": \"" + email + "\", \"status\": \"subscribed\", \"merge_fields\": {\"FNAME\": \"" + name + "\"}}";

        // This will add the subscriber to Mailchimp
        CallMailchimpApi(dataCenter, $"lists/{listId}/members", requestJson, apiKey);
    }
    private static void CallMailchimpApi(string dataCenter, string method, string requestJson, string key)
    {
        string endpoint = $"https://{dataCenter}.api.mailchimp.com/3.0/{method}";
        byte[] dataStream = Encoding.UTF8.GetBytes(requestJson);
        WebRequest request = WebRequest.Create(endpoint);
        try
        {
            request.ContentType = "application/json";
            SetBasicAuthHeader(request, "anystring", key); // BASIC AUTH
            request.Method = "POST";
            request.ContentLength = dataStream.Length;

            Stream newStream = request.GetRequestStream();
            newStream.Write(dataStream, 0, dataStream.Length);
            newStream.Close();

            WebResponse response = request.GetResponse();
            response.Close();
        }
        catch (WebException ex)
        {
            Debug.LogError(ex.Message);
            Debug.LogError(requestJson);

            Stream responseStream = ex.Response?.GetResponseStream();
            if (responseStream != null)
            {
                using StreamReader sr = new StreamReader(responseStream);
                Debug.LogError(sr.ReadToEnd());
            }
        }
    }

    private static void SetBasicAuthHeader(WebRequest request, string username, string password)
    {
        string auth = $"{username}:{password}";
        auth = Convert.ToBase64String(Encoding.Default.GetBytes(auth));
        request.Headers["Authorization"] = "Basic " + auth;
    }
}


