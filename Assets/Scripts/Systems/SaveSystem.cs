using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Système de sauvegarde définition : 
Chaque event pour chaque batiment contient un eventdata et un mapinfodigidata
Au début du jeu on load depuis les players prefs un dictionnaire avec un id et un bool
On assign le bool à l'event data correspondant qui signifie que l'event a été complété ou non
Quand le jeu commence, le code manager crack tous les code que le joueur avait dajà cracker grâce à l'id de l'event data
qui doit être le même que le MapInfoDigiData
*/

[CreateAssetMenu(fileName = "Save System", menuName = "Save System")]
public class SaveSystem : ScriptableObject
{
    Dictionary<string, bool> eventsSaves = new Dictionary<string, bool>();
    public EventData[] eventDatas { get; private set; }
    [SerializeField] UnitAssetPack unitAssetPack;


    void LoadAllEventDatas()
    {
        eventDatas = unitAssetPack.EventDatas;
        //eventDatas = Resources.LoadAll<EventData>("Event Datas"); OLD

        for (int i = 0; i < eventDatas.Length; i++)
        {
            eventDatas[i].ResetData();
            Debug.Log("Event data is "+eventDatas[i].name);
            if (eventsSaves.TryAdd(eventDatas[i].Id, eventDatas[i].isCompleted)){
                Debug.Log("Success");
            }else{
                Debug.Log("Fail");
            }
            
        }
    }

    public void LoadSave()
    {
        LoadAllEventDatas();

        string base64string = PlayerPrefs.GetString("savesys", "");

        if (base64string != "")
        {
            eventsSaves = Serializer.Load<Dictionary<string, bool>>(base64string);
            SaveData();
        }
        else
        {
            Save();
        }
    }

    public void Save()
    {
        UpdateDictionary();

        string base64Dictionary = Serializer.SaveToBase64(eventsSaves);
        PlayerPrefs.SetString("savesys", base64Dictionary);
        PlayerPrefs.Save();
    }

    void SaveData()
    {
        for (int i = 0; i < eventDatas.Length; i++)
        {
            eventDatas[i].isCompleted = eventsSaves[eventDatas[i].Id];
        }
    }

    void UpdateDictionary()
    {
        for (int i = 0; i < eventDatas.Length; i++)
        {
            eventsSaves[eventDatas[i].Id] = eventDatas[i].isCompleted;
        }
    }

    public void DeleteSaveFile()
    {
        PlayerPrefs.SetString("savesys", "");
        SETTINGS.isTutorialCompleted = false;
        for (int i = 0; i < eventDatas.Length; i++)
        {
            eventDatas[i].ResetData();
            eventsSaves.TryAdd(eventDatas[i].Id, eventDatas[i].isCompleted);
        }
        PlayerPrefs.Save();
    }
}
