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
    Image image;
    [HideInInspector]
    public bool forceUnlock;

    void Start()
    {
        if(TryGetComponent(out image))
        {
            Debug.Log("image color is null ? : " + InfoManager.instance.lockColor == null);
            image.color = InfoManager.instance.lockColor;
        }
        if (TryGetComponent(out _renderer))
        {
            _renderer.materials[materialId].color = InfoManager.instance.lockColor;
        }
        
        InfoManager.instance.onInfoAdded.AddListener(UpdateGraphic);
        if (forceUnlock)
        {
            UpdateGraphic(true);
        }
        else
        {
            UpdateGraphic(InfoManager.instance.UpdatePinGraphic(eventD.Id));
        }
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (forceUnlock)
        {
            InfoManager.instance.ShowInfo(eventD);
        }
        else
        {
            InfoManager.instance.ShowInfo(eventD.Id);
        }
        
    }

    public void UpdateGraphic(bool yes)
    {
        Debug.Log("Trying to update grahics");
        if (yes)
        {
            if (image)
            {
                image.color = InfoManager.instance.unlockColor;
            }
            if (_renderer)
            {
                _renderer.materials[materialId].color = InfoManager.instance.unlockColor;
            }
        }
    }

    public void UpdateGraphic(string id)
    {
        Debug.Log("Trying to update grahics");
        if (id == eventD.Id)
        {
            if (image)
            {
                image.color = InfoManager.instance.unlockColor;
            }
            if (_renderer)
            {
                _renderer.materials[materialId].color = InfoManager.instance.unlockColor;
            }
        }
    }

    void Update()
    {
        if (!lookAtCam) return;
        transform.LookAt(Camera.main.transform.position, Vector3.up);
        //transform.forward = Camera.main.transform.forward;
    }
}
