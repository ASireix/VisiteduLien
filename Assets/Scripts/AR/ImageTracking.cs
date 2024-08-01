using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TrackedEvenement
{
    public float cooldown;
    public float progress;

    public bool refreshed;

    public TrackedEvenement(float _cooldown, float _progress, bool _refreshed)
    {
        cooldown = _cooldown;
        progress = _progress;
        refreshed = _refreshed;
    }
}
public class ImageTracking : MonoBehaviour
{
    [SerializeField] EvenementScriptableObject[] placeablePrefabs;

    Dictionary<string, (GameObject, EvenementScriptableObject)> spawnedFlatPrefabs
    = new Dictionary<string, (GameObject, EvenementScriptableObject)>();
    Dictionary<string, (GameObject, EvenementScriptableObject)> spawnedARPrefabs
    = new Dictionary<string, (GameObject, EvenementScriptableObject)>();
    public ARTrackedImageManager trackedImageManager;
    [SerializeField] XRReferenceImageLibrary runtimeImageLibrary;
    [SerializeField] ARSession aRSession;
    public UnityEvent<float> onScanProgress { get; private set; } = new UnityEvent<float>();
    Dictionary<ARTrackedImage, TrackedEvenement> imagesProgress = new Dictionary<ARTrackedImage, TrackedEvenement>();

    [SerializeField] Image backgroundForFlat;

    [SerializeField] float eventCooldown = 5f;

    bool isInEvent;
    bool _tracking = true;
    string lastPrefabName;

    // launch settings for SETTINGS.isGuidee
    bool startGuidee;

    private void Awake()
    {
        startGuidee = SETTINGS.isGuidee;
        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();
        Evenement.onEventCompleted.AddListener(ReactivateARSession);
        foreach (var item in placeablePrefabs)
        {
            GameObject newPrefab;
            //Only spawn the ar event if you can
            if (item.arEvent != null)
            {
                GameObject anchor = new GameObject(item.name, typeof(ARAnchor));
                newPrefab = Instantiate(item.arEvent.gameObject, Vector3.zero, Quaternion.identity);
                newPrefab.transform.parent = anchor.transform;
                spawnedARPrefabs.Add(item.name, (anchor, item));
                anchor.SetActive(false);
                newPrefab.name = item.name;
            }
            //All events require a flat event to have list of mini game working
            newPrefab = Instantiate(item.flatEvent.gameObject, Vector3.zero, Quaternion.identity);
            newPrefab.transform.parent = Camera.main.transform;
            spawnedFlatPrefabs.Add(item.name, (newPrefab, item));
            newPrefab.SetActive(false);
            newPrefab.name = item.name;

            lastPrefabName = item.name;
        }
    }

    public void ToggleTracking(bool on)
    {
        _tracking = on;
    }

    private void Start()
    {
        UpdateImageLibrary(runtimeImageLibrary);
    }

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += ImageChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= ImageChanged;
    }

    void ReactivateARSession(EventData eventData)
    {
        backgroundForFlat.gameObject.SetActive(false);
        aRSession.GetComponent<ARSession>().enabled = true;
        aRSession.Reset();
        SETTINGS.isGuidee = startGuidee;
        isInEvent = false;
    }

    void UpdateImageLibrary(XRReferenceImageLibrary library)
    {
        trackedImageManager.enabled = false;
        trackedImageManager.referenceLibrary = trackedImageManager.CreateRuntimeLibrary(library);
        trackedImageManager.enabled = true;
    }

    void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        if (!_tracking) return;
        //Debug.Log("The image changed");
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            //Debug.Log("Image added");
            imagesProgress.TryAdd(trackedImage, new TrackedEvenement(eventCooldown, 0f, true));
            UpdateImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            //Debug.Log("Image updated");
            UpdateImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            //Debug.Log("Image Removde");
            imagesProgress[trackedImage].progress = 0f;
            if (spawnedFlatPrefabs.TryGetValue(trackedImage.referenceImage.name,
            out (GameObject, EvenementScriptableObject) value))
            {
                value.Item1.SetActive(false);
            }
        }
    }

    public async void LaunchLastEvent()
    {
        await Task.Delay(2000);
        Debug.Log("lastPrefabname = " + lastPrefabName);
        GameObject prefab = spawnedFlatPrefabs[lastPrefabName].Item1;
        prefab.SetActive(true);

        if (spawnedFlatPrefabs[lastPrefabName].Item2.useFlatEvent)
        {
            backgroundForFlat.gameObject.SetActive(true);
            backgroundForFlat.sprite = spawnedFlatPrefabs[lastPrefabName].Item2.backgroundImageForFlatEvent;
            aRSession.GetComponent<ARSession>().enabled = false;
        }

        isInEvent = true;
    }
    void UpdateImage(ARTrackedImage trackedImage)
    {
        if (!_tracking) return;
        //Use the name to get the corresponding prefab
        string name = trackedImage.referenceImage.name;
        trackedImage.transform.GetPositionAndRotation(out Vector3 position, out Quaternion rotation);
        //Debug.Log("Tracked image named " + name + " is at cooldown : " + imagesProgress[trackedImage].cooldown+" and is refreshed ? : "+ imagesProgress[trackedImage].refreshed);

        bool state = UpdatePrefabPosition(trackedImage, name, position, rotation);
        //Keep the ar evenement in place and stop the scan progression
        if (state || isInEvent) return;

        //Detect if the image is visible or the event is on cooldown
        UpdateScanProgress(trackedImage);

        //Spawn the event
        if (imagesProgress[trackedImage].progress > 0.99f)
        {
            SpawnEvent(trackedImage, name);
            
        }
    }

    bool UpdatePrefabPosition(ARTrackedImage trackedImage, string name, Vector3 position, Quaternion rotation)
    {
        //Check if there are AR events
        bool state = false;
        GameObject prefab;
        if (spawnedARPrefabs.TryGetValue(name,out (GameObject,EvenementScriptableObject) value))
        {
            prefab = value.Item1;
            state = prefab.activeInHierarchy;
            // If it's spawner, you update the position and rotation and
            if (state)
            {
                imagesProgress[trackedImage].progress = 0f;
                prefab.transform.SetPositionAndRotation(position, rotation);
                DisableInactiveEvents(name);
            }
        }
        state = state || spawnedFlatPrefabs[name].Item1.activeInHierarchy;
        
        return state;
    }

    void UpdateScanProgress(ARTrackedImage trackedImage)
    {
        if (trackedImage.trackingState == TrackingState.Tracking && imagesProgress[trackedImage].refreshed)
        {
            //Debug.Log("We are tracking"+name);
            imagesProgress[trackedImage].progress += Time.deltaTime;
            onScanProgress.Invoke(imagesProgress[trackedImage].progress);
        }
        else
        {
            imagesProgress[trackedImage].progress = 0f;
        }
    }

    void HandleEventCooldown(ARTrackedImage trackedImage)
    {
        imagesProgress[trackedImage].cooldown -= Time.deltaTime;

        if (imagesProgress[trackedImage].cooldown <= 0f)
        {
            Debug.Log("Resetting refresh with cooldown being : " + imagesProgress[trackedImage].cooldown);
            imagesProgress[trackedImage].cooldown = 0f;
            imagesProgress[trackedImage].refreshed = true;
        }
    }

    void DisableInactiveEvents(string name)
    {
        foreach (var go in spawnedFlatPrefabs.Values)
        {
            if (go.Item1.name != name)
            {
                go.Item1.SetActive(false);
            }
        }

        foreach (var go in spawnedARPrefabs.Values)
        {
            if (go.Item1.name != name)
            {
                go.Item1.SetActive(false);
            }
        }
    }

    void SpawnEvent(ARTrackedImage trackedImage, string name)
    {
        if (isInEvent) return;
        imagesProgress[trackedImage].refreshed = false;
        imagesProgress[trackedImage].progress = 0f;
        onScanProgress.Invoke(imagesProgress[trackedImage].progress);

        GameObject prefab;

        if (spawnedFlatPrefabs[name].Item2.useFlatEvent)
        {
            prefab = spawnedFlatPrefabs[name].Item1;
            backgroundForFlat.gameObject.SetActive(true);
            backgroundForFlat.sprite = spawnedFlatPrefabs[name].Item2.backgroundImageForFlatEvent;
            RectTransform rectTransform = backgroundForFlat.GetComponent<RectTransform>();
            Vector2 max = spawnedFlatPrefabs[name].Item2.right_top;
            Vector2 min = spawnedFlatPrefabs[name].Item2.left_bottom;
            rectTransform.SetTop(max.y);
            rectTransform.SetLeft(min.x);
            rectTransform.SetRight(max.x);
            rectTransform.SetBottom(min.y);
            aRSession.GetComponent<ARSession>().enabled = false;
        }
        else
        {
            prefab = spawnedARPrefabs[name].Item1;
        }
        prefab.SetActive(true);
        isInEvent = true;
        imagesProgress[trackedImage].cooldown = eventCooldown;
    }

    public void SpawnEventCustom(EvenementScriptableObject evt)
    {
        if (isInEvent) return;
        GameObject prefab = spawnedFlatPrefabs[evt.name].Item1;

        prefab.SetActive(true);

        backgroundForFlat.gameObject.SetActive(true);
        backgroundForFlat.sprite = evt.backgroundImageForFlatEvent;
        RectTransform rectTransform = backgroundForFlat.GetComponent<RectTransform>();
        Vector2 max = evt.right_top;
        Vector2 min = evt.left_bottom;
        rectTransform.SetTop(max.y);
        rectTransform.SetLeft(min.x);
        rectTransform.SetRight(max.x);
        rectTransform.SetBottom(min.y);
        aRSession.GetComponent<ARSession>().enabled = false;

        isInEvent = true;

        foreach (var key in imagesProgress)
        {
            key.Value.refreshed = false;
            key.Value.progress = 0f;
            onScanProgress.Invoke(key.Value.progress);
            key.Value.cooldown = eventCooldown;
        }
    }
}
