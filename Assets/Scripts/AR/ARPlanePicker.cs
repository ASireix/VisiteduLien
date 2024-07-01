using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlanePicker : MonoBehaviour
{
    [SerializeField] float marginOfErrors;
    [SerializeField] Vector3 offset;
    [SerializeField] GameObject floorPrefab;
    GameObject _spawnedFloor;
    Camera _cam;
    ARRaycastManager _raycastManager;
    List<ARRaycastHit> _hits = new List<ARRaycastHit>();


    // Start is called before the first frame update
    void Start()
    {
        _raycastManager = Object.FindObjectOfType<ARRaycastManager>();
        _cam = Camera.main;
        var manager = Object.FindObjectOfType<ARPlaneManager>();

        manager.planesChanged += OnPlanesChanged;
        _spawnedFloor = Instantiate(floorPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFloorLevel();
    }

    public void OnPlanesChanged(ARPlanesChangedEventArgs changes)
    {

        foreach (var plane in changes.added)
        {
            
        }

        foreach (var plane in changes.updated)
        {
        
        }

        foreach (var plane in changes.removed)
        {

        }
    }

    void UpdateFloorLevel(){
        if (_raycastManager.Raycast(Vector3.down * 1000f,_hits)){
            var hitpose = _hits[0].pose;
            _spawnedFloor.transform.position=hitpose.position + offset;
            Debug.Log("HIT");
        }
    }
}
