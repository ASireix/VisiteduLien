using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebTextHandler
{

    public static IEnumerator GetText(string uri, System.Action<bool,string> callback){
        if (string.IsNullOrEmpty(uri)){
            Debug.Log("The url is empty");
            callback(true,"");
            yield break;
        }
        UnityWebRequest www = UnityWebRequest.Get(uri);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success){
            callback(true,"");
            Debug.Log(www.error);
        }else{
            callback(true,www.downloadHandler.text);
        }
    }
}
