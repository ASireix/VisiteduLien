using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Android;

public class GPS : MonoBehaviour
{
    public float latitude;
    public TextMeshProUGUI latText;
    public float longitude;
    public TextMeshProUGUI lonText;
    public TextMeshProUGUI debugText;
    public bool isUpdating;

    [SerializeField] List<float> qrCodePositions;
    Dictionary<int, (float, float)> qrPositionDico;

    private void Start()
    {
        StartCoroutine(StartGPS());   
    }

    IEnumerator StartGPS()
    {
        string log = "";
        // Check if the user has location service enabled.
        if (!Input.location.isEnabledByUser)
            Debug.Log("Location not enabled on device or app does not have permission to access location");

        // Starts the location service.
        Input.location.Start();

        // Waits until the location service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // If the service didn't initialize in 20 seconds this cancels location service use.
        if (maxWait < 1)
        {
            log = "Timed out";
            debugText.text = log;
            Debug.Log(log);
            yield return new WaitForSeconds(5);
        }

        // If the connection failed this cancels location service use.
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            log = "Unable to determine device location";
            debugText.text = log;
            Debug.LogError(log);
            yield break;
        }
        else
        {
            // If the connection succeeded, this retrieves the device's current location and displays it in the Console window.
            Debug.Log("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
            latText.text = Input.location.lastData.latitude+" _ la";
            lonText.text = Input.location.lastData.longitude+" _ lt";
        }

        // Stops the location service if there is no need to query location updates continuously.
        Input.location.Stop();
    }
}
