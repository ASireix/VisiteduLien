using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARSubsystems;

public class ARCustomObjectSpawner : PressInputBase
{
    [SerializeField] ARRaycastManager aRRaycastManager;
    List<ARRaycastHit> _hits = new List<ARRaycastHit>();
    [SerializeField] GameObject prefabToSpawn;
    [SerializeField] bool keepPrefab;
    bool _isPressed;
    Transform _lastPrefabSpawned;

    [SerializeField] float minimalSpawnDistance;


    // Start is called before the first frame update
    void Start()
    {
        if (!aRRaycastManager)
        {
            aRRaycastManager = FindObjectOfType<ARRaycastManager>();
        }
    }
    private void Update()
    {
        if (Pointer.current == null || !_isPressed) return;

        var touchPosition = Pointer.current.position.ReadValue();

        if (aRRaycastManager.Raycast(touchPosition, _hits, TrackableType.PlaneWithinPolygon))
        {

            var hitPose = _hits[0].pose;
            

            if (_lastPrefabSpawned == null)
            {
                _lastPrefabSpawned = Instantiate(prefabToSpawn, hitPose.position, hitPose.rotation).transform;
            }
            else
            {
                float distance = Vector3.Distance(hitPose.position, _lastPrefabSpawned.position);
                if (distance > minimalSpawnDistance || minimalSpawnDistance < 0)
                {
                    _lastPrefabSpawned.position = hitPose.position;
                    _lastPrefabSpawned.rotation = hitPose.rotation;
                }
            }
        }
    }

    protected override void OnPress(Vector3 position)
    {
        base.OnPress(position);
        _isPressed = true;

        
    }

    protected override void OnPressCancel()
    {
        base.OnPressCancel();
        _isPressed = false;
        if (!keepPrefab) _lastPrefabSpawned = null;
    }

}
