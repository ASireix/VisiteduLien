using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoUnlockMap : MonoBehaviour
{
    private void Awake()
    {
        foreach (Transform t in transform)
        {
            if (t.TryGetComponent(out MapButton button))
            {
                button.forceUnlock = true;
            }
        }
    }
}
