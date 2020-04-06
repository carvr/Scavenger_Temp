using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
[RequireComponent(typeof(ARTrackedImageManager))]
public class TrackedImageInfoMultipleManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] arObjectPrefabs;
    [SerializeField]
    private Vector3 scaleFactor = new Vector3(0.1f, 0.1f, 0.1f);
    // listener
    private ARTrackedImageManager m_TrackedImageManager;
    private Dictionary<string, GameObject> arObjects = new Dictionary<string, GameObject>();

    //new stuffs for listening, creates events to be pinged for listeners to hear
    public event Action<string> imageOnScreen;
    public event Action<string> imageOffScreen;

    void Awake()
    {
        m_TrackedImageManager = GetComponent<ARTrackedImageManager>();
        // setup all game objects in dictionary
        foreach (GameObject arObject in arObjectPrefabs)
        {
            GameObject newARObject = Instantiate(arObject, Vector3.zero, Quaternion.identity);
            newARObject.name = arObject.name;
            newARObject.SetActive(false);
            arObjects.Add(arObject.name, newARObject);
        }
    }
    void OnEnable()
    {
        m_TrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }
    void OnDisable()
    {
        m_TrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }
    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        //look at ARTrackedImageManager.cs Code if confused about how listeners work and what these parameters are
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateARImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateARImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            arObjects[trackedImage.name].SetActive(false);
        }
    }

    private void UpdateARImage(ARTrackedImage trackedImage)
    {
        if (arObjectPrefabs != null)
        {
            string name = trackedImage.referenceImage.name;
            GameObject goARObject = arObjects[name];

            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                // sends out ping for imageOnScreen to all listeners and sends image name
                if (imageOnScreen != null)
                {
                    imageOnScreen(trackedImage.referenceImage.name);
                }

                goARObject.SetActive(true);
                goARObject.transform.position = trackedImage.transform.position;
                goARObject.transform.rotation = trackedImage.transform.rotation;
                goARObject.transform.localScale = scaleFactor;
            }
            else
            {
                // sends out ping for imageOffScreen to all listeners and sends image name
                if (imageOffScreen != null)
                {
                    imageOffScreen(trackedImage.referenceImage.name);
                }

                goARObject.SetActive(false);
            }
        }
    }
}