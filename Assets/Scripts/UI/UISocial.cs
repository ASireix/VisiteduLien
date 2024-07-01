using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Link Sender",menuName = "Web/Link send")]
public class UISocial : ScriptableObject
{
    public void OpenLink(string url)
    {
        Application.OpenURL($"http://{url}");
    }
}
