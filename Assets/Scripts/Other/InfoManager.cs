using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InfoManager : MonoBehaviour
{
    public static InfoManager instance;
    Dictionary<string, GameObject> infoDico = new Dictionary<string, GameObject>();

    [ColorUsage(true, true)] public Color unlockColor;
    [ColorUsage(true, true)] public Color lockColor;
    [SerializeField] CanvasGroup notUnlockMsgBox;
    [System.NonSerialized] public UnityEvent<string> onInfoAdded = new UnityEvent<string>();

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void ShowInfo(string id, MapButton mapButton = null)
    {
        if (infoDico.TryGetValue(id, out GameObject info))
        {
            Debug.Log("Found ID");
            info.SetActive(true);
        }
        else
        {
            ShowMsgBox();
            Invoke("HideMsgBox",2f);
        }
    }

    public void ShowInfo(EventData eventD)
    {
        if (infoDico.TryGetValue(eventD.Id, out GameObject info))
        {
            info.SetActive(true);
        }
        else
        {
            GameObject obj = Instantiate(eventD.objectToShow);
            obj.transform.SetParent(transform, false);
            obj.SetActive(true);
            infoDico.TryAdd(eventD.Id, obj);
        }
    }

    public void AddInfo(string id, GameObject info)
    {
        GameObject obj = Instantiate(info);
        obj.transform.SetParent(transform, false);
        obj.SetActive(false);
        Debug.Log("Information added");
        if (infoDico.TryAdd(id, obj))
        {
            Debug.Log("Added info to dico");
            onInfoAdded?.Invoke(id);
        }
    }

    public bool UpdatePinGraphic(string id){
        return infoDico.ContainsKey(id);
    }

    void ShowMsgBox()
    {
        notUnlockMsgBox.alpha = 1f;
    }

    void HideMsgBox()
    {
        LeanTween.value(notUnlockMsgBox.gameObject, 1f, 0f, 2f).setOnUpdate((float val) =>
        {
            notUnlockMsgBox.alpha = val;
        });
    }
}
