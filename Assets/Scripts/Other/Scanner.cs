using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class QRCodeCoord
{
    public double latitude;
    public double longitude;
}

public class Scanner : MonoBehaviour
{
    [Tooltip("Format : (LATITUDE,LONGITUDE)")]
    [SerializeField] QRCodeCoord[] qrCodesPositions;
    [Tooltip("A lower refresh rate means the scanner will echo the location more often")]
    [SerializeField] float refreshRate;
    [SerializeField] TextMeshProUGUI distance;
    bool isScannerActif;

    double currentLatitude;
    double currentLongitude;

    public void StartScanner()
    {
        isScannerActif = true;
        StartCoroutine(ScannerCoro());
    }

    public void StopScanner()
    {
        isScannerActif = false;
    }

    IEnumerator ScannerCoro()
    {
        while (isScannerActif)
        {
            currentLatitude = NativeGPSPlugin.GetLatitude();
            currentLongitude = NativeGPSPlugin.GetLongitude();

            yield return new WaitForSeconds(refreshRate);

            distance.text = GetNearestQRCode() + "";
        }
        yield return null;
    }

    double GetNearestQRCode()
    {
        int code = -1;
        double dist = -1d;

        for (int index = 0; index < qrCodesPositions.Length; index++)
        {
            (double, double) qrPos = (qrCodesPositions[index].latitude, qrCodesPositions[index].longitude);
            (double, double) pos = (currentLatitude, currentLongitude);
            double newDist = GetDistance(qrPos, pos);

            if (newDist < dist || dist < 0d){
                code = index;
                dist = newDist;
            }
        }
        
        
        return dist;
        
    }

    double GetDistance((double, double) x, (double, double) y)
    {
        var x2 = (x.Item1 - x.Item2) * (x.Item1 - x.Item2);
        var y2 = (y.Item1 - y.Item2) * (y.Item1 - y.Item2);
        return System.Math.Abs(x2 + y2);
    }
}
