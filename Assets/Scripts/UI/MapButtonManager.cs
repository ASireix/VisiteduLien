using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapButtonManager : MonoBehaviour
{

    [SerializeField] List<GameObject> pinsBeauTemps;
    [SerializeField] List<GameObject> pinsBeurkTemps;
    [SerializeField] List<MapUpdateButton> mapUpdateButtons;
    [Tooltip("SAME ORDER AS MAPUPDATEBUTTONS")]
    [SerializeField] List<GameObject> ui;
    [SerializeField] TextMeshProUGUI categorieTitle;
    [SerializeField] int firstSelectId;
    [SerializeField] GameObject rainEffet;

    bool beauTemps = true;
    int lastId = 0;


    void Start(){
        foreach (var button in mapUpdateButtons)
        {
            button.callPinChange+=ChangePins;
        }
        ChangePins(mapUpdateButtons[firstSelectId].pinHolderID);
    }

    public void SwitchWeather()
    {
        beauTemps = !beauTemps;
        rainEffet.SetActive(!beauTemps);
        for (int i = 0; i < pinsBeauTemps.Count; i++)
        {
            pinsBeauTemps[i].SetActive(beauTemps && lastId==i);
            pinsBeurkTemps[i].SetActive(!beauTemps && lastId==i);
        }
    }
    void ChangePins(int id){
        lastId = id;
        for (int i = 0; i<mapUpdateButtons.Count; i++){
            if (i==id){

                if (beauTemps)
                {
                    pinsBeauTemps[mapUpdateButtons[i].pinHolderID].SetActive(true);
                }
                else
                {
                    pinsBeurkTemps[mapUpdateButtons[i].pinHolderID].SetActive(true);
                }
                mapUpdateButtons[i].SelectButton();
                ui[i].SetActive(true);
                categorieTitle.text = pinsBeauTemps[i].name;
            }else{
                pinsBeurkTemps[mapUpdateButtons[i].pinHolderID].SetActive(false);
                pinsBeauTemps[mapUpdateButtons[i].pinHolderID].SetActive(false);
                ui[i].SetActive(false);
                mapUpdateButtons[i].DeselectButton();
            }
        }
    }
}
