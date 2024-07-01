using System.Collections;
using System.Collections.Generic;
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

    Dictionary<string, (GameObject, EvenementScriptableObject)> spawnedPrefabs
    = new Dictionary<string, (GameObject, EvenementScriptableObject)>();
    public ARTrackedImageManager trackedImageManager;
    [SerializeField] XRReferenceImageLibrary runtimeImageLibrary;
    [SerializeField] ARSession aRSession;
    public UnityEvent<float> onScanProgress { get; private set; } = new UnityEvent<float>();
    Dictionary<ARTrackedImage, TrackedEvenement> imagesProgress = new Dictionary<ARTrackedImage, TrackedEvenement>();

    [SerializeField] Image backgroundForFlat;

    [SerializeField] float eventCooldown = 5f;

    bool isInEvent;

    private void Awake()
    {
        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();
        Evenement.onEventCompleted.AddListener(ReactivateARSession);
        foreach (var item in placeablePrefabs)
        {
            GameObject newPrefab;

            if (item.useFlatEvent)
            {
                newPrefab = Instantiate(item.flatEvent.gameObject, Vector3.zero, Quaternion.identity);
                newPrefab.transform.parent = Camera.main.transform;
                spawnedPrefabs.Add(item.name, (newPrefab, item));
                newPrefab.SetActive(false);
            }
            else
            {
                GameObject anchor = new GameObject(item.name, typeof(ARAnchor));
                newPrefab = Instantiate(item.arEvent.gameObject, Vector3.zero, Quaternion.identity);
                newPrefab.transform.parent = anchor.transform;
                spawnedPrefabs.Add(item.name, (anchor, item));
                anchor.SetActive(false);
            }
            newPrefab.name = item.name;
        }
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
            if (spawnedPrefabs.TryGetValue(trackedImage.referenceImage.name,
            out (GameObject, EvenementScriptableObject) value))
            {
                value.Item1.SetActive(false);
            }
        }
    }

    void UpdateImage(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;
        trackedImage.transform.GetPositionAndRotation(out Vector3 position, out Quaternion rotation);
        GameObject prefab = spawnedPrefabs[name].Item1;
        
        Debug.Log("Tracked image named " + name + " is at cooldown : " + imagesProgress[trackedImage].cooldown+" and is refreshed ? : "+ imagesProgress[trackedImage].refreshed);
        
        bool state = UpdatePrefabPosition(prefab, trackedImage, name, position, rotation);
        //Keep the ar evenement in place and stop the scan progression
        if (state || isInEvent) return;

        //Detect if the image is visible or the event is on cooldown
        UpdateScanProgress(trackedImage);

        //Spawn the event
        if (imagesProgress[trackedImage].progress > 0.99f)
        {
            SpawnEvent(prefab, trackedImage, name);
            isInEvent = true;
        }
    }

    bool UpdatePrefabPosition(GameObject prefab, ARTrackedImage trackedImage, string name, Vector3 position, Quaternion rotation)
    {
        bool state = prefab.activeInHierarchy;
        // If it's spawner, you update the position and rotation and 
        if (state)
        {
            imagesProgress[trackedImage].progress = 0f;

            if (!spawnedPrefabs[name].Item2.useFlatEvent)
            {
                prefab.transform.SetPositionAndRotation(position, rotation);
            }

            DisableInactiveEvents(name);
        }

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
        foreach (var go in spawnedPrefabs.Values)
        {
            if (go.Item1.name != name)
            {
                go.Item1.SetActive(false);
            }
        }
    }

    void SpawnEvent(GameObject prefab, ARTrackedImage trackedImage, string name)
    {
        imagesProgress[trackedImage].refreshed = false;
        imagesProgress[trackedImage].progress = 0f;
        onScanProgress.Invoke(imagesProgress[trackedImage].progress);

        prefab.SetActive(true);

        if (spawnedPrefabs[name].Item2.useFlatEvent)
        {
            backgroundForFlat.gameObject.SetActive(true);
            backgroundForFlat.sprite = spawnedPrefabs[name].Item2.backgroundImageForFlatEvent;
            aRSession.GetComponent<ARSession>().enabled = false;
        }

        imagesProgress[trackedImage].cooldown = eventCooldown;
    }
}