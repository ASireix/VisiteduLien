using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class MapButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] EventData eventD;
    [SerializeField] bool lookAtCam;
    [SerializeField] int materialId;
    MeshRenderer _renderer;

    void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        _renderer.materials[materialId].color = InfoManager.instance.lockColor;
        InfoManager.instance.onInfoAdded.AddListener(UpdateGraphic);
        UpdateGraphic(InfoManager.instance.UpdatePinGraphic(eventD.Id));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        InfoManager.instance.ShowInfo(eventD.Id);
    }

    public void UpdateGraphic(bool yes)
    {
        Debug.Log("Trying to update grahics");
        if (yes)
        {
            _renderer.materials[materialId].color = InfoManager.instance.unlockColor;
        }
    }

    public void UpdateGraphic(string id)
    {
        Debug.Log("Trying to update grahics");
        if (id == eventD.Id)
        {
            _renderer.materials[materialId].color = InfoManager.instance.unlockColor;
        }
    }

    void Update()
    {
        if (!lookAtCam) return;
        transform.LookAt(Camera.main.transform.position, Vector3.up);
        //transform.forward = Camera.main.transform.forward;
    }
}
