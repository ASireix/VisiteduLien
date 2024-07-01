using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsChanger : MonoBehaviour
{
    public void ChangeVisiteType(bool guidee){
        SETTINGS.isGuidee = guidee;
    }
}
